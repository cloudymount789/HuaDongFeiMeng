using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// 新 Input System 输入路由（最小版）：
/// - 提供指针世界坐标
/// - 提供 Press 按下/抬起/按住状态
/// </summary>
public class HfmInputRouter : MonoBehaviour
{
    [Header("Refs")]
    public Camera mainCamera;

#if ENABLE_INPUT_SYSTEM
    [Header("Input Actions (New Input System)")]
    public InputActionReference point;
    public InputActionReference press;
#endif

    public Vector3 PointerWorld { get; private set; }
    public bool PressDown { get; private set; }
    public bool PressUp { get; private set; }
    public bool PressHeld { get; private set; }

    private bool _prevHeld;

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        point?.action?.Enable();
        press?.action?.Enable();
#endif
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        point?.action?.Disable();
        press?.action?.Disable();
#endif
    }

    private void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector2 screen;
        bool held;

#if ENABLE_INPUT_SYSTEM
        if (point != null && point.action != null)
            screen = point.action.ReadValue<Vector2>();
        else
            screen = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;

        if (press != null && press.action != null)
            held = press.action.IsPressed();
        else
            held = Mouse.current != null && Mouse.current.leftButton.isPressed;
#else
        screen = Input.mousePosition;
        held = Input.GetMouseButton(0);
#endif

        var w = mainCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, 0f));
        w.z = 0f;
        PointerWorld = w;

        PressHeld = held;
        PressDown = held && !_prevHeld;
        PressUp = !held && _prevHeld;
        _prevHeld = held;
    }
}

