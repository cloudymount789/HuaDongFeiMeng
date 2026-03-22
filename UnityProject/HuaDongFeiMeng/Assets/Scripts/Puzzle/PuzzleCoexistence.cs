using UnityEngine;

/// <summary>
/// 当场景中存在已启用的 PuzzleDragRuntime 时，旧版放置（InputController / Roof / Town 等）应让路，避免同一次松手/点击重复生成物体。
/// </summary>
public static class PuzzleCoexistence
{
    public static bool SuppressesLegacyInput()
    {
        var p = Object.FindObjectOfType<PuzzleDragRuntime>();
        return p != null && p.isActiveAndEnabled;
    }
}
