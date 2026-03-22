using System.Collections;
using UnityEngine;

/// <summary>
/// 槽位拼图运行时：新 Input System（HfmInputRouter）下拖拽拼图片；
/// 松手时匹配 pieceId + 距离 → 缓慢滑入槽位；否则缓慢回到 Home。
/// </summary>
public class PuzzleDragRuntime : MonoBehaviour
{
    [Header("Refs")]
    public HfmInputRouter input;
    public Camera mainCamera;

    [Header("Snap")]
    public float snapRadius = 0.65f;

    [Header("Animation")]
    [Tooltip("拼图片滑入槽位的时长（秒）")]
    public float slideIntoSlotDuration = 0.45f;

    [Tooltip("放错槽回到部件栏原位的时长（秒）")]
    public float returnHomeDuration = 0.35f;

    [Header("Layers (optional)")]
    public LayerMask pieceLayers = ~0;
    public LayerMask slotLayers = ~0;

    [Header("Audio (optional)")]
    [Tooltip("若空则尝试 HfmSfxHub / 场景内 HfmSfxPlayer")]
    public HfmSfxPlayer sfxPlayer;

    private PuzzlePiece _dragging;
    private Vector3 _grabOffset;
    private bool _busy;
    private int _dragStartSortOrder;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (input == null) input = FindObjectOfType<HfmInputRouter>();
    }

    private void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null || input == null) return;
        if (_busy) return;

        var pw = input.PointerWorld;

        if (input.PressDown)
        {
            TryBeginDrag(pw);
        }

        if (_dragging != null && input.PressHeld)
        {
            _dragging.transform.position = pw + _grabOffset;
        }

        if (input.PressUp && _dragging != null)
        {
            EndDrag(pw);
        }
    }

    private void TryBeginDrag(Vector3 pointerWorld)
    {
        // 同一点可能叠在槽位 Trigger 与拼图片上，OverlapPoint 只返回其一；优先拾取 PuzzlePiece
        var hits = Physics2D.OverlapPointAll(pointerWorld, pieceLayers);
        PuzzlePiece piece = null;
        foreach (var h in hits)
        {
            if (h == null) continue;
            var p = h.GetComponentInParent<PuzzlePiece>();
            if (p != null)
            {
                piece = p;
                break;
            }
        }

        if (piece == null || piece.IsAnimating) return;

        if (piece.IsPlaced)
            piece.LiftFromSlotForDrag();

        _dragging = piece;
        _grabOffset = piece.dragOffset + (piece.transform.position - pointerWorld);

        _dragStartSortOrder = CacheAndBoostSorting(piece);
        PlaySfx(HfmSfxEvent.PuzzlePiecePickup);
    }

    private void EndDrag(Vector3 pointerWorld)
    {
        var piece = _dragging;
        _dragging = null;
        if (piece == null) return;

        var slot = FindBestSlot(pointerWorld, piece.pieceId);
        if (slot != null && !slot.occupied)
        {
            int baseSort = _dragStartSortOrder;
            StartCoroutine(AnimateSnapThenLock(piece, slot, baseSort));
        }
        else
        {
            StartCoroutine(AnimateReturnHome(piece));
        }
    }

    private static int CacheAndBoostSorting(PuzzlePiece piece)
    {
        var sr = piece.GetComponent<SpriteRenderer>();
        if (sr == null) return 0;
        int b = sr.sortingOrder;
        sr.sortingOrder = b + 100;
        return b;
    }

    private static void ApplyPlacedSorting(PuzzlePiece piece, PuzzleSlot slot, int baseSortBeforeDrag)
    {
        var sr = piece.GetComponent<SpriteRenderer>();
        if (sr == null) return;
        if (slot != null && slot.placedPieceSortingOrder >= 0)
            sr.sortingOrder = slot.placedPieceSortingOrder;
        else
            sr.sortingOrder = baseSortBeforeDrag;
    }

    private void RestoreSorting(PuzzlePiece piece)
    {
        var sr = piece.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = _dragStartSortOrder;
    }

    private PuzzleSlot FindBestSlot(Vector3 worldPos, string pid)
    {
        if (string.IsNullOrEmpty(pid)) return null;

        PuzzleSlot best = null;
        float bestD = float.MaxValue;

        foreach (var slot in FindObjectsOfType<PuzzleSlot>())
        {
            if (slot == null || slot.occupied) continue;
            if (slot.pieceId != pid) continue;

            float r = slot.snapRadius > 0 ? slot.snapRadius : snapRadius;
            var sp = slot.SnapWorldPosition;
            float d = Vector2.SqrMagnitude((Vector2)(worldPos - sp));
            if (d <= r * r && d < bestD)
            {
                bestD = d;
                best = slot;
            }
        }

        return best;
    }

    private IEnumerator AnimateSnapThenLock(PuzzlePiece piece, PuzzleSlot slot, int baseSortBeforeDrag)
    {
        _busy = true;
        piece.IsAnimating = true;

        var from = piece.transform.position;
        var to = slot.SnapWorldPosition;
        yield return SmoothMove(piece.transform, from, to, slideIntoSlotDuration);

        slot.occupied = true;
        piece.MarkPlaced(slot);
        ApplyPlacedSorting(piece, slot, baseSortBeforeDrag);
        PlaySfx(HfmSfxEvent.PuzzlePieceSnapSuccess);
        piece.IsAnimating = false;
        _busy = false;
    }

    private IEnumerator AnimateReturnHome(PuzzlePiece piece)
    {
        _busy = true;
        piece.IsAnimating = true;
        PlaySfx(HfmSfxEvent.PuzzlePieceSnapReject);

        var from = piece.transform.position;
        var to = piece.HomeWorldPosition;
        yield return SmoothMove(piece.transform, from, to, returnHomeDuration);

        RestoreSorting(piece);
        piece.IsAnimating = false;
        _busy = false;
    }

    private static IEnumerator SmoothMove(Transform t, Vector3 from, Vector3 to, float duration)
    {
        if (duration <= 0f)
        {
            t.position = to;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float u = Mathf.Clamp01(elapsed / duration);
            u = Mathf.SmoothStep(0f, 1f, u);
            t.position = Vector3.Lerp(from, to, u);
            yield return null;
        }

        t.position = to;
    }

    private void PlaySfx(HfmSfxEvent e)
    {
        if (e == HfmSfxEvent.None) return;
        var r = sfxPlayer != null ? (IHfmSfx)sfxPlayer : HfmSfxHub.Resolve();
        r?.Play(e);
    }

}
