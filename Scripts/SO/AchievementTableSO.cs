using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementType
{
    CharacterLevelUp,
    AttackEnhance,
    HpEnhance,
    EquipmentEnhance,
    EquipmentMerge
}
[CreateAssetMenu(menuName = "Table/AchievementTableSO")]
public class AchievementTableSO : ScriptableObject
{
    public AchievementData[] AchievementDatas;
    [Serializable]
    public class AchievementData
    {
        public AchievementType achievementType;
        public string description;
        public int increaseValue;
        public int rewardValue;
    }
}
