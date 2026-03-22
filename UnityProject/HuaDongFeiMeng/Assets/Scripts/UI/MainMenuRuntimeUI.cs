using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 主菜单（运行时生成 UGUI）：标题 + 进入关卡场景。需在 Build Settings 中加入本场景与目标关卡场景。
/// </summary>
public class MainMenuRuntimeUI : MonoBehaviour
{
    [Header("Scenes")]
    [Tooltip("File -> Build Settings 里要有的场景名（不含 .unity）")]
    public string gameSceneName = "Puzzle_VerticalSlice";

    [Header("Copy")]
    public bool autoBuild = true;
    public string title = "画栋飞甍";
    public string subtitle = "厅堂造 · 槽位拼图";
    public string startButtonLabel = "开始搭建";

    private void Start()
    {
        if (autoBuild)
            BuildUi();
    }

    private void BuildUi()
    {
        PuzzleUGUIUtil.EnsureEventSystem();
        var canvas = PuzzleUGUIUtil.CreateOverlayCanvas("MainMenuCanvas", transform, 10);
        var font = PuzzleUGUIUtil.GetDefaultFont();

        var bg = PuzzleUGUIUtil.CreatePanel("Bg", canvas.transform, new Color(0.96f, 0.94f, 0.9f, 1f));
        PuzzleUGUIUtil.StretchFull(bg.GetComponent<RectTransform>());

        var titleTxt = PuzzleUGUIUtil.CreateText("Title", bg.transform, title, 52, font, TextAnchor.MiddleCenter, new Color(0.22f, 0.16f, 0.14f));
        var trt = titleTxt.GetComponent<RectTransform>();
        trt.anchorMin = new Vector2(0f, 0.55f);
        trt.anchorMax = new Vector2(1f, 0.85f);
        trt.offsetMin = Vector2.zero;
        trt.offsetMax = Vector2.zero;

        var subTxt = PuzzleUGUIUtil.CreateText("Subtitle", bg.transform, subtitle, 22, font, TextAnchor.MiddleCenter, new Color(0.4f, 0.35f, 0.32f));
        var srt = subTxt.GetComponent<RectTransform>();
        srt.anchorMin = new Vector2(0f, 0.45f);
        srt.anchorMax = new Vector2(1f, 0.58f);
        srt.offsetMin = Vector2.zero;
        srt.offsetMax = Vector2.zero;

        var btn = PuzzleUGUIUtil.CreateButton("StartBtn", bg.transform, startButtonLabel, font, LoadGame);
        var brt = btn.GetComponent<RectTransform>();
        brt.anchorMin = new Vector2(0.5f, 0.28f);
        brt.anchorMax = new Vector2(0.5f, 0.28f);
        brt.pivot = new Vector2(0.5f, 0.5f);
        brt.sizeDelta = new Vector2(280f, 56f);
        brt.anchoredPosition = Vector2.zero;
    }

    public void LoadGame()
    {
        HfmSfxHub.Play(HfmSfxEvent.UIClick);
        if (string.IsNullOrWhiteSpace(gameSceneName))
        {
            Debug.LogError("MainMenuRuntimeUI: gameSceneName 为空。");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }
}
