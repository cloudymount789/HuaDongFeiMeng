using UnityEngine;

public class InputController : MonoBehaviour
{
    [Header("Placement")]
    public GameObject currentPartPrefab;
    public Camera mainCamera;
    public SnapManager snapManager;

    [Header("Input (New Input System preferred)")]
    public HfmInputRouter input;
    public ToolModeController toolMode;

    [Tooltip("若场景里存在已启用的 PuzzleDragRuntime，则本脚本完全不处理放置（避免松手时多 Instantiate 一份）")]
    public bool respectPuzzleMode = true;

    private GameObject _previewInstance;

    private bool PuzzleModeBlocksLegacyPlacement()
    {
        if (!respectPuzzleMode) return false;
        return PuzzleCoexistence.SuppressesLegacyInput();
    }

    private void Update()
    {
        if (PuzzleModeBlocksLegacyPlacement())
        {
            if (_previewInstance != null)
            {
                Destroy(_previewInstance);
                _previewInstance = null;
            }
            return;
        }

        if (input == null) input = FindObjectOfType<HfmInputRouter>();
        if (toolMode == null) toolMode = FindObjectOfType<ToolModeController>();
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 暂时只在“放梁/放置”工具下启用吸附放置，避免与屋顶刷子冲突
        var mode = toolMode != null ? toolMode.currentTool : ToolMode.PlaceBeam;
        if (mode != ToolMode.PlaceBeam) return;

        EnsurePreview();
        var pointerWorld = input != null ? input.PointerWorld : GetLegacyPointerWorld();
        if (_previewInstance != null) _previewInstance.transform.position = pointerWorld;

        // 拖拽放置：抬起时确认
        if (input != null)
        {
            if (input.PressUp) PlaceAt(pointerWorld);
        }
        else
        {
            if (Input.GetMouseButtonUp(0)) PlaceAt(pointerWorld);
        }
    }

    public void SetCurrentPartPrefab(GameObject prefab)
    {
        currentPartPrefab = prefab;
        if (_previewInstance != null) Destroy(_previewInstance);
        _previewInstance = null;
        EnsurePreview();
    }

    private void EnsurePreview()
    {
        if (_previewInstance != null) return;
        if (currentPartPrefab == null) return;
        _previewInstance = Instantiate(currentPartPrefab);
    }

    private void PlaceAt(Vector3 worldPos)
    {
        if (PuzzleModeBlocksLegacyPlacement()) return;
        if (currentPartPrefab == null || snapManager == null) return;

        var go = Instantiate(currentPartPrefab);
        var part = go.GetComponent<BuildingPart>();
        if (part == null)
        {
            Debug.LogWarning("Prefab missing BuildingPart.");
            Destroy(go);
            return;
        }

        var snapped = snapManager.TrySnapPart(part, worldPos);
        if (!snapped) Destroy(go);
    }

    private Vector3 GetLegacyPointerWorld()
    {
        var p = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0f;
        return p;
    }
}

