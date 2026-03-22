using UnityEngine;

/// <summary>
/// 最小工具模式控制器：
/// - 先用 Inspector 手动选择 currentTool
/// - 后续再接 UI 工具栏按钮切换
/// </summary>
public class ToolModeController : MonoBehaviour
{
    public ToolMode currentTool = ToolMode.PlaceBeam;

    // UI Buttons can call these directly
    public void SetPlaceBeam() => currentTool = ToolMode.PlaceBeam;
    public void SetPaintRafter() => currentTool = ToolMode.PaintRafter;
    public void SetPaintTile() => currentTool = ToolMode.PaintTile;
}

