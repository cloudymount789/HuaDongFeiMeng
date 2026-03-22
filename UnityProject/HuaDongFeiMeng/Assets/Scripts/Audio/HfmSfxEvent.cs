/// <summary>
/// 音效事件 id。新增事件请在末尾追加，避免打乱已配置的序列化数据。
/// </summary>
public enum HfmSfxEvent
{
    None = 0,

    // --- 槽位拼图 ---
    PuzzlePiecePickup = 100,
    PuzzlePieceSnapSuccess = 101,
    PuzzlePieceSnapReject = 102,
    PuzzleLevelComplete = 103,

    // --- UI（预留）---
    UIClick = 200,
    UIOpen = 201,
    LoreLineAdvance = 202,

    // --- 预留区间 300+ ---
}
