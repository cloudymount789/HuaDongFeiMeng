using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 当场景中所有 <see cref="PuzzleSlot"/> 均为 occupied 时，弹出一次完成提示（运行时生成 UGUI）。
/// </summary>
public class PuzzleLevelCompleteUI : MonoBehaviour
{
    [Header("Behaviour")]
    [Tooltip("开始时缓存槽位；若运行时动态增槽请改代码或重进场景")]
    public bool autoFindSlotsOnStart = true;

    [Tooltip("完成后禁用拖拽，避免误触")]
    public bool disableDragRuntimeWhenComplete = true;

    [Header("UI")]
    public bool autoBuildUi = true;
    public string title = "搭建完成";
    [TextArea] public string message = "本关构件已全部就位。";
    public string closeButtonLabel = "好的";

    private PuzzleSlot[] _slots;
    private bool _completed;
    private GameObject _uiRoot;
    private PuzzleDragRuntime _drag;

    private void Start()
    {
        if (autoFindSlotsOnStart) RefreshSlots();
        _drag = FindObjectOfType<PuzzleDragRuntime>();
        if (autoBuildUi) BuildUi();
        HidePanel();
    }

    public void RefreshSlots()
    {
        _slots = FindObjectsOfType<PuzzleSlot>();
    }

    private void Update()
    {
        if (_completed || _slots == null || _slots.Length == 0) return;

        foreach (var s in _slots)
        {
            if (s == null || !s.occupied) return;
        }

        _completed = true;
        ShowPanel();
        HfmSfxHub.Play(HfmSfxEvent.PuzzleLevelComplete);

        if (disableDragRuntimeWhenComplete && _drag != null)
            _drag.enabled = false;
    }

    private void BuildUi()
    {
        if (_uiRoot != null) return;

        PuzzleUGUIUtil.EnsureEventSystem();
        var canvas = PuzzleUGUIUtil.CreateOverlayCanvas("PuzzleLevelCompleteCanvas", transform, 200);
        _uiRoot = canvas.gameObject;
        _uiRoot.transform.SetAsLastSibling();

        var dim = PuzzleUGUIUtil.CreatePanel("Dim", canvas.transform, new Color(0f, 0f, 0f, 0.45f));
        var dimRt = dim.GetComponent<RectTransform>();
        PuzzleUGUIUtil.StretchFull(dimRt);

        var box = PuzzleUGUIUtil.CreatePanel("Box", dim.transform, new Color(0.98f, 0.96f, 0.93f, 1f));
        var boxRt = box.GetComponent<RectTransform>();
        boxRt.anchorMin = new Vector2(0.5f, 0.5f);
        boxRt.anchorMax = new Vector2(0.5f, 0.5f);
        boxRt.pivot = new Vector2(0.5f, 0.5f);
        boxRt.sizeDelta = new Vector2(520f, 280f);
        boxRt.anchoredPosition = Vector2.zero;

        var font = PuzzleUGUIUtil.GetDefaultFont();

        var titleTxt = PuzzleUGUIUtil.CreateText("Title", box.transform, title, 34, font, TextAnchor.UpperCenter, new Color(0.2f, 0.16f, 0.14f));
        var titleRt = titleTxt.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0f, 0.65f);
        titleRt.anchorMax = new Vector2(1f, 1f);
        titleRt.offsetMin = new Vector2(24f, 0f);
        titleRt.offsetMax = new Vector2(-24f, -16f);

        var bodyTxt = PuzzleUGUIUtil.CreateText("Body", box.transform, message, 22, font, TextAnchor.MiddleCenter, new Color(0.35f, 0.3f, 0.28f));
        var bodyRt = bodyTxt.GetComponent<RectTransform>();
        bodyRt.anchorMin = new Vector2(0f, 0.28f);
        bodyRt.anchorMax = new Vector2(1f, 0.65f);
        bodyRt.offsetMin = new Vector2(28f, 0f);
        bodyRt.offsetMax = new Vector2(-28f, 0f);

        var btn = PuzzleUGUIUtil.CreateButton("CloseBtn", box.transform, closeButtonLabel, font, HidePanel);
        var bRect = btn.GetComponent<RectTransform>();
        bRect.anchorMin = new Vector2(0.5f, 0f);
        bRect.anchorMax = new Vector2(0.5f, 0f);
        bRect.pivot = new Vector2(0.5f, 0f);
        bRect.sizeDelta = new Vector2(220f, 52f);
        bRect.anchoredPosition = new Vector2(0f, 20f);
    }

    private void ShowPanel()
    {
        if (_uiRoot != null) _uiRoot.SetActive(true);
    }

    private void HidePanel()
    {
        if (_uiRoot != null) _uiRoot.SetActive(false);
    }
}
