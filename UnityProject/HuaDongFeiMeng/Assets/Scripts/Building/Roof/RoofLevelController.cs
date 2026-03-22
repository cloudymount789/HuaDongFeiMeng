using UnityEngine;

/// <summary>
/// 屋顶建造垂直切控制器（灰盒版）：
/// - 用几个 Transform 代表：屋脊点、檐口点（或边界）
/// - 用刷子方式铺设椽条与瓦片（先按直线插值生成）
/// - 最终导出 RoofSnapshot（数据快照），后续可转换为可放置组件
/// </summary>
public class RoofLevelController : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public HfmInputRouter input;
    public ToolModeController toolMode;

    [Header("Prefabs")]
    public GameObject beamPrefab;
    public GameObject rafterPrefab;
    public GameObject tilePrefab;
    public GameObject ridgeDecorPrefab;

    [Header("Brush")]
    public float brushSpacing = 0.2f;

    [Header("Debug")]
    public bool logExportOnRelease = true;

    private RoofSnapshot _snapshot = new RoofSnapshot();

    private void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        if (PuzzleCoexistence.SuppressesLegacyInput()) return;

        if (input == null) input = FindObjectOfType<HfmInputRouter>();
        if (toolMode == null) toolMode = FindObjectOfType<ToolModeController>();

        var pointerWorld = input != null ? input.PointerWorld : GetLegacyPointerWorld();
        var down = input != null ? input.PressDown : Input.GetMouseButtonDown(0);
        var held = input != null ? input.PressHeld : Input.GetMouseButton(0);
        var up = input != null ? input.PressUp : Input.GetMouseButtonUp(0);

        var mode = toolMode != null ? toolMode.currentTool : ToolMode.PlaceBeam;

        if (mode == ToolMode.PlaceBeam && up)
        {
            Place(beamPrefab, pointerWorld, RoofSnapshot.ElementType.Beam);
        }

        if (mode == ToolMode.PaintRafter && held)
        {
            Paint(rafterPrefab, pointerWorld, RoofSnapshot.ElementType.Rafter);
        }

        if (mode == ToolMode.PaintTile && held)
        {
            Paint(tilePrefab, pointerWorld, RoofSnapshot.ElementType.Tile);
        }

        if (logExportOnRelease && down)
        {
            // no-op, reserved for UI hooks
        }

        if (logExportOnRelease && up)
        {
            var snap = ExportSnapshot();
            Debug.Log($"Roof snapshot: beams={snap.Count(RoofSnapshot.ElementType.Beam)} rafters={snap.Count(RoofSnapshot.ElementType.Rafter)} tiles={snap.Count(RoofSnapshot.ElementType.Tile)}");
        }
    }

    private void Place(GameObject prefab, Vector3 pos, RoofSnapshot.ElementType type)
    {
        if (prefab == null) return;
        var go = Instantiate(prefab, pos, Quaternion.identity);
        _snapshot.elements.Add(new RoofSnapshot.Element
        {
            type = type,
            position = go.transform.position,
            rotation = go.transform.rotation
        });
    }

    private void Paint(GameObject prefab, Vector3 pos, RoofSnapshot.ElementType type)
    {
        if (prefab == null) return;

        // 防止刷得过密：与上一次同类元素距离小于 spacing 则跳过
        var last = _snapshot.LastOfType(type);
        if (last != null && Vector3.Distance(last.position, pos) < brushSpacing) return;

        var go = Instantiate(prefab, pos, Quaternion.identity);
        _snapshot.elements.Add(new RoofSnapshot.Element
        {
            type = type,
            position = go.transform.position,
            rotation = go.transform.rotation
        });
    }

    public RoofSnapshot ExportSnapshot()
    {
        // 返回一个浅拷贝，避免外部修改内部列表
        return _snapshot.Clone();
    }

    private Vector3 GetLegacyPointerWorld()
    {
        var p = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0f;
        return p;
    }
}

