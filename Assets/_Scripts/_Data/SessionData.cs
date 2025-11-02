using MoreMountains.Tools;
using UnityEngine;

[System.Serializable]
public class SessionData :MMEventListener<ReleaseScrew>
{
    public int screwsRemoved;
    

    public void Enable()
    {
        this.MMEventStartListening<ReleaseScrew>();
    }
    public void Disable()
    {
        this.MMEventStopListening<ReleaseScrew>();
    }
    public SessionData()
    {
        Reset();
    }

    public void AddScrewRemoved()
    {
        screwsRemoved++;
    }

    public void Reset()
    {
        screwsRemoved = 0;
    }

    public void OnMMEvent(ReleaseScrew eventType)
    {
        AddScrewRemoved();
    }
}
