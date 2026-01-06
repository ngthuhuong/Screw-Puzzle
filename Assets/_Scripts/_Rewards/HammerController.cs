using UnityEngine;

public class HammerController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void InitializeHammer(Vector3 position )
    {
        AudioManager.Instance.PlaySFX(SoundId.Hammer);
        Vector3 offset = new Vector3(0f, 5f, -10f);
        transform.position = position + offset;
        gameObject.SetActive(true);
        if(animator != null) animator.Play("Hammer_pop");
    }

    public void DisableHammer()
    {
        gameObject.SetActive(false);
    }
}
