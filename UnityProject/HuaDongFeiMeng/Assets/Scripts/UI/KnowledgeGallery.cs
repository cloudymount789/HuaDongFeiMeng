using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图鉴/知识卡片浏览（灰盒版）。
/// 先用 Debug.Log 输出已解锁的卡片，后续替换为真实 UI。
/// </summary>
public class KnowledgeGallery : MonoBehaviour
{
    public List<KnowledgeCardData> allCards = new List<KnowledgeCardData>();

    public void Show(KnowledgeUnlockState state)
    {
        if (state == null)
        {
            Debug.LogWarning("KnowledgeGallery: missing state.");
            return;
        }

        foreach (var c in allCards)
        {
            if (c == null) continue;
            if (state.IsUnlocked(c.cardId))
            {
                Debug.Log($"[Unlocked] {c.title}");
            }
        }
    }
}

