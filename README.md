## 《画栋飞甍》项目说明

《画栋飞甍》是一款 **2D 斜二测、槽位吸附式异形拼图** 向游戏（兼 **解压 / 科普 / 治愈**）：玩家拖动 **真实营造构件**（如栌斗、散斗、檩椽等）到 **形状匹配的灰槽**，按 **先后顺序与图层遮挡** 完成整栋建筑的拼装；点击构件可 **逐句阅读** 短科普（类 Galgame）。

> **当前主方案**：[游戏开发方案_槽位拼图版.md](游戏开发方案_槽位拼图版.md) · **方向依据**：[游戏设计更新.md](游戏设计更新.md)  
> **旧版**「大网格 + 刷子屋顶 + 城镇主轴」已弃用说明：[Docs/规划说明_旧版网格方案已弃用.md](Docs/规划说明_旧版网格方案已弃用.md)

### 核心玩法概述（新版）

- **槽位拼图**：异形灰槽 + 同形拼图片，靠近即吸附嵌入（非「整图切碎」的传统拼图）。
- **先后与图层**：放置顺序 + Sorting 遮挡，贴近真实搭建层次。
- **科普**：点击构件 → **一句一句点下去** 的短文本。
- **UI**：治愈、可爱向包装。
- **Demo 内容锚点**：「四架椽屋前后剳牵用四柱」部件链（详见游戏设计更新.md）。

### 模块结构简表（新版 · 与槽位拼图方案一致）

- **A. 槽位拼图引擎**：Piece/Slot 数据、吸附校验、图层与顺序、关卡运行时
- **B. 输入与相机**：New Input System、斜二测下平移缩放（无玩法网格）
- **C. 科普子系统**：逐句 Lore UI、与 pieceId 绑定
- **D. 游戏 UI**：主菜单、HUD、完成页、治愈可爱视觉规范
- **E. 关卡构建辅助**：摆槽位 + 设图层 + 导出 LevelData（策划少写代码）
- **F. Demo 内容**：四架椽屋部件链关卡切片
- **G. 存档与进度**（可选）：通关状态、解锁
- **H. 发布**：PC Demo 打包、设置与音量

（旧版 A–F「网格建造/城镇主轴」模块表已迁入弃用说明文档，仅作历史参考。）

### Unity 工程与目录约定

> 注：Unity 实际工程建议建立在本仓库的子目录下，例如 `UnityProject/`，以便与设计文档分开管理。

推荐目录结构（Unity 工程内部）：

- `Assets/Art/`：像素美术资源（建筑部件、屋顶、斗拱、UI 等）
- `Assets/Audio/`：音乐与音效
- `Assets/Scripts/`：
  - `Audio/`：**IHfmSfx** 音效接口与 `HfmSfxPlayer`（拼图事件已接）
  - `Puzzle/`：**槽位拼图引擎**（Piece/Slot、吸附、关卡运行时）— **新版主代码建议放这里**
  - `Core/`：输入、相机、存档等
  - `Building/`：**Legacy** 网格/刷子原型（可选保留复用）
  - `Gameplay/`：模式、工具切换等
  - `UI/`：界面与交互（Lore/通关/**MainMenuRuntimeUI** 等）
- `Assets/Prefabs/`：部件、建筑、UI 预制体
- `Assets/Scenes/`：各类场景（Boot、MainMenu、Level_xxx、Sandbox、Town 等）
- `Assets/Configs/`：ScriptableObject 配置（关卡、部件库、知识卡片等）

### 开发环境

- **引擎**：Unity **6000.3.10f1**（见 `UnityProject/HuaDongFeiMeng/ProjectSettings/ProjectVersion.txt`）。建议安装 **相同或更高修订的 6000.3.x**，避免工程升级提示。
- **模块**：2D / URP（工程已带 `manifest.json` 与 URP 资源，首次打开会自动拉包）。
- **目标平台**：PC 首发（Windows），后续可扩展到其他平台。

### Clone 后别人能直接跑吗？

**可以。** 仓库里包含 `Assets/`、`Packages/`（`manifest.json` / `packages-lock.json`）、`ProjectSettings/`，**不包含** `Library/`（本机缓存，体积大）。别人克隆后：

1. 安装 **Unity Hub** 与 **Unity 6000.3.x**（与上面版本一致最佳）。
2. **Add** 或 **Open** 文件夹：`UnityProject/HuaDongFeiMeng`（不要只打开仓库根目录，除非你自己改结构）。
3. 首次打开会 **导入资源并生成 `Library/`**，可能要几分钟，属正常现象。
4. 在 Unity 中打开场景，例如 **`Assets/Scenes/MainMenu.unity`** 或 **`Assets/Scenes/Puzzle_VerticalSlice.unity`**，再点 **Play**。
5. **视频**（`*.mp4` 等）已写入 `.gitignore`，不会进仓库；若文档里提到的参考视频在你本机，可放在仓库外或自行拷贝，不影响工程编译运行。

### 运行方式（建议）

1. 用 Unity Hub 打开 **`UnityProject/HuaDongFeiMeng`**。
2. 打开 **`Assets/Scenes/MainMenu.unity`** 或 **`Puzzle_VerticalSlice.unity`** 等场景后点 Play。
3. 旧版网格/刷子原型可在 **`Prototype_Building.unity`** 等场景中调试（Legacy）。

### 你现在该做什么（新版优先）

1. 阅读 **[游戏开发方案_槽位拼图版.md](游戏开发方案_槽位拼图版.md)**。  
2. **P1 槽位拼图切片**：按 **[Docs/Unity_槽位拼图场景搭建步骤.md](Docs/Unity_槽位拼图场景搭建步骤.md)** 搭建 `Puzzle_VerticalSlice` 场景，验证 **拖起 → 松手后缓动滑入槽位**；放错则 **缓动回部件栏**。  
3. 旧版 `Docs/Unity_原型搭建步骤.md` 等 **仅供 Legacy 参考**；验收以槽位拼图方案为准。

### 开发进度（概要 · 已按新版重排）

- [ ] P0：方案冻结（Piece/Slot 格式、图层命名、Demo 最小构件集）
- [ ] P1：垂直切片（单柱 + 单斗 + 吸附 + 完成 + Lore）
- [ ] P2：Demo「四架椽屋」骨架关卡（分段交付）
- [ ] P3：UI 包装与治愈风一轮
- [ ] P4：音效动效与发布级 Demo 打磨

