using UnityEngine;

public struct ReleaseScrew
{
    public ScrewController screwController;

    public ReleaseScrew(ScrewController s)
    {
        screwController = s;
    }
}
public struct BoxFull
{
    public StorageBox box;
    public BoxFull(StorageBox b) { box = b; }
}

public struct LoseGame
{
    
}
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

