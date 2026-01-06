using TMPro;
using UnityEngine;

public class AlertUIController : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private CanvasGroup canvasGroup;

    private float timeLeft;
    private float blinkTimer;

    private bool isShowing;
    private float blinkInterval = 0.25f;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isShowing) return;

        timeLeft -= Time.deltaTime;
        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0f)
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0f : 1f;
            blinkTimer = blinkInterval;
        }

        if (timeLeft <= 0f)
        {
            Hide();
        }
    }

    public void Show(string message, float duration = 2f)
    {
        if(textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = message;

        timeLeft = duration;
        blinkTimer = blinkInterval;
        isShowing = true;

        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        isShowing = false;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}