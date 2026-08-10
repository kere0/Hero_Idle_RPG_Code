using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrowthSlot : CharacterSlot
{
    [SerializeField] private GrowthType _growthType;
    protected override void UpgradeButtonClick()
    {
        PlayerManager.CharacterSystem.UpgradeGrowthData(_growthType);
        Canvas_Menu.Instance.PlayUpgradeEffect(_slotImage.transform.position);
    }
    private void OnEnable()
    {
        Refresh();
    }
    private void Start()
    {
        Managers.PlayerManager.OnGrowthChanged += Refresh;
        Managers.PlayerManager.OnLevelUp += Refresh;
    }
    // 조건 확인만
    private void Refresh()
    {
        int level = 0;
        switch (_growthType)
        {
            case GrowthType.Attack:
                level = PlayerManager.playerData.GrowthData.AttackLevel;
                break;
            case GrowthType.HpIncrease:
                level = PlayerManager.playerData.GrowthData.HpIncreaseLevel;
                break;
            case GrowthType.HpRecovery:
                level = PlayerManager.playerData.GrowthData.HpRecoveryLevel;
                break;
            case GrowthType.CritAttack:
                level = PlayerManager.playerData.GrowthData.CritAttackLevel;
                break;
            case GrowthType.GoldRate:
                level = PlayerManager.playerData.GrowthData.GoldGainLevel;
                break;
        }
        SetSlot(level);
    }
    private void SetSlot(int level)
    {
        bool result = PlayerManager.playerData.StatPoint >= 1;
        _upgradeButton.interactable = result;
        if (result == false)
        {
            _upgradeButton.isHolding = false;
        }
        if (_growthType == GrowthType.CritAttack || _growthType == GrowthType.GoldRate)
        {
            _upgradeInfoText.text = $"+{level}% -> +{level + 1}%";
        }
        else
        {
            _upgradeInfoText.text = $"+{level} -> +{level + 1}";
        }
        _upgradeLevelText.text = $"LV.{level}";
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnGrowthChanged -= Refresh;
        Managers.PlayerManager.OnLevelUp -= Refresh;
    }
}
