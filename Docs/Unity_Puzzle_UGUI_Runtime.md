# Puzzle 运行时 UGUI（无需 Prefab）

为短工期：**代码在运行时生成** Canvas / Panel / Text / Button，你只需往场景里挂组件。

---

## 1. Lore 底栏（自动）

1. 在 `LoreSystems`（或任意空物体）上保留 **`PuzzleLorePresenter`**。  
2. **不要**手动拖 `Body Text` / `Root Panel`（留空）。  
3. 保持 **`Auto Build UI`** 勾选（默认）。  
4. Play 后会自动生成：  
   - 底部 **半透明深底栏**  
   - 标题「营造小记」（可在 Inspector 改 `Panel Title`）  
   - 正文 **Text**  
   - 右上角 **「下一句」** 按钮  

- **右键**已嵌入槽位的拼图片 → **每次从第一句重新播放**（可重复观看）。  
- **下一句** 按钮 → 同一次阅读内 **往后播**；读完关底栏后，再 **右键** 可从头再看。

> 若已手动拖了 UI 引用，则 **不会**再自动生成，改用你的 Prefab。

---

## 2. 关卡完成弹窗（自动）

1. `Hierarchy` 右键 → `Create Empty`，命名 `LevelFlow`。  
2. `Add Component` → **`PuzzleLevelCompleteUI`**。  
3. 保持 **`Auto Build Ui`** 勾选。  
4. Play：当场景中 **所有** `PuzzleSlot` 均为 **`occupied`** 时，弹出一次 **完成卡片**（标题/正文/「好的」可改）。  
5. 默认会 **禁用** `PuzzleDragRuntime`，防止完成后误拖。

**注意**：槽位列表在 **`Start` 时缓存**。若运行时动态加槽，需后续改代码或调用 `RefreshSlots()`（可做成公开按钮给策划用）。

---

## 3. 脚本位置

| 脚本 | 路径 |
|------|------|
| `PuzzleUGUIUtil` | `Assets/Scripts/UI/PuzzleUGUIUtil.cs` |
| `PuzzleLevelCompleteUI` | `Assets/Scripts/UI/PuzzleLevelCompleteUI.cs` |
| `PuzzleLorePresenter` | `Assets/Scripts/Puzzle/Lore/PuzzleLorePresenter.cs` |
| `MainMenuRuntimeUI` | `Assets/Scripts/UI/MainMenuRuntimeUI.cs` |

主菜单与 Build 列表见 **[Unity_主菜单与场景列表.md](Unity_主菜单与场景列表.md)**。

---

## 4. EventSystem

首次生成 UI 时会自动创建 **`EventSystem` + `StandaloneInputModule`**。若场景里已有，不会重复创建。

---

## 5. 与音效

完成关时播放 **`HfmSfxEvent.PuzzleLevelComplete`**；Lore 仍用 **`LoreLineAdvance`**。见 [Unity_音效与Lore搭建步骤.md](Unity_音效与Lore搭建步骤.md)。
