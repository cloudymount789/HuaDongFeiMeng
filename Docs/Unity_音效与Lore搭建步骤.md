# Unity：音效接口（HfmSfx）+ 最简 Lore（右键逐句）

## 一、音效接口说明

| 文件 | 作用 |
|------|------|
| `Assets/Scripts/Audio/IHfmSfx.cs` | 全局音效 **接口**：`Play(HfmSfxEvent)`、`PlayOneShot(AudioClip)` |
| `Assets/Scripts/Audio/HfmSfxEvent.cs` | **事件枚举**（拼图 / UI / Lore 等，可继续追加） |
| `Assets/Scripts/Audio/HfmSfxPlayer.cs` | **默认实现**：挂 `AudioSource`，在 Inspector 里拖 `AudioClip` |
| `Assets/Scripts/Audio/HfmSfxHub.cs` | **可选查找器**：未手动引用时 `FindObjectOfType<HfmSfxPlayer>` |

`HfmSfxPlayer` 在 `Awake` 会 **`HfmSfxHub.SetPlayer(this)`**（单场景建议只放一个；多实例时以后一个为准）。

**已接入拼图**：`PuzzleDragRuntime` 在 **拖起 / 吸附成功 / 放错回弹** 时会调用对应事件（未拖音频则为 **静音**，不报错）。

---

## 二、在场景里添加音效（约 2 分钟）

1. `Hierarchy` 右键 → `Create Empty`，命名 `AudioSystems`。  
2. `Add Component` → **Audio Source**  
   - 取消 **Play On Awake**  
3. 再 `Add Component` → **`HfmSfxPlayer`**  
4. 在 `HfmSfxPlayer` 的 **Puzzle** 区拖入你的 `.wav / .mp3`：  
   - **Puzzle Piece Pickup**：拿起构件  
   - **Puzzle Piece Snap Success**：嵌入槽位成功  
   - **Puzzle Piece Snap Reject**：未对上槽、正在滑回部件栏  
5. （可选）在 **`PuzzleRuntime`（PuzzleDragRuntime）** 上把 **Sfx Player** 字段 **拖入** 刚建的 `HfmSfxPlayer`；不拖也会通过 **Hub** 自动找到。

---

## 三、代码里如何调用（扩展用）

```csharp
// 按事件（推荐，便于统一混音/换实现）
HfmSfxHub.Play(HfmSfxEvent.UIClick);

// 或直接拿接口
IHfmSfx sfx = HfmSfxHub.Resolve();
sfx?.Play(HfmSfxEvent.PuzzlePieceSnapSuccess);
sfx?.PlayOneShot(myClip, 0.8f);
```

以后若接 **FMOD/Wwise**，只需做一个新 `MonoBehaviour : IHfmSfx`，并在进关时 `HfmSfxHub.SetPlayer(...)` 或改 `Resolve` 逻辑即可。

---

## 四、Lore（右键 + 运行时 UGUI，推荐）

**默认无需手摆 Canvas**：`PuzzleLorePresenter` 的 **`Auto Build UI`** 开启时，会在 **Awake** 自动生成底栏 + 正文 +「下一句」按钮。  

详细步骤与关卡完成弹窗见：**[Unity_Puzzle_UGUI_Runtime.md](Unity_Puzzle_UGUI_Runtime.md)**。

### 文案与绑定（仍需要）

1. `Create -> HuadongFeimeng -> Puzzle -> LoreLines`，填 **Lines**。  
2. 拼图片上 **`PuzzlePieceLore`**，拖入 **Data**。  
3. 场景里 **`PuzzleLorePresenter`** + **`PuzzleLoreClick`**（Presenter / Camera 引用拖好）。  

### 游玩方式

- **左键**：拖拽拼图  
- **右键** 点在 **已嵌入** 的拼图片上：**从头播放**该构件 Lore（可重复观看；点在 UI 上不会误触世界）  
- **下一句** 按钮：同一次阅读内 **接着播**，不重置  

---

## 五、下一步开发建议

- 关卡完成时调用：`HfmSfxHub.Play(HfmSfxEvent.PuzzleLevelComplete)`（可在 `PuzzleLevelComplete` 脚本里接）  
- Lore 改左键双态或专用「讲解」按钮，避免与拖拽抢键  
- `PuzzleLorePresenter` 的 **下一句** 可绑到 UGUI **Button.onClick**
