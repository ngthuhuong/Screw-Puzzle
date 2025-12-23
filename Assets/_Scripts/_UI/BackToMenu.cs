using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackToMenuButton : MonoBehaviour
{
    private Button button;
    private PopupController popupController;

    private void Awake()
    {
        button = GetComponent<Button>();
        popupController = GetComponentInParent<PopupController>();

        if (popupController == null)
        {
            Debug.LogError(
                "[BackToMenuButton] Không tìm thấy PopupController trong parent hierarchy!"
            );
            enabled = false;
            return;
        }

        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        popupController.EnableConfirmGroup(
            "Are you sure you want to return to the main menu?",
            "BackToMenu"
        );
    }
}