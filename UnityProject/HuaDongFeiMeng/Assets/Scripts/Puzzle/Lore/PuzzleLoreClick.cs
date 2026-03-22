using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 右键点击已放置且带 <see cref="PuzzlePieceLore"/> 的拼图片 → <b>从头</b>播放 Lore（可重复观看）。
/// 「下一句」按钮由 <see cref="PuzzleLorePresenter"/> 继续往后播，不重置进度。
/// </summary>
public class PuzzleLoreClick : MonoBehaviour
{
    public Camera mainCamera;
    public PuzzleLorePresenter presenter;
    public LayerMask hitLayers = ~0;

    private void Update()
    {
        if (presenter == null) return;
        if (!Input.GetMouseButtonDown(1)) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        var w = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        w.z = 0f;

        foreach (var h in Physics2D.OverlapPointAll(w, hitLayers))
        {
            if (h == null) continue;
            var piece = h.GetComponentInParent<PuzzlePiece>();
            var lore = h.GetComponentInParent<PuzzlePieceLore>();
            if (piece == null || lore == null || !piece.IsPlaced) continue;

            lore.ResetProgress();
            presenter.ShowNext(lore);
            break;
        }
    }
}
