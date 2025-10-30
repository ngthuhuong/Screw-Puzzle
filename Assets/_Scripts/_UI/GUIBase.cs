using UnityEngine;

public class GUIBase : MonoBehaviour
{
    [SerializeField] protected bool lockInputWhenOpen = true;

    protected bool isOpen = false;

    public virtual void Show()
    {
        gameObject.SetActive(true);
        isOpen = true;

        if (lockInputWhenOpen)
        {
            GameManager.Instance.LockInput();
        }
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        isOpen = false;

        if (lockInputWhenOpen)
        {
            GameManager.Instance.UnlockInput();
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}