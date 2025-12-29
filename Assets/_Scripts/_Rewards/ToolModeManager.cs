using MoreMountains.Tools;
using UnityEngine;

public enum ToolMode
{
    None=0,
    Hammer=1,
    Magnet=2
}

public class ToolModeManager : MMSingleton<ToolModeManager>,MMEventListener<UseHammerTool>
{
    private ToolMode mode;
    public ToolMode CurrentMode { get; private set; } = ToolMode.None;

    protected override void Awake()
    {
        base.Awake();
        CurrentMode = ToolMode.None;
    }

    private void OnEnable()
    {
        this.MMEventStartListening<UseHammerTool>();
    }
    private void OnDisable()
    {
        this.MMEventStopListening<UseHammerTool>();
    }
    public void ExitToolMode()
    {
        CurrentMode = ToolMode.None;
    }

    public bool IsHammerMode()
    {
        return CurrentMode == ToolMode.Hammer;
    }

    #region Event Listeners
    public void OnMMEvent(UseHammerTool eventType)
    {
        CurrentMode = ToolMode.Hammer;
    }
    #endregion
}
