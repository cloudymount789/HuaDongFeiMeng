# 主菜单场景 + Build Settings

## 1. 脚本

`Assets/Scripts/UI/MainMenuRuntimeUI.cs`  

- 运行时生成：**标题、副标题、「开始搭建」按钮**  
- **`Game Scene Name`**：要进入的关卡场景名（与 `File -> Build Settings` 里 **完全一致**，不要带 `.unity`）

---

## 2. 新建主菜单场景（约 1 分钟）

1. `File -> New Scene` → 保存为 `Assets/Scenes/MainMenu.unity`（名称自定）。  
2. 创建空物体 `MenuRoot`，`Add Component` → **`MainMenuRuntimeUI`**。  
3. Inspector 中把 **Game Scene Name** 改成你的拼图场景，例如：`Puzzle_VerticalSlice`。  
4. `File -> Build Settings`：  
   - **先拖入 `MainMenu`**（建议 index 0）  
   - **再拖入 `Puzzle_VerticalSlice`**（或你的关卡场景）  
5. Play：点 **开始搭建** 应切换场景。

若报错 **Scene not found**：检查场景名拼写、是否已加入 Build 列表。

---

## 3. Lore 可重复观看

- **右键**已嵌入槽位的构件：**每次都会从第一句重新播放**（内部 `ResetProgress`）。  
- **下一句**按钮：在同一次阅读中 **接着往后**，不会重置；读完关闭后，再 **右键** 可重新从头看。
