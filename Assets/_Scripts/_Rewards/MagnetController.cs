using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class MagnetController : MonoBehaviour, MMEventListener<UseMagnetTool>
{
    public StorageBox targetBox;
    private Animator animator;
    private FindScrewForMagnet screwFinder;
    
    private void OnEnable()
    {
        this.MMEventStartListening<UseMagnetTool>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<UseMagnetTool>();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        screwFinder = FindAnyObjectByType<FindScrewForMagnet>();
        
        if (animator == null)
            Debug.LogError("[MagnetController] Animator is NULL");

        if (screwFinder == null)
            Debug.LogError("[MagnetController] FindScrewForMagnet is NULL");
    }


    public void OnMMEvent(UseMagnetTool e)
    {
        animator.Play("Attracting");
        if (targetBox == null || !targetBox.IsActive)
            return;
        ScrewColor targetColor = targetBox.acceptedColor;
        int num = targetBox.GetSlotCount();
        List<ScrewController> screws = screwFinder.FindAllModelScrews(targetColor, num);
        
        if (screws.Count == 0)
            return;
        foreach (var screw in screws)
        {
            screw.ForceRelease();
        }
    }
    public void HideMagnet()
    {
        gameObject.SetActive(false);
    }
    public void PlayMagnetSfx()
    {
        AudioManager.Instance.PlaySFX(SoundId.Magnet);
    }
}