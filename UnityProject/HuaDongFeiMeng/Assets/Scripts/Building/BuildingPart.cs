using UnityEngine;

public class BuildingPart : MonoBehaviour
{
    public enum PartType
    {
        Frame,
        Wall,
        Door,
        Window,
        Roof,
        DouGong
    }

    public string partId;
    public PartType partType;

    public int widthInCells = 1;
    public int heightInCells = 1;
}

