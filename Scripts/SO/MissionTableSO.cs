using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DailyMissionType
{
    PlayTime,
    MonsterKill,
    SwordSummon,
    RingSummon
}
[CreateAssetMenu(menuName = "Table/DailyMissionTableSO")]
public class DailyMissionTableSO : ScriptableObject
{
    public DailyMissionData[] dailyMissions;
    [Serializable]
    public class DailyMissionData
    {
        public DailyMissionType dailyMissionType;
        public string description;
        public int targetValue;
        public int rewardValue;
        public bool hasReward;
    }
}


