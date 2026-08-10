using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueCalculator
{
    // 플레이어 경험치량 계산
    public static int GetPlayerMaxExp(int level)
    {
        return Mathf.RoundToInt(100 * Mathf.Pow(1.15f, level - 1));
    }
    // 장비 강화 수치 계산
    public static float GetEquipmentEnhanceValue(int defaultValue, int enhanceLevel)
    {
        return defaultValue * Mathf.Pow(1.08f, enhanceLevel);
    }
    // 스테이지 보상 계산
    public static int GetStageMonsterReward(ItemType itemType, int currentStage)
    {
        int reward = 0;
        switch (itemType)
        {
            case ItemType.Gold:
                reward = Mathf.RoundToInt(10 * Mathf.Pow(1.08f, currentStage - 1));
                break;
            case ItemType.Exp:
                reward = Mathf.RoundToInt(15 * Mathf.Pow(1.5f, currentStage - 1));
                break;
            case ItemType.EnhanceStone:
                reward = Mathf.RoundToInt(1 * Mathf.Pow(1.5f, currentStage - 1));
                break;
        }
        return reward;
    }
    // 던전 보상 계산
    public static int GetDungeonReward(DungeonType dungeonType, int dungeonLevel)
    {
        int reward = 0;
        switch (dungeonType)
        {
            case DungeonType.GoldDungeon:
                reward = Mathf.RoundToInt(100 * Mathf.Pow(1.5f, dungeonLevel - 1) * 5);
                break;
            case DungeonType.ExpDungeon:
                reward = Mathf.RoundToInt(10 * Mathf.Pow(1.5f, dungeonLevel - 1) * 5);
                break;
            case DungeonType.EnhanceStoneDungeon:
                reward = Mathf.RoundToInt(5 * Mathf.Pow(2f, dungeonLevel - 1) * 5);
                break;
        }
        return reward;
    }
    public static int GetMonsterHp(int stageLevel)
    {
        int monsterHp = Mathf.RoundToInt(30 * Mathf.Pow(1.5f, stageLevel - 1));
        return monsterHp;
    }
    public static int GetDungeonBossHp(int dungeonLevel)
    {
        int bossMonsterHp = Mathf.RoundToInt(500 * Mathf.Pow(3f, dungeonLevel - 1));
        return bossMonsterHp;
    }
    public static int GetDungeonBossAttack(int dungeonLevel)
    {
        int monsterAttack = Mathf.RoundToInt(10 * Mathf.Pow(1.5f, dungeonLevel - 1));
        return monsterAttack;
    }
    public static int GetStageBossHp(int stageLevel)
    {
        int bossMonsterHp = Mathf.RoundToInt(300 * Mathf.Pow(2f, stageLevel - 1));
        return bossMonsterHp;
    }
    public static int GetStageBossAttack(int stageLevel)
    {
        int monsterAttack = Mathf.RoundToInt(10 * Mathf.Pow(1.5f, stageLevel - 1));
        return monsterAttack;
    }
}
