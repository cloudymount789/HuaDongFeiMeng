# Unity 槽位拼图场景搭建步骤（P1 垂直切片）

本步骤对应工程脚本：`UnityProject/HuaDongFeiMeng/Assets/Scripts/Puzzle/`  

实现效果：

- 从屏幕下方（或任意你摆的「部件栏」位置）**拖起拼图片**；
- **靠近**同 `pieceId` 的槽位并松手 → 拼图片用 **SmoothStep 缓动滑入**槽位（不是瞬移）；
- **放错**或 **够不着槽** → 拼图片 **缓慢滑回**拖起前的原位（`HomeWorldPosition`）；
- **不旋转**（仅平移）。

---

## 0. 前置条件

1. 已打开 Unity 工程：`UnityProject/HuaDongFeiMeng`（或你本地的同名工程）。
2. 已按 [Unity_新输入系统与工具模式（下一步）.md](Unity_新输入系统与工具模式（下一步）.md) 配好 **Input Actions**（至少 `Gameplay/Point` 与 `Gameplay/Press`）。
3. 场景里已有 **Main Camera**（正交 Orthographic 推荐）。

---

## 1. 新建测试场景

1. 菜单 `File -> Save As...`  
2. 保存为：`Assets/Scenes/Puzzle_VerticalSlice.unity`（路径可自定）。

---

## 2. 放置输入路由器（与旧文档一致）

1. `Hierarchy` 右键 → `Create Empty`，命名 `InputSystems`。  
2. `Add Component` → 搜索 `HfmInputRouter` → 添加。  
3. Inspector 中：  
   - **Main Camera**：把 `Main Camera` 拖进去。  
   - **Point**：拖入 `HfmInputActions` 里的 **`Gameplay/Point`**。  
   - **Press**：拖入 **`Gameplay/Press`**。  

---

## 3. 创建「槽位」PuzzleSlot（灰模）

对每个槽位重复以下步骤（先做 **1 个** 即可跑通）：

1. `Hierarchy` 右键 → `2D Object -> Sprites -> Square`（或你自己的占位图）。  
2. 命名例如 `Slot_A`。  
3. `Add Component` → `PuzzleSlot`。  
4. **Collider2D**：  
   - 若自动加了 `BoxCollider2D`，勾选 **`Is Trigger`**（`PuzzleSlot` 的 `Reset` 会设，但手动检查一次）。  
5. **Piece Id**：填一个字符串，例如 `part_a`（与拼图片一致）。  
6. **灰模外观**（与方案一致：同图降透明度）：  
   - 选中物体上的 `Sprite Renderer`  
   - `Color` 的 **A（Alpha）** 调低，例如 `0.35`  
   - `Color` 的 RGB 可略调灰（例如 `(0.7, 0.7, 0.7)`）  
7. 把物体移到画面**偏上**的位置（建筑区）。

> **Sorting**：若需要被后放的部件挡住，后面再给槽位/拼图片调 `Sorting Layer` 与 `Order in Layer`。

---

## 4. 创建「拼图片」PuzzlePiece（有色）

1. `Hierarchy` 右键 → `2D Object -> Sprites -> Square`。  
2. 命名 `Piece_A`。  
3. `Add Component` → `PuzzlePiece`。  
4. **Collider2D**：  
   - `BoxCollider2D` **不要**勾选 `Is Trigger`（方便 `OverlapPoint` 点到）。  
5. **Piece Id**：填 **`part_a`**（必须与槽位一致）。  
6. **Sprite Renderer**：  
   - 颜色可与槽位区分（例如浅木色），**Alpha = 1**。  
7. **摆放位置（重要）**：  
   - 把 `Piece_A` 放在画面**下方**当作「部件栏」位置。  
   - 运行后第一次拖起前，`PuzzlePiece` 会在 **`Awake`** 里把当前坐标记为 **`HomeWorldPosition`**（放错会回到这里）。  
   - 若你在代码里动态生成拼图片，请手动调用 `SetHomeWorldPosition(...)`。

8. **可选**：`Drag Offset` 一般保持 `(0,0,0)`；若手感和中心点对不齐可微调。

---

## 5. 挂载拖拽运行时 PuzzleDragRuntime

1. `Hierarchy` 右键 → `Create Empty`，命名 `PuzzleRuntime`。  
2. `Add Component` → `PuzzleDragRuntime`。  
3. Inspector：  
   - **Input**：把 `InputSystems` 拖进来（或留空会在运行时 `FindObjectOfType`）。  
   - **Main Camera**：拖 `Main Camera`。  
   - **Snap Radius**：全局默认吸附距离，例如 `0.65`（槽位上 `snapRadius` 可覆盖）。  
   - **Slide Into Slot Duration**：滑入槽位秒数，例如 **`0.45`**（想更慢就调大）。  
   - **Return Home Duration**：回部件栏秒数，例如 **`0.35`**。

---

## 6. Play 测试

1. 点击 **Play**。  
2. 在 **Game** 窗口内：  
   - **按住左键**在拼图片上 → 拖动；  
   - 移到槽位附近 **松开** → 应看到 **缓慢滑入**；  
   - 拖到远处松开 → 应 **缓慢回到**下方原位。  

### 若拖不动 / 没反应

- 确认 **拼图片** 有 **非 Trigger** 的 `Collider2D`，且能盖住鼠标下的点。  
- 确认 `HfmInputRouter` 的 Point/Press 已绑定，`Main Camera` 为 **Orthographic**，且 `Game` 窗口被点击过。  
- 打开 `Window -> Analysis -> Physics 2D` 看是否禁用了模拟（一般不会）。

### 若松手后多出一个「复制件」

- 常见原因：场景里仍挂着旧版 **`InputController`**，它会在 **PressUp** 时 `Instantiate` 预制体，与 **`PuzzleDragRuntime`** 冲突。  
- **处理**：保证场景里有 **已启用** 的 `PuzzleDragRuntime` 时，`InputController`、**`RoofLevelController`**、**`TownPlacementController`** 会 **自动不再** 用同一次输入去 **生成梁/建筑/吸附件**（见 `PuzzleCoexistence`）。若仍异常，可 **取消勾选** 上述 Legacy 组件，或把 `InputController` 的 **`Respect Puzzle Mode`** 保持勾选。

### 从槽位里再拿出来

- **已嵌入**的拼图片可再次 **在构件上按下并拖走**：会 **自动释放槽位**（`occupied = false`），再松手可 **滑入另一槽** 或 **滑回部件栏**。  
- 若点不中构件：略 **放大** 拼图片的 `Collider2D`，或提高 `SpriteRenderer` 的 **Order in Layer**，避免被槽位挡住。

### 若一放就瞬移

- 检查 `PuzzleDragRuntime` 的 **Slide Into Slot Duration** 是否被改成 `0`。

---

## 7. 图层遮挡（简单做法）

1. 菜单 `Edit -> Project Settings -> Tags and Layers` → **Sorting Layers** 增加例如：`Background`、`Slots`、`Pieces`、`FX`。  
2. 背景图用 `Background`；槽位用 `Slots`；拼图片默认 `Pieces`。  
3. 后放的构件：在槽位 `PuzzleSlot` 上填 **Placed Piece Sorting Order**（例如更大），或在拼图片 `Sprite Renderer` 上直接调 **Order in Layer**。

---

## 8. 与《游戏设计更新》粒度对齐时

每增加一个真实构件：

1. 复制一份 `Slot_xxx` + `Piece_xxx`，改 **`pieceId` 唯一字符串**。  
2. 换 **同一套 Sprite**（槽位灰模、拼图片上色）。  
3. 按搭建顺序调整 **Sorting** 与 **位置**。  

后续我们会加 **烘焙工具**（有色摆法 → 自动生成灰槽关卡），当前先用场景手工摆即可验证手感。

---

## 9. 与视频的关系

视频与 `.mp4` **仅作参考**；关卡内容以 **[游戏设计更新.md](../游戏设计更新.md)** 为准。本文只保证 **交互与动画** 符合当前方案。

## 10. 音效、Lore UGUI、关卡完成

- 音效与 Lore 文案：[Unity_音效与Lore搭建步骤.md](Unity_音效与Lore搭建步骤.md)  
- **运行时 UGUI**（Lore 底栏 + 完成弹窗）：[Unity_Puzzle_UGUI_Runtime.md](Unity_Puzzle_UGUI_Runtime.md)
