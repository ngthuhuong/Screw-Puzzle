using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private string currentTag;

    public void Show(string message, string tag)
    {
        AudioManager.Instance.PlaySFX(SoundId.Noti);
        gameObject.SetActive(true);
        currentTag = tag;
        messageText.text = message;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickButton();
            MMEventManager.TriggerEvent(new Confirm(currentTag, true));
            Hide();
        });

        noButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickButton();
            MMEventManager.TriggerEvent(new Confirm(currentTag, false));
            Hide();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}