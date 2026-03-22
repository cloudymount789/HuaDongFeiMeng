using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TownSaveData
{
    [Serializable]
    public class PlacedBuilding
    {
        public string buildingId;
        public Vector2Int gridPos;
        public int rotation; // 0/90/180/270, reserved
    }

    public List<PlacedBuilding> buildings = new List<PlacedBuilding>();
}

