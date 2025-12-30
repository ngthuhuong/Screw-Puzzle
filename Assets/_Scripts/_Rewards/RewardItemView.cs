using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class RewardItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    private RewardType type;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SetupAsReward(RewardType rewardType, int amount)
    {
        type = rewardType;
        if (amountText == null)
        {
            Debug.LogError("amountText chưa được gán", this);
            return;
        }
        if (button == null)
        {
            Debug.LogError("button chưa được gán", this);
            return;
        }
        amountText.text = amount.ToString();
        button.onClick.RemoveAllListeners();
         
    }
    public void SetupInToolPannel(RewardType rewardType, int amount)
    {
        type = rewardType;

        if (amountText == null)
        {
            Debug.LogError("amountText chưa được gán", this);
            return;
        }

        if (button == null)
        {
            Debug.LogError("button chưa được gán", this);
            return;
        }

        amountText.text = amount.ToString();

        if (amount != 0)
        {
            button.interactable = amount > 0;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }
    
    private void OnClick()
    {
        RewardManager.Instance.UseTool(type);
    }
    
}