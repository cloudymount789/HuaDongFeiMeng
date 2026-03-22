## Unity 城镇模式搭建步骤（摆放 + 保存/加载）

目标：在一个简单城镇场景里，实现：

- 点击放置建筑到 `TownGrid`
- `F5` 保存布局
- `F9` 读取布局

---

### 1. 创建场景

1. 新建场景：`Assets/Scenes/Prototype_Town.unity`
2. 创建空物体 `TownSystems`，挂：\n+   - `TownGrid`\n+   - `TownPlacementController`
3. 设置 `TownGrid`：\n+   - `cellSize`（建议 1）\n+   - `size`（例如 30x30）

---

### 2. 准备一个“建筑”Prefab（占位即可）

1. 创建一个 Sprite 方块（或任意占位图），拖到场景\n+2. 加 `Collider2D`（可选）\n+3. 拖到 `Assets/Prefabs/Buildings/Building_Test.prefab`\n+4. 在 `TownPlacementController` 中：\n+   - 把 `Current Building Prefab` 指向该 prefab\n+   - `currentBuildingId` 先随便填，比如 `building_test_1`

---

### 3. 运行与验证

- Play 后左键点击：在网格上放置建筑。\n+- 按 `F5` 保存。\n+- 停止 Play 再运行，按 `F9` 加载，你会看到布局被还原。\n+\n+存档位置：`Application.persistentDataPath/HuadongFeimeng/town_save.json`

