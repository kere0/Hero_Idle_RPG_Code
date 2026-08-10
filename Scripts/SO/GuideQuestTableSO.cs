using System;
using UnityEngine;

public enum GuideQuestType
{
    Attack,
    HpIncrease,
    HpRecovery,
    CritAttack,
    EquipmentSummon
}

[CreateAssetMenu(menuName = "Table/GudieQuestTableSO")]
public class GuideQuestTableSO : ScriptableObject
{
    public GuideQuestData[] guideQuests;
    [Serializable]
    public class GuideQuestData
    {
        public GuideQuestType guideQuestType;
        public string description;
        public int startValue;
        public int increaseValue;
        public int rewardValue;
    }
}
