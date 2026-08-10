using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;
    public CharacterSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;
    }
    public void InitData()
    {
        _playerData.EnhanceData = new EnhanceData()
        {
            AttackLevel = 1,
            HpIncreaseLevel = 1,
            HpRecoveryLevel = 1,
            CritChanceLevel = 1,
            CritAttackLevel = 1
        };
        _playerData.GrowthData = new GrowthData()
        {
            AttackLevel = 0,
            HpIncreaseLevel = 0,
            HpRecoveryLevel = 0,
            CritAttackLevel = 0,
            GoldGainLevel = 0
        };
        _playerData.StatPoint = 0;
    }
    // 강화
    public void UpgradeEnhanceData(EnhanceType enhanceType)
    { 
        switch (enhanceType)
        {
            case EnhanceType.Attack:
                _playerData.Gold -= GetEnhanceCost(_playerData.EnhanceData.AttackLevel);
                _playerData.EnhanceData.AttackLevel += 1;
                _playerManager.OnGuideQuestValueChanged?.Invoke(GuideQuestType.Attack);
                _playerManager.OnAchievementValueChanged?.Invoke(AchievementType.AttackEnhance);
                break;
            case EnhanceType.HpIncrease:
                _playerData.Gold -= GetEnhanceCost(_playerData.EnhanceData.HpIncreaseLevel);
                _playerData.EnhanceData.HpIncreaseLevel += 1;
                _playerManager.OnGuideQuestValueChanged?.Invoke(GuideQuestType.HpIncrease);
                _playerManager.OnAchievementValueChanged?.Invoke(AchievementType.HpEnhance);
                break;
            case EnhanceType.HpRecovery:
                _playerData.Gold -= GetEnhanceCost(_playerData.EnhanceData.HpRecoveryLevel);
                _playerData.EnhanceData.HpRecoveryLevel += 1;
                _playerManager.OnGuideQuestValueChanged?.Invoke(GuideQuestType.HpRecovery);
                break;
            case EnhanceType.CritAttack:
                _playerData.Gold -= GetEnhanceCost(_playerData.EnhanceData.CritAttackLevel);
                _playerData.EnhanceData.CritAttackLevel += 1;
                _playerManager.OnGuideQuestValueChanged?.Invoke(GuideQuestType.CritAttack);
                break;
            case EnhanceType.CritChance:
                _playerData.Gold -= GetEnhanceCost(_playerData.EnhanceData.CritChanceLevel);
                _playerData.EnhanceData.CritChanceLevel += 1;
                break;
        }
        _playerManager.OnGoldChanged.Invoke();
        _playerManager.OnEnhanceChanged.Invoke();
    }
    public int GetEnhanceCost(int level)
    {
        return 1 + level * (level - 1) / 2;
    }
    public void UpgradeGrowthData(GrowthType growthType)
    {
        switch (growthType)
        {
            case GrowthType.Attack:
                _playerData.GrowthData.AttackLevel += 1;
                break;
            case GrowthType.HpIncrease:
                _playerData.GrowthData.HpIncreaseLevel += 1;
                break;
            case GrowthType.HpRecovery:
                _playerData.GrowthData.HpRecoveryLevel += 1;
                break;
            case GrowthType.CritAttack:
                _playerData.GrowthData.CritAttackLevel += 1;
                break;
            case GrowthType.GoldRate:
                _playerData.GrowthData.GoldGainLevel += 1;
                break;
        }
        _playerData.StatPoint--;
        _playerManager.OnGrowthChanged.Invoke();
    }
}
