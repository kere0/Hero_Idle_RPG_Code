using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardSystem
{
    private BattleManager _battleManager;
    private ItemDropper _itemDropper;
    private int _dungeonBossrewardCount = 5;

    public RewardSystem(BattleManager battleManager)
    {
        _battleManager = battleManager;
        _itemDropper = new ItemDropper();
    }
    public int MonsterReward(BaseMonster monster)
    {
        int currentStage = Managers.PlayerManager.playerData.CurrentStage;
        int goldValue = 0;
        int expValue = 0;
        int enhanceStoneValue = 0;
        int totalValue = 0;
        switch(monster.MonsterType)
        {
            case MonsterType.Normal:
                goldValue = ValueCalculator.GetStageMonsterReward(ItemType.Gold, currentStage);
                expValue = ValueCalculator.GetStageMonsterReward(ItemType.Exp, currentStage);
                // 골드
                _itemDropper.GoldDrop(monster._collider.bounds.center, goldValue);
                int rand = Random.Range(0, 100);
                if (rand < 30)
                {
                    // 강화석
                    enhanceStoneValue = ValueCalculator.GetStageMonsterReward(ItemType.EnhanceStone, currentStage);
                    _itemDropper.EnhanceStoneDrop(monster._collider.bounds.center, enhanceStoneValue);
                }
                // 경험치 오르게
                GainReward(ItemType.Exp, expValue);
                break;
            case MonsterType.StageBoss:
                goldValue = ValueCalculator.GetStageMonsterReward(ItemType.Gold, currentStage);
                expValue = ValueCalculator.GetStageMonsterReward(ItemType.Exp, currentStage);
                for (int i = 0; i < 7; i++)
                {
                    GainReward(ItemType.Gold, goldValue);
                    GainReward(ItemType.Exp, expValue);
                }
                break;
            case MonsterType.TreasureChest:
                goldValue = ValueCalculator.GetStageMonsterReward(ItemType.Gold, currentStage);
                expValue = ValueCalculator.GetStageMonsterReward(ItemType.Exp, currentStage);
                for (int i = 0; i < 5; i++)
                {
                    GainReward(ItemType.Gold, goldValue);
                    GainReward(ItemType.Exp, expValue);
                }
                break;
            case MonsterType.GoldDungeon:
                goldValue = ValueCalculator.GetDungeonReward(DungeonType.GoldDungeon, Managers.PlayerManager.playerData.CurrentStage) / 5;
                for (int i = 0; i < _dungeonBossrewardCount; i++)
                {
                    GainReward(ItemType.Gold, goldValue);
                }
                // 보상 패널 띄우기
                Managers.PlayerManager.playerData.MaxDungeonLevel[(int)DungeonType.GoldDungeon]++;
                totalValue = goldValue * _dungeonBossrewardCount;
                break;
            case MonsterType.ExpDungeon:
                expValue = ValueCalculator.GetDungeonReward(DungeonType.ExpDungeon, Managers.PlayerManager.playerData.CurrentStage) / 5;
                for (int i = 0; i < _dungeonBossrewardCount; i++)
                {
                    GainReward(ItemType.Exp, expValue);
                }
                // 보상 패널 띄우기
                Managers.PlayerManager.playerData.MaxDungeonLevel[(int)DungeonType.ExpDungeon]++;
                totalValue = expValue * _dungeonBossrewardCount;
                break;
            case MonsterType.EnhanceStoneDungeon:
                enhanceStoneValue = ValueCalculator.GetDungeonReward(DungeonType.EnhanceStoneDungeon, Managers.PlayerManager.playerData.CurrentStage) / 5;
                for (int i = 0; i < _dungeonBossrewardCount; i++)
                {
                    GainReward(ItemType.EnhanceStone, enhanceStoneValue);
                }
                // 보상 패널 띄우기
                Managers.PlayerManager.playerData.MaxDungeonLevel[(int)DungeonType.EnhanceStoneDungeon]++;
                totalValue = enhanceStoneValue * _dungeonBossrewardCount;
                break;
        }
        return totalValue;
    }
    // 최종
    public void GainReward(ItemType itemType, int value)
    {
        int totalValue = 0;
        switch (itemType)
        {
            case ItemType.Gold:
                // 수치
                float goldGainMultiplier = (1f + Managers.PlayerManager.playerData.GrowthData.GoldGainLevel / 100f);
                totalValue = Mathf.RoundToInt(value * goldGainMultiplier * Managers.PlayerManager.AdBuffSystem.GoldAdBuffMultiplier);
                Managers.PlayerManager.PlayerInfoSystem.GainGold(totalValue);
                break;
            case ItemType.Exp:
                // 수치
                totalValue = Mathf.RoundToInt(value * Managers.PlayerManager.AdBuffSystem.ExpAdBuffMultiplier);
                Managers.PlayerManager.PlayerInfoSystem.GainExp(totalValue);
                break;
            case ItemType.EnhanceStone:
                totalValue = value;
                Managers.PlayerManager.PlayerInfoSystem.GainEnhanceStone(totalValue);
                break;
        }
        // UI
        GameContainer.Instance.AcquireInfoUI.PushInfoSlot(totalValue, itemType);
    }
}
