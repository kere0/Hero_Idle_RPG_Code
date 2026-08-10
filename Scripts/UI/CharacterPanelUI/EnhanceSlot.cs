using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceSlot : CharacterSlot
{
    [SerializeField] private EnhanceType _enhanceType;

    protected virtual void Awake()
    {
        base.Awake();
        GameManager.Instance.OnGameStart += Refresh;
    }
    private void OnEnable()
    {
        if (GameManager.Instance.isGameStarted == true)
        {
            Refresh();
        }
    }
    private void Start()
    {
        Managers.PlayerManager.OnEnhanceChanged += Refresh;
        Managers.PlayerManager.OnGoldChanged += Refresh;
    }
    protected override void UpgradeButtonClick()
    {
        PlayerManager.CharacterSystem.UpgradeEnhanceData(_enhanceType);
        Canvas_Menu.Instance.PlayUpgradeEffect(_slotImage.transform.position);
    }
    // 조건 확인만
    private void Refresh()
    {
        int level = 0;
        switch (_enhanceType)
        {
            case EnhanceType.Attack:
                level = PlayerManager.playerData.EnhanceData.AttackLevel;
                break;
            case EnhanceType.HpIncrease:
                level = PlayerManager.playerData.EnhanceData.HpIncreaseLevel;
                break;
            case EnhanceType.HpRecovery:
                level = PlayerManager.playerData.EnhanceData.HpRecoveryLevel;
                break;
            case EnhanceType.CritAttack:
                level = PlayerManager.playerData.EnhanceData.CritAttackLevel;
                break;
            case EnhanceType.CritChance:
                level = PlayerManager.playerData.EnhanceData.CritChanceLevel;
                break;
        }
        SetSlot(level);
    }
    private void SetSlot(int level)
    {
        int upgradeCost = PlayerManager.CharacterSystem.GetEnhanceCost(level);
        bool result = PlayerManager.GetGold() >= upgradeCost;
        _upgradeButton.interactable = result;
        if (result == false)
        {
            _upgradeButton.isHolding = false;
        }
        _costText.text = upgradeCost.ToString();
        if (_enhanceType == EnhanceType.CritChance)
        {
             if (level != 100)
             {
                 _upgradeInfoText.text = $"{level}% -> {level + 1}%";
             }
             else
             {
                 _upgradeButton.isHolding = false;
                 _upgradeButton.interactable = false;
                 _upgradeInfoText.text = $"{level}%";
             }
        
        }
        else if (_enhanceType == EnhanceType.CritAttack)
        {
            _upgradeInfoText.text = $"{level}% -> {level + 1}%";
        }
        else
        {
            _upgradeInfoText.text = $"{level} -> {level + 1}";
        }
        _upgradeLevelText.text = $"LV.{level}";
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnEnhanceChanged -= Refresh;
        Managers.PlayerManager.OnGoldChanged -= Refresh;
        GameManager.Instance.OnGameStart -= Refresh;

    }
}
