using UnityEngine;

/// <summary>
/// 可选：在场景中拖一个 HfmSfxPlayer 到这里，避免各处 FindObjectOfType。
/// 若未赋值，会在首次调用时尝试 FindObjectOfType&lt;HfmSfxPlayer&gt;。
/// </summary>
public static class HfmSfxHub
{
    private static HfmSfxPlayer _override;

    public static void SetPlayer(HfmSfxPlayer player)
    {
        _override = player;
    }

    public static IHfmSfx Resolve()
    {
        if (_override != null) return _override;
        return Object.FindObjectOfType<HfmSfxPlayer>();
    }

    public static void Play(HfmSfxEvent e)
    {
        var p = Resolve();
        p?.Play(e);
    }
}
