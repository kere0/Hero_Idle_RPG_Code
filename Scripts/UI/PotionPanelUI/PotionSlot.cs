using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour
{
    [SerializeField] private Button _button;
    private int _slotNum;
    [SerializeField] private TextMeshProUGUI _useCheckText;
    private void Awake()
    {
        _button.onClick.AddListener(ADButtonClick);
    }

    public void Init(int slotNum)
    {
        _slotNum = slotNum;
        bool isUsed = Managers.PlayerManager.playerData.HasUsedAd[_slotNum];
        _button.interactable = !isUsed;
        _useCheckText.text = $"일일 사용 가능 {(isUsed ? 0 : 1)} / 1";
    }
    private void ADButtonClick()
    {
        Managers.PlayerManager.playerData.HasUsedAd[_slotNum] = true;
        bool isUsed = Managers.PlayerManager.playerData.HasUsedAd[_slotNum];
        _button.interactable = !isUsed;
        _useCheckText.text = $"일일 사용 가능 {(isUsed ? 0 : 1)} / 1";
        AdsManager.Instance.ShowRewardAd(() =>
        {
            Managers.PlayerManager.AdBuffSystem.ApplyAdBuff((PotionType)_slotNum, PotionPanelUI.PotionBuffTime, PotionPanelUI.PotionBuffRate);
        });
    }
}
