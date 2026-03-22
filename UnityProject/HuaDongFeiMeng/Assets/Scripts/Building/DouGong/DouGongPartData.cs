using UnityEngine;

[CreateAssetMenu(menuName = "HuadongFeimeng/DouGong/PartData", fileName = "DouGongPartData_")]
public class DouGongPartData : ScriptableObject
{
    public enum AtomType
    {
        LuDou,
        Gong,
        Ang,
        SanDou
    }

    public string id;
    public AtomType atomType;
    public Sprite icon;
}

