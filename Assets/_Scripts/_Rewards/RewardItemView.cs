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
    public void SetupInToolPannel(RewardType rewardType, int amount)
    {
        type = rewardType;
        amountText.text = amount.ToString();
        button.interactable = amount > 0;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);    
    }
    private void OnClick()
    {
        RewardManager.Instance.UseTool(type);
    }
    
}