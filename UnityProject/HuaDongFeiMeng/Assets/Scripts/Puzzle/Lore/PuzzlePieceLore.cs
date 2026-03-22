using UnityEngine;

/// <summary>
/// 挂在拼图片上；已嵌入槽位后，由 <see cref="PuzzleLoreClick"/> 用右键（或Presenter绑定）逐句展示。
/// </summary>
public class PuzzlePieceLore : MonoBehaviour
{
    public PuzzleLoreLines data;

    private int _index;

    public void ResetProgress()
    {
        _index = 0;
    }

    /// <summary>下一句；已无内容时返回 null。</summary>
    public string AdvanceLine()
    {
        if (data == null || data.lines == null || data.lines.Length == 0) return null;
        if (_index >= data.lines.Length) return null;
        return data.lines[_index++];
    }

    public bool HasMore => data != null && data.lines != null && _index < data.lines.Length;
}
