using System;
using UnityEngine;

public class DrillController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void InitializeDrill(float offsetX )
    {
        AudioManager.Instance.PlaySFX(SoundId.Drill);
        Vector3 oldpos = transform.position;
        transform.position = oldpos +Vector3.right*offsetX;
        gameObject.SetActive(true);
        if(animator != null) animator.Play("DrillAnim");
    }
}
