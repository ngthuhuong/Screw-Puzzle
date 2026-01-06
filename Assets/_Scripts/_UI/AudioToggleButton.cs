using UnityEngine;
using UnityEngine.UI;

public class AudioToggleButton : MonoBehaviour
{
    public enum AudioType
    {
        BGM,
        SFX
    }

    [Header("Type")]
    [SerializeField] private AudioType audioType;

    [Header("UI")]
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Image image;

    private Button button;
    private bool isOn = true;

    private string PrefKey => audioType == AudioType.BGM
        ? "BGM_ON"
        : "SFX_ON";

    void Start()
    {
        button = GetComponent<Button>();
        if (image == null)
            image = GetComponentInChildren<Image>();

        // Load saved state
        isOn = PlayerPrefs.GetInt(PrefKey, 1) == 1;

        ApplyState();

        button.onClick.AddListener(Toggle);
    }

    private void Toggle()
    {
        AudioManager.Instance.PlayClickButton();
        isOn = !isOn;
        ApplyState();

        PlayerPrefs.SetInt(PrefKey, isOn ? 1 : 0);
    }

    private void ApplyState()
    {
        image.sprite = isOn ? onSprite : offSprite;

        if (AudioManager.Instance == null) return;

        switch (audioType)
        {
            case AudioType.BGM:
                AudioManager.Instance.MuteBGM(!isOn);
                break;

            case AudioType.SFX:
                AudioManager.Instance.MuteSFX(!isOn);
                break;
        }
    }
}