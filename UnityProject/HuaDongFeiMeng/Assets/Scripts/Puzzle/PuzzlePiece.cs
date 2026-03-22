using UnityEngine;

/// <summary>
/// 可拖拽拼图片：不允许旋转；嵌入槽位后可再次拖起取出（槽位会重新变为空）。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PuzzlePiece : MonoBehaviour
{
    public string pieceId;

    [Tooltip("拖起时相对指针的偏移")]
    public Vector3 dragOffset = Vector3.zero;

    /// <summary>从部件栏拖出时的世界坐标（放错槽时平滑回到这里）</summary>
    public Vector3 HomeWorldPosition { get; private set; }

    public bool IsPlaced { get; private set; }

    /// <summary>当前占用的槽（嵌入时设置，取出时清空）</summary>
    public PuzzleSlot CurrentSlot { get; private set; }

    public bool IsAnimating { get; set; }

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        c.isTrigger = false;
    }

    private void Awake()
    {
        HomeWorldPosition = transform.position;
    }

    /// <summary>在场景摆好后调用，或运行时由 Dock 设置</summary>
    public void SetHomeWorldPosition(Vector3 world)
    {
        HomeWorldPosition = world;
    }

    public void MarkPlaced(PuzzleSlot slot)
    {
        IsPlaced = true;
        CurrentSlot = slot;
        if (slot != null)
        {
            transform.SetParent(slot.transform, true);
            transform.position = slot.SnapWorldPosition;
        }
    }

    /// <summary>从槽位取出，准备拖拽（释放 occupied，解除父子关系）</summary>
    public void LiftFromSlotForDrag()
    {
        if (!IsPlaced) return;

        if (CurrentSlot != null)
        {
            CurrentSlot.occupied = false;
            CurrentSlot = null;
        }

        IsPlaced = false;
        transform.SetParent(null, true);
    }

    public void ResetPlaced()
    {
        if (CurrentSlot != null)
        {
            CurrentSlot.occupied = false;
            CurrentSlot = null;
        }
        IsPlaced = false;
        transform.SetParent(null, true);
    }
}
