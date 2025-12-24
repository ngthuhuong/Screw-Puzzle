using System;
using UnityEngine;
using TMPro;

public class RewardItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    
    public void Setup(int amount)
    {
        amountText.text = $"{amount}";
    }
}