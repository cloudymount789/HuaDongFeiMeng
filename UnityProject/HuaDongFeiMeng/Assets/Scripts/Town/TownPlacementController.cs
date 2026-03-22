using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 城镇摆放（灰盒版）：
/// - 从一个 prefab 库中选择建筑（先用 currentBuildingPrefab 代替 UI）
/// - 拖拽/点击放到 TownGrid 上
/// - 保存/加载城镇布局到 persistentDataPath
/// </summary>
public class TownPlacementController : MonoBehaviour
{
    [Header("Refs")]
    public TownGrid townGrid;
    public Camera mainCamera;

    [Header("Placement")]
    public GameObject currentBuildingPrefab;
    public string currentBuildingId = "building_test_1";

    [Header("Runtime")]
    public List<GameObject> placedBuildings = new List<GameObject>();

    [Header("Save")]
    public string saveFileName = "town_save.json";

    [Header("Debug Keys")]
    public KeyCode saveKey = KeyCode.F5;
    public KeyCode loadKey = KeyCode.F9;

    private void Update()
    {
        if (townGrid == null) townGrid = FindObjectOfType<TownGrid>();
        if (mainCamera == null) mainCamera = Camera.main;
        if (townGrid == null || mainCamera == null) return;

        if (PuzzleCoexistence.SuppressesLegacyInput()) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceAtMouse();
        }

        if (Input.GetKeyDown(saveKey)) Save();
        if (Input.GetKeyDown(loadKey)) Load();
    }

    private void TryPlaceAtMouse()
    {
        if (currentBuildingPrefab == null) return;

        var world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;

        var gridPos = townGrid.WorldToGrid(world);
        if (!townGrid.IsInside(gridPos)) return;

        var placeWorld = townGrid.GridToWorld(gridPos);
        var go = Instantiate(currentBuildingPrefab, placeWorld, Quaternion.identity);
        go.name = $"Placed_{currentBuildingId}_{gridPos.x}_{gridPos.y}";
        placedBuildings.Add(go);
    }

    public void Save()
    {
        var data = new TownSaveData();

        foreach (var go in placedBuildings)
        {
            if (go == null) continue;
            var gridPos = townGrid.WorldToGrid(go.transform.position);

            data.buildings.Add(new TownSaveData.PlacedBuilding
            {
                buildingId = currentBuildingId,
                gridPos = gridPos,
                rotation = 0
            });
        }

        var json = SaveSystem.ToJsonPretty(data);
        SaveSystem.SaveJson(saveFileName, json);
        Debug.Log($"Town saved: {saveFileName}");
    }

    public void Load()
    {
        var json = SaveSystem.LoadJson(saveFileName);
        var data = SaveSystem.FromJson<TownSaveData>(json);
        if (data == null)
        {
            Debug.LogWarning("Town load: no save found.");
            return;
        }

        ClearPlaced();

        foreach (var b in data.buildings)
        {
            if (!townGrid.IsInside(b.gridPos)) continue;
            if (currentBuildingPrefab == null) continue;
            var pos = townGrid.GridToWorld(b.gridPos);
            var go = Instantiate(currentBuildingPrefab, pos, Quaternion.identity);
            go.name = $"Placed_{b.buildingId}_{b.gridPos.x}_{b.gridPos.y}";
            placedBuildings.Add(go);
        }

        Debug.Log($"Town loaded: buildings={placedBuildings.Count}");
    }

    private void ClearPlaced()
    {
        foreach (var go in placedBuildings)
        {
            if (go != null) Destroy(go);
        }
        placedBuildings.Clear();
    }
}

