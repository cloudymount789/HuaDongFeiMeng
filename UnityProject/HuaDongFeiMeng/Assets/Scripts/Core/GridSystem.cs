using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Cell")]
    public float cellSize = 1f;

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        var x = Mathf.RoundToInt(worldPos.x / cellSize);
        var y = Mathf.RoundToInt(worldPos.y / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0f);
    }
}

