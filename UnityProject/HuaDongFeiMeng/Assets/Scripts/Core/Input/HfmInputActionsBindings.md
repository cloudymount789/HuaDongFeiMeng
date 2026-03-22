## HFM Input Actions 绑定约定（给新 Input System 用）

本项目使用 Unity **Input System** 的 `InputActionAsset` 来驱动鼠标/触摸输入。

建议你创建一个 `Input Actions` 资源：`Assets/Configs/Input/HfmInputActions.inputactions`，并包含一个 Action Map：`Gameplay`，其中至少包含这些 Actions：

### 必须的 Actions

- **Point**（Value / Vector2）
  - Bindings：
    - `<Pointer>/position`
- **Press**（Button）
  - Bindings：
    - `<Pointer>/press`

### 可选（后续扩展）

- **Scroll**（Value / Vector2）
  - `<Mouse>/scroll`

> 说明：我们在代码里只依赖 `Point` 和 `Press` 两个动作就能实现拖拽放置与刷子铺设。

