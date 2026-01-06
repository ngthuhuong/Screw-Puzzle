using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BroomController : MonoBehaviour,MMEventListener<UseBroomTool>
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<UseBroomTool>();
    }
    private void OnDisable()
    {
        this.MMEventStopListening<UseBroomTool>();
    }
    

    public void OnMMEvent(UseBroomTool eventType)
    {
        animator.Play("BroomSweep");
    }
    public void PlayBroomSfx()
    {
        AudioManager.Instance.PlaySFX(SoundId.Broom);
    }
}
