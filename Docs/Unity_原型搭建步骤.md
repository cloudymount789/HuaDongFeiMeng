## Unity 原型搭建步骤（按此做即可跑起来）

本步骤用于把仓库中的脚本快速放进 Unity 6.x 工程，并搭出 2 个可运行场景：

- `Prototype_Building`：框架/槽位/吸附放置（灰盒）
- `Prototype_DouGongRoof`：斗拱小关卡 + 屋顶建造的垂直切（灰盒）

> 说明：目前脚本以“能跑通流程”为目标，视觉与规则都先简化，后续再逐步加严谨性与美术替换。

---

### 0. 你需要准备什么

- Unity Hub + Unity 6.3 LTS（或你已安装的 Unity 6 LTS）
- 本仓库代码（你现在已经有）
- 2D 占位图（可用纯色方块/矩形 Sprite）

---

### 1. 创建 Unity 工程并导入脚本

1. 在仓库根目录创建 `UnityProject/`（Unity Hub 新建项目时选择此目录）。
2. 选择 **2D** 模板创建项目。
3. 将本仓库 `UnityProject/Assets/Scripts/` 下的脚本保持原样（已经按推荐目录放好）。

> 如果 Unity 创建项目时默认使用“新输入系统”，但你想继续用旧输入也行：\n+> `Edit -> Project Settings -> Player -> Active Input Handling` 选择 `Both`。

---

### 2. 场景一：Prototype_Building（网格 + 槽位 + 拖拽放置）

#### 2.1 创建场景与基础物体

1. 新建场景 `Assets/Scenes/Prototype_Building.unity`
2. Hierarchy 创建空物体：`Systems`\n+3. `Systems` 下创建：\n+   - `GridSystem`（挂 `GridSystem` 脚本）\n+   - `SnapManager`（挂 `SnapManager` 脚本）\n+   - `InputController`（挂 `InputController` 脚本）\n+4. Main Camera 挂 `CameraController` 脚本（可选）。

#### 2.2 准备一个可放置部件 Prefab

1. 新建一个 Sprite（比如一个 1x1 方块），拖到场景成为 GameObject\n+2. 给它添加：\n+   - `BuildingPart` 组件（设 `partType=Frame`，`partId` 随便填）\n+   - `Collider2D`（例如 `BoxCollider2D`）\n+3. 将其拖入 `Assets/Prefabs/Parts/` 成为 Prefab（比如 `Part_Frame_1.prefab`）\n+\n+> 注意：**可放置部件自己不需要 SnapSlot**，它只是被吸附到槽位上。

#### 2.3 准备若干槽位 SnapSlot

1. 在场景里创建多个空物体，摆成一排/网格\n+2. 每个槽位物体添加：\n+   - `SnapSlot` 组件\n+   - `Collider2D`（例如 `CircleCollider2D`，勾选 `IsTrigger`）\n+3. 槽位物体可以用一个小 Sprite 做可视化（可选）。

#### 2.4 连接引用并运行

1. 选中 `InputController`，把：\n+   - `Main Camera` 拖入字段\n+   - `SnapManager` 拖入字段\n+2. 选中 `InputController` 的 `Current Part Prefab` 设置为 `Part_Frame_1.prefab`\n+3. 点击 Play：\n+   - 鼠标移动时预览跟随\n+   - 按住鼠标左键拖动，松开时自动吸附到最近空槽位\n+\n+如果放置失败会销毁这次生成的对象。

---

### 3. 场景二：Prototype_DouGongRoof（斗拱 + 屋顶）

#### 3.1 创建场景与控制器

1. 新建场景 `Assets/Scenes/Prototype_DouGongRoof.unity`\n+2. 创建空物体 `DouGongSystems`，挂 `DouGongLevelController`\n+3. 创建空物体 `RoofSystems`，挂 `RoofLevelController`\n+\n+> 这两个控制器负责：创建测试槽位、接受放置输入、判断完成并输出“组件数据快照”。

#### 3.2 准备 4 个斗拱原子件 Prefab

为每个原子件做一个方块 Sprite Prefab：\n+\n+- `Part_DouGong_LuDou.prefab`\n+- `Part_DouGong_Gong.prefab`\n+- `Part_DouGong_Ang.prefab`\n+- `Part_DouGong_SanDou.prefab`\n+\n+每个都挂：\n+\n+- `BuildingPart`（`partType=DouGong`，`partId` 对应名字）\n+- `Collider2D`\n+\n+然后在 `DouGongLevelController` 里把这 4 个 Prefab 拖进数组字段。

#### 3.3 屋顶部件 Prefab（梁架/椽条/瓦片/脊饰）

先用占位 Sprite Prefab：\n+\n+- `Part_Roof_Beam.prefab`\n+- `Part_Roof_Rafter.prefab`\n+- `Part_Roof_Tile.prefab`\n+- `Part_Roof_RidgeDecor.prefab`\n+\n+每个挂 `BuildingPart`（`partType=Roof`）+ `Collider2D`。\n+\n+在 `RoofLevelController` 中把 Prefab 引用拖入对应字段。

#### 3.4 运行与验证

- Play 后：\n+  - 斗拱侧：你能在固定槽位上放置原子件，满足规则后点击“完成”按钮（UI 后续补）或按快捷键输出结果。\n+  - 屋顶侧：你能放梁架点、刷椽条、刷瓦片，并生成一个屋顶数据快照。\n+\n+> 目前 UI 以快捷键为主，后续会把工具栏/按钮 UI 补齐。

