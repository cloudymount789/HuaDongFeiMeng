using UnityEngine;

/// <summary>
/// 结构校验（灰盒版）：先用“场景里是否存在某类 BuildingPart”来判断完成条件。
/// 后续再升级为真正的承重/连通性检查。
/// </summary>
public class StructureValidator : MonoBehaviour
{
    public bool Validate(LevelConfig level, out string reason)
    {
        reason = "OK";
        if (level == null)
        {
            reason = "Missing level config.";
            return false;
        }

        var parts = FindObjectsOfType<BuildingPart>();
        bool hasFrame = false;
        bool hasRoof = false;

        foreach (var p in parts)
        {
            if (p == null) continue;
            if (p.partType == BuildingPart.PartType.Frame) hasFrame = true;
            if (p.partType == BuildingPart.PartType.Roof) hasRoof = true;
        }

        if (level.requireFrame && !hasFrame)
        {
            reason = "Frame required.";
            return false;
        }

        if (level.requireRoof && !hasRoof)
        {
            reason = "Roof required.";
            return false;
        }

        return true;
    }
}

