using UnityEngine;

[CreateAssetMenu(menuName = "HuadongFeimeng/Knowledge/Card", fileName = "KnowledgeCard_")]
public class KnowledgeCardData : ScriptableObject
{
    [Header("Identity")]
    public string cardId;
    public string title;

    [Header("Content")]
    [TextArea(6, 20)]
    public string body;

    [Header("Media")]
    public Sprite illustration;

    [Header("Tags")]
    public string[] tags;
}

