using UnityEngine;

/// <summary>
/// 结算面板（灰盒版）：先用 Debug.Log 代替真实 UI。
/// 后续接入 Unity UI Toolkit 或 UGUI。
/// </summary>
public class LevelResultPanel : MonoBehaviour
{
    public void ShowSuccess(LevelConfig level)
    {
        Debug.Log($"Level success: {(level != null ? level.displayName : "(null)")}");
    }

    public void ShowFail(string reason)
    {
        Debug.LogWarning($"Level failed: {reason}");
    }
}

