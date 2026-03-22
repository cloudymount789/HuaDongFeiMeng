## Unity 新输入系统与工具模式（下一步开发）

目标：把当前的“按键/旧输入”原型，升级为 **New Input System + 鼠标拖拽** 的统一输入，并用一个最小“工具模式”控制屋顶放置/刷子。

你将得到的效果：

- 鼠标移动：指针世界坐标正常更新
- 鼠标按下/抬起：事件稳定（不依赖 Game 窗口键盘焦点）
- 在 Inspector 选择 `ToolMode`：\n+  - `PlaceBeam`：松开鼠标放梁\n+  - `PaintRafter`：按住鼠标刷椽\n+  - `PaintTile`：按住鼠标刷瓦

---

### 1. 开启 Input System 包

1. 打开 `Window -> Package Manager`\n+2. 搜索并安装 `Input System`（如果已安装可跳过）\n+3. 打开 `Edit -> Project Settings -> Player`\n+4. 找到 `Active Input Handling`，选择：\n+   - **Input System Package (New)** 或 **Both**（推荐 Both，方便兼容旧代码）
5. Unity 可能提示重启 Editor，按提示重启。

---

### 2. 创建 Input Actions 资源（最小两项）

1. 在 Project 面板右键：`Create -> Input Actions`\n+2. 命名：`HfmInputActions`\n+3. 双击打开编辑器，创建：\n+   - Action Map：`Gameplay`\n+   - Action：`Point`（Action Type=Value，Control Type=Vector2）\n+     - Binding：`<Pointer>/position`\n+   - Action：`Press`（Action Type=Button）\n+     - Binding：`<Pointer>/press`\n+4. 保存。

（绑定约定也写在：`UnityProject/HuaDongFeiMeng/Assets/Scripts/Core/Input/HfmInputActionsBindings.md`）

---

### 3. 场景里放一个输入路由器 HfmInputRouter

在你测试屋顶的场景（例如 `Prototype_DouGongRoof`）里：

1. Hierarchy 右键 -> Create Empty，命名 `InputSystems`\n+2. 给它 Add Component：`HfmInputRouter`\n+3. 在 Inspector 中，把字段拖好：\n+   - `Main Camera`：拖入你的 Main Camera\n+   - `Point`：拖入 `HfmInputActions` 里的 `Gameplay/Point`\n+   - `Press`：拖入 `HfmInputActions` 里的 `Gameplay/Press`\n+\n+> 拖引用的方法：先点选 `InputSystems`，在 Inspector 里看到 `Point` 右边的空框，然后把 Project 里的 `HfmInputActions` 展开到对应 Action 再拖进去。\n+
如果你不知道怎么“展开到 Action”：\n+\n+- 选中 `HfmInputActions` 资源后，Inspector 会显示 Action Map 和 Actions，可从那里拖 `Point/Press` 的引用。

---

### 4. 加一个工具模式控制器 ToolModeController

1. Hierarchy 右键 -> Create Empty，命名 `ToolMode`\n+2. Add Component：`ToolModeController`\n+3. 在 Inspector 里设置 `Current Tool`：\n+   - `PlaceBeam` / `PaintRafter` / `PaintTile`\n+\n+> 目前先用 Inspector 手动切换工具，下一步我们再加真正的 UI 工具栏按钮。

---

### 5. 让 RoofLevelController 使用新输入与工具模式

1. 选中挂了 `RoofLevelController` 的物体（如 `RoofSystems`）\n+2. 在 Inspector 把字段拖好：\n+   - `Input`：拖入 `InputSystems`（上一步的物体）上的 `HfmInputRouter`\n+   - `Tool Mode`：拖入 `ToolMode`（上一步的物体）上的 `ToolModeController`\n+3. Play 测试：\n+   - 选择 `PlaceBeam`：松开鼠标放梁\n+   - 选择 `PaintRafter`：按住刷椽\n+   - 选择 `PaintTile`：按住刷瓦\n+
---

### 6. 如果你运行时报错 “ENABLE_INPUT_SYSTEM” 相关

说明你的工程尚未启用 Input System 或 `Active Input Handling` 没切对。\n+\n+回到第 1 步检查：\n+\n+- Package Manager 是否安装了 Input System\n+- Player Settings 的 `Active Input Handling` 是否为 New 或 Both\n+
