using System.Collections.Generic;
using UnityEngine;

public struct ReleaseScrew
{
    public ScrewController screwController;

    public ReleaseScrew(ScrewController s)
    {
        screwController = s;
    }
}
public struct CubeCleared
{
    public CubeController cube;
    public CubeCleared(CubeController c) => cube = c;
}

public struct BoxFull
{
    public StorageBox box;
    public BoxFull(StorageBox b) { box = b; }
}
public struct StartGame{} //for reset game 
public struct LoseGame
{
}
public struct WinGameEvent{ }
public struct Confirm
{
    public string tag;
    public bool isYes;
    public Confirm(string m,bool yes)
    {
        tag = m;
        isYes = yes; 
    }
}
public struct DataChange
{
}

public struct LevelSolutionReadyEvent
{
    public List<ScrewColor> solutionColors;

    public LevelSolutionReadyEvent(List<ScrewColor> colors)
    {
        solutionColors = colors;
    }
}


public struct UseDrillTool{}

public struct UseHammerTool{}
public struct UseBroomTool{}
public struct UseMagnetTool{}