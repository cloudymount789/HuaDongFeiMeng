using System.Linq;
using UnityEngine;

public class SnapManager : MonoBehaviour
{
    public float snapRadius = 0.5f;

    public bool TrySnapPart(BuildingPart part, Vector3 worldPos)
    {
        var slot = FindClosestFreeSlot(worldPos, part.partType);
        if (slot == null) return false;

        part.transform.position = slot.transform.position;
        slot.occupied = true;
        return true;
    }

    private SnapSlot FindClosestFreeSlot(Vector3 worldPos, BuildingPart.PartType partType)
    {
        var hits = Physics2D.OverlapCircleAll(worldPos, snapRadius);
        var slots = hits
            .Select(h => h.GetComponent<SnapSlot>())
            .Where(s => s != null && !s.occupied)
            .ToList();

        if (slots.Count == 0) return null;

        slots = slots.Where(s =>
                s.allowedTypes == null ||
                s.allowedTypes.Length == 0 ||
                s.allowedTypes.Contains(partType))
            .ToList();

        if (slots.Count == 0) return null;

        return slots
            .OrderBy(s => Vector3.SqrMagnitude(s.transform.position - worldPos))
            .First();
    }
}

