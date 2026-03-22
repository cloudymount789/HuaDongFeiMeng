using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HuadongFeimeng/Level/LevelConfig", fileName = "LevelConfig_")]
public class LevelConfig : ScriptableObject
{
    [Header("Identity")]
    public string levelId;
    public string displayName;

    [Header("Goal")]
    [TextArea] public string goalText;

    [Header("Allowed Parts (by id prefix or exact id)")]
    public List<string> allowedPartIds = new List<string>();

    [Header("Completion Requirements")]
    public bool requireFrame = true;
    public bool requireRoof = true;

    [Tooltip("Optional: required roof type id (e.g. wudian/xieshan...)")]
    public string requiredRoofTypeId;

    [Header("Rewards")]
    public List<KnowledgeCardData> rewardCards = new List<KnowledgeCardData>();
}

