using UnityEngine;

/// <summary>
/// 与构件 id 绑定的逐句科普文案（VN 式一句一句点）。
/// </summary>
[CreateAssetMenu(menuName = "HuadongFeimeng/Puzzle/LoreLines", fileName = "PuzzleLoreLines_")]
public class PuzzleLoreLines : ScriptableObject
{
    [Tooltip("与 PuzzlePiece.pieceId 一致时可自动校验")]
    public string pieceId;

    [TextArea(2, 6)]
    public string[] lines;
}
