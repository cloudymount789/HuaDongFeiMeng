using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 斗拱小关卡控制器（灰盒版）：
/// - 场景中放置一批 DouGongSlot 作为槽位
/// - 玩家用 InputController + SnapManager 放置 DouGong 原子件到槽位附近
/// - 校验规则（DouGongLayoutRule），满足后可“完成”并产出一个组件快照
/// </summary>
public class DouGongLevelController : MonoBehaviour
{
    [Header("Rule")]
    public DouGongLayoutRule layoutRule;

    [Header("Slots")]
    public List<DouGongSlot> slots = new List<DouGongSlot>();

    [Header("Placed Parts (runtime)")]
    public List<BuildingPart> placedParts = new List<BuildingPart>();

    [Header("Debug")]
    public KeyCode validateKey = KeyCode.V;
    public KeyCode completeKey = KeyCode.C;

    private void Awake()
    {
        if (slots.Count == 0)
        {
            slots = FindObjectsOfType<DouGongSlot>().ToList();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(validateKey))
        {
            Debug.Log($"DouGong validate: {Validate(out var reason)} ({reason})");
        }

        if (Input.GetKeyDown(completeKey))
        {
            if (!Validate(out var reason))
            {
                Debug.LogWarning($"Cannot complete: {reason}");
                return;
            }

            var snapshot = ExportSnapshot();
            Debug.Log($"DouGong completed. Snapshot parts={snapshot.parts.Count}");
        }
    }

    public bool Validate(out string reason)
    {
        reason = "OK";
        if (layoutRule == null)
        {
            reason = "Missing layoutRule.";
            return false;
        }

        // 统计每个 slot 是否有 DouGong 部件落在附近（先用距离近似）
        var slotMap = new Dictionary<string, BuildingPart>();
        foreach (var slot in slots)
        {
            if (slot == null) continue;
            var nearest = FindNearestDouGongPart(slot.transform.position, 0.25f);
            if (nearest != null)
            {
                slot.occupied = true;
                slotMap[slot.slotId] = nearest;
            }
            else
            {
                slot.occupied = false;
            }
        }

        // 按 rule 校验
        foreach (var sr in layoutRule.slots)
        {
            var hasPart = slotMap.TryGetValue(sr.slotId, out var part);
            if (sr.required && !hasPart)
            {
                reason = $"Required slot missing: {sr.slotId}";
                return false;
            }

            if (hasPart)
            {
                // 暂用 BuildingPart.partId 作为 atomType 的区分（后续可绑定 DouGongPartData）
                if (sr.allowedAtoms != null && sr.allowedAtoms.Length > 0)
                {
                    var ok = sr.allowedAtoms.Any(a => part.partId != null && part.partId.ToLower().Contains(a.ToString().ToLower()));
                    if (!ok)
                    {
                        reason = $"Slot {sr.slotId} has disallowed atom: {part.partId}";
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private BuildingPart FindNearestDouGongPart(Vector3 pos, float radius)
    {
        var hits = Physics2D.OverlapCircleAll(pos, radius);
        foreach (var h in hits)
        {
            var bp = h.GetComponent<BuildingPart>();
            if (bp != null && bp.partType == BuildingPart.PartType.DouGong) return bp;
        }
        return null;
    }

    public DouGongSnapshot ExportSnapshot()
    {
        var snap = new DouGongSnapshot();
        snap.layoutType = layoutRule.layoutType.ToString();

        foreach (var slot in slots)
        {
            if (slot == null) continue;
            var part = FindNearestDouGongPart(slot.transform.position, 0.25f);
            if (part == null) continue;

            snap.parts.Add(new DouGongSnapshot.Part
            {
                slotId = slot.slotId,
                partId = part.partId,
                localPosition = slot.transform.localPosition
            });
        }

        return snap;
    }
}

public class DouGongSnapshot
{
    public string layoutType;

    public class Part
    {
        public string slotId;
        public string partId;
        public Vector3 localPosition;
    }

    public List<Part> parts = new List<Part>();
}

