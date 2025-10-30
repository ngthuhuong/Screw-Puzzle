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

