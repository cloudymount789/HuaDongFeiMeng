using UnityEngine;

/// <summary>
/// 拼图槽位：与拼图片同 pieceId；表现上可用同 Sprite 灰模+透明度（在 Inspector 调 SpriteRenderer.color）。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PuzzleSlot : MonoBehaviour
{
    [Tooltip("只接受与此 id 相同的 PuzzlePiece")]
    public string pieceId;

    public bool occupied;

    [Tooltip("吸附判定的世界坐标半径（米）")]
    public float snapRadius = 0.6f;

    [Tooltip("若空则使用本物体 position")]
    public Transform snapPoint;

    [Tooltip("拼图片放入后 SpriteRenderer.sortingOrder；-1 表示保持拖起前的顺序")]
    public int placedPieceSortingOrder = -1;

    public Vector3 SnapWorldPosition => snapPoint != null ? snapPoint.position : transform.position;

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        c.isTrigger = true;
    }
}
