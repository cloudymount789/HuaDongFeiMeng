using System.Collections.Generic;

public class KnowledgeUnlockState
{
    public HashSet<string> unlockedCardIds = new HashSet<string>();

    public bool IsUnlocked(string cardId) => unlockedCardIds.Contains(cardId);

    public void Unlock(string cardId)
    {
        if (!string.IsNullOrWhiteSpace(cardId)) unlockedCardIds.Add(cardId);
    }
}

