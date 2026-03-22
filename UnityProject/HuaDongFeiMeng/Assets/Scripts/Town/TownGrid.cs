using UnityEngine;

public class TownGrid : MonoBehaviour
{
    public float cellSize = 1f;
    public Vector2Int size = new Vector2Int(30, 30);

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0f);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        var x = Mathf.RoundToInt(worldPos.x / cellSize);
        var y = Mathf.RoundToInt(worldPos.y / cellSize);
        return new Vector2Int(x, y);
    }

    public bool IsInside(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < size.x && gridPos.y < size.y;
    }
}

