using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Lore 展示：默认 <b>运行时自动生成</b> 底栏 UGUI（无 prefab）；也可在 Inspector 手动指定 Text + Panel。
/// 右键由 <see cref="PuzzleLoreClick"/> 调 ShowNext(lore)（会先 Reset，从头看）；「下一句」按钮调 ShowNext() 续播。
/// </summary>
public class PuzzleLorePresenter : MonoBehaviour
{
    [Header("Manual assign (optional — leave empty to auto-build)")]
    [SerializeField] private Text bodyText;
    [SerializeField] private GameObject rootPanel;

    [Header("Auto UGUI")]
    [Tooltip("未手动指定 Body/Panel 时，在 Awake 生成整套 Canvas")]
    public bool autoBuildUI = true;

    public string panelTitle = "营造小记";
    public string nextButtonLabel = "下一句";

    private PuzzlePieceLore _current;

    private void Awake()
    {
        if (autoBuildUI && (bodyText == null || rootPanel == null))
            BuildLoreUI();

        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    public void BindOptionalText(Text text)
    {
        bodyText = text;
    }

    private void BuildLoreUI()
    {
        PuzzleUGUIUtil.EnsureEventSystem();
        var canvas = PuzzleUGUIUtil.CreateOverlayCanvas("PuzzleLoreCanvas", transform, 40);

        var panel = PuzzleUGUIUtil.CreatePanel("LoreBar", canvas.transform, new Color(0.14f, 0.11f, 0.1f, 0.88f));
        rootPanel = panel.gameObject;
        var prt = panel.GetComponent<RectTransform>();
        PuzzleUGUIUtil.SetBottomBar(prt, 220f);

        var font = PuzzleUGUIUtil.GetDefaultFont();

        var title = PuzzleUGUIUtil.CreateText("Title", rootPanel.transform, panelTitle, 24, font, TextAnchor.MiddleLeft, new Color(0.95f, 0.92f, 0.88f));
        var titleRt = title.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0f, 0.78f);
        titleRt.anchorMax = new Vector2(1f, 1f);
        titleRt.offsetMin = new Vector2(20f, 0f);
        titleRt.offsetMax = new Vector2(-160f, -6f);

        bodyText = PuzzleUGUIUtil.CreateText("Body", rootPanel.transform, string.Empty, 20, font, TextAnchor.UpperLeft, new Color(0.9f, 0.87f, 0.82f));
        var bodyRt = bodyText.GetComponent<RectTransform>();
        bodyRt.anchorMin = new Vector2(0f, 0.12f);
        bodyRt.anchorMax = new Vector2(1f, 0.78f);
        bodyRt.offsetMin = new Vector2(24f, 4f);
        bodyRt.offsetMax = new Vector2(-24f, -4f);

        var nextBtn = PuzzleUGUIUtil.CreateButton("NextBtn", rootPanel.transform, nextButtonLabel, font, () => ShowNext());
        var brt = nextBtn.GetComponent<RectTransform>();
        brt.anchorMin = new Vector2(1f, 1f);
        brt.anchorMax = new Vector2(1f, 1f);
        brt.pivot = new Vector2(1f, 1f);
        brt.sizeDelta = new Vector2(140f, 44f);
        brt.anchoredPosition = new Vector2(-12f, -8f);
    }

    /// <summary>对当前绑定的 Lore 取下一句；若传入 lore 则切换目标。</summary>
    public void ShowNext(PuzzlePieceLore lore = null)
    {
        if (lore != null)
            _current = lore;

        if (_current == null) return;

        var line = _current.AdvanceLine();
        if (string.IsNullOrEmpty(line))
        {
            Hide();
            _current = null;
            return;
        }

        if (rootPanel != null)
            rootPanel.SetActive(true);

        if (bodyText != null)
            bodyText.text = line;
        else
            Debug.Log($"[Lore] {line}");

        HfmSfxHub.Play(HfmSfxEvent.LoreLineAdvance);
    }

    public void Hide()
    {
        if (rootPanel != null)
            rootPanel.SetActive(false);
        if (bodyText != null)
            bodyText.text = string.Empty;
    }
}
