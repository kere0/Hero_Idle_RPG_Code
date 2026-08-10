using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float TotalPlayTime;
    public int CurrentStage = 1;
    public int Level = 1;
    public int DefaultHp = 100;
    public int DefaltMana = 100;
    public int HpRegenPerSecond = 1;
    public float ManaRegenPerSecond = 1;
    public float DefaultAttackSpeed = 1; // 초당 2번
    public EnhanceData EnhanceData = new EnhanceData();
    public GrowthData GrowthData = new GrowthData();
    public int StatPoint = 0;
    public int SkillPoint = 0;
    public int Exp;
    public int Gold = 0;
    public int Diamond = 0;
    public int EnhanceStone = 0;
    public SwordDataInstance[] SwordInstances = new SwordDataInstance[16];
    public RingDataInstance[] RingInstances = new RingDataInstance[16];
    
    public int EquippedSwordSlotNum = 0;
    public int EquippedRingSlotNum = 0;
    
    public SkillInstance[] SkillInstances;
    public SkillInstance[] EquippedSkillInstances;
    
    public int[] MaxDungeonLevel = new int[3];
    // 장비 소환
    public int SwordSummonLevel;
    public int SwordSummonCount;
    public int RingSummonLevel;
    public int RingSummonCount;
    // 퀘스트
    public int GuideQuestIndex; // 현재 몇 번째 퀘스트인지
    public int GuideQuestCycle; // 몇 번째 반복인지
    public int EquipmentSummonQuestCount;
    // 미션 진행도
    public DailyMissionProgressInfo DailyMissionProgressInfo = new DailyMissionProgressInfo();
    // 업적
    public int[] AchievementLevel = new int[5];
    public int TotalEquipmentEnhance = 0;
    public int TotalEquipmentMerge = 0;
    // 광고
    public bool hasUsedGoldAd = false;
    public bool hasUsedExpAd = false;
    public bool hasUsedAttackAd = false;
    public bool[] HasUsedAd = new bool[3];
} 
