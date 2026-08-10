using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnhanceType
{
    Attack,
    HpIncrease,
    HpRecovery,
    CritAttack,
    CritChance
}
public enum GrowthType
{
    Attack,
    HpIncrease,
    HpRecovery,
    CritAttack,
    GoldRate
}

public enum EquipmentRarity
{
    Normal,
    Rare,
    Unique,
    Legend
}

public enum EquipmentType
{
    Sword,
    Ring
}
public class PlayerManager
{
    public PlayerData playerData;
    // System
    public CharacterSystem CharacterSystem;
    public PlayerInfoSystem PlayerInfoSystem;
    public EquipmentSystem EquipmentSystem;
    public SkillSystem SkillSystem;
    public GuideQuestSystem GuideQuestSystem;
    public MissionSystem MissionSystem;
    public BuffSystem BuffSystem;
    public SummonSystem SummonSystem;
    public AdBuffSystem AdBuffSystem;
    public PlayerTimeSystem PlayerTimeSystem;
    // 무기 
    // Action
    public Action OnLevelUp;
    public Action OnEnhanceChanged;
    public Action OnGrowthChanged;
    public Action OnSkillLevelChanged;
    public Action OnExpChanged;
    public Action OnGoldChanged;
    public Action OnEnhanceStoneChanged;
    public Action OnDiamondChanged;
    public Action OnSwordSummon;
    public Action OnRingSummon;
    
    // 전투 시작
    public Action OnBattleStart;
    public Action OnBuffSkill;
    // 아이템 합성
    public Action<int> OnSwordMergeComplete;
    public Action<int> OnRingMergeComplete;
    // 아이템 강화
    public Action<int> OnSwordEnhanceComplete;
    public Action<int> OnRingEnhanceComplete;
    // 장비 장착
    public Action OnSwordChanged;
    public Action OnRingChanged;
    
    // 퀘스트
    public Action<GuideQuestType> OnGuideQuestValueChanged;
    // 미션
    public Action<DailyMissionType> OnDailyMissionValueChanged;
    public Action<AchievementType> OnAchievementValueChanged;
    public Action OnDailyMissionRewardAvailableChanged;
    public Action OnAchievementRewardAvailableChanged;
    public void Init()
    {
        playerData = new PlayerData();
        CharacterSystem = new CharacterSystem(this, playerData);
        PlayerInfoSystem = new PlayerInfoSystem(this, playerData);
        EquipmentSystem = new EquipmentSystem(this, playerData);
        SkillSystem = new SkillSystem(this, playerData);
        GuideQuestSystem = new GuideQuestSystem(this, playerData);
        MissionSystem = new MissionSystem(this, playerData);
        BuffSystem = new BuffSystem(this, playerData);
        SummonSystem = new SummonSystem(this, playerData);
        AdBuffSystem = new AdBuffSystem();
        PlayerTimeSystem = new PlayerTimeSystem(playerData);
        SetData();
        GameManager.Instance.GameStart();
        RedDotCheckEvent();
    }

    private void RedDotCheckEvent()
    {
        
    }
    private void SetData()
    {
        CharacterSystem.InitData();
        PlayerInfoSystem.InitData();
        EquipmentSystem.InitData();
        SkillSystem.InitData();
        GuideQuestSystem.InitData();
        MissionSystem.InitData();
        SummonSystem.InitData();
        PlayerTimeSystem.InitData();
        for (int i = 0; i < 3; i++)
        {
            playerData.MaxDungeonLevel[i] = 1;
        }
    }
    // 공격 간격
    public float GetAttackInterval()
    {
        return 1f / (playerData.DefaultAttackSpeed * BuffSystem.TotalAttackSpeedMultiplier);
    }
    // 최종 데미지
     public int GetTotalDamage()
     {
         int enhanceDamage = playerData.EnhanceData.AttackLevel;
         int growthDamage = playerData.GrowthData.AttackLevel;
         float swordMultiplier = 1 + (GetSwordAttackIncreasePercent(playerData.EquippedSwordSlotNum) / 100);
    
         return Mathf.RoundToInt((enhanceDamage + growthDamage) * swordMultiplier * BuffSystem.TotalAttackBuffMultiplier * GetOwnedSwordIncreaseRate() * AdBuffSystem.AttackAdBuffMultiplier);
     }
     // 크리티컬 데미지
     public int GetTotalCriticalDamage()
     {
         float critMultiplier = 1f + (playerData.EnhanceData.CritAttackLevel + playerData.GrowthData.CritAttackLevel) / 100f;
         return Mathf.RoundToInt(GetTotalDamage() * critMultiplier);
     }
     public float GetSwordAttackIncreasePercent(int id)
     {
         int defaultValue = playerData.SwordInstances[id].EquipmentData.Value;
         int enhanceLevel = playerData.SwordInstances[id].EnhanceLevel;
         return ValueCalculator.GetEquipmentEnhanceValue(defaultValue, enhanceLevel);
     } 
    // 체력 회복
    public int GetTotalMaxHp()
    {
        return playerData.DefaultHp + playerData.EnhanceData.HpIncreaseLevel + playerData.GrowthData.HpIncreaseLevel;
    }
    // 최종 초당 체력 회복량
    public int GetTotalHpRegenPerSecond()
    {
        return playerData.HpRegenPerSecond + playerData.EnhanceData.HpRecoveryLevel + playerData.GrowthData.HpRecoveryLevel;
    }
    // 최종 초당 마나 회복량
    public float GetTotalManaRegenPerSecond()
    {
        float ringMultiplier = GetRingManaRegenPercent(playerData.EquippedRingSlotNum) / 100;
        return playerData.ManaRegenPerSecond * (1 + ringMultiplier) * GetOwnedManaIncreaseRate();
    }
    public float GetRingManaRegenPercent(int id)
    {
        int defaultValue = playerData.RingInstances[id].EquipmentData.Value;
        int enhanceLevel = playerData.RingInstances[id].EnhanceLevel;
        return ValueCalculator.GetEquipmentEnhanceValue(defaultValue, enhanceLevel);
    }
    public float GetOwnedManaIncreaseRate()
    {
        float totalRate = 0;
        foreach (RingDataInstance ringInstance in playerData.RingInstances)
        {
            if(ringInstance.IsUnlocked == false) continue;
            totalRate += GetRingManaRegenPercent(ringInstance.EquipmentData.ItemId) / 10000f;
        }
        totalRate = 1 + totalRate;
        return totalRate;
    }
    public float GetOwnedSwordIncreaseRate()
    {
        float totalRate = 0;
        foreach (SwordDataInstance swordInstance in playerData.SwordInstances)
        {
            if(swordInstance.IsUnlocked == false) continue;
            totalRate += GetSwordAttackIncreasePercent(swordInstance.EquipmentData.ItemId) / 10000f;
        }
        totalRate = 1 + totalRate;
        return totalRate;
    }
    public int GetGold()
    {
        return playerData.Gold;
    }
}
