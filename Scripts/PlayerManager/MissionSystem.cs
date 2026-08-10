using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMissionProgressInfo
{
    public float CurrentPlayTime;
    public int CurrentMonsterKill;
    public int CurrentSwordSummon;
    public int CurrentRingSummon;
}

public class MissionSystem
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;

    // 일일 미션
    public DailyMissionTableSO.DailyMissionData[] DailyMissionData { get; private set; }
    public DailyMissionProgressInfo DailyMissionProgressInfo { get; private set; }
    // 업적
    public AchievementTableSO.AchievementData[] AchievementDatas { get; private set; }

    public MissionSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;

        // 기존 진행도 변경 이벤트로 Red Dot도 갱신
        _playerManager.OnDailyMissionValueChanged += OnDailyMissionChanged;
        _playerManager.OnAchievementValueChanged += OnAchievementChanged;
    }
    public void InitData()
    {
        // 일일 미션
        DailyMissionData = Managers.Resource.Load<DailyMissionTableSO>("DailyMissionTableSO").dailyMissions;

        DailyMissionProgressInfo = _playerData.DailyMissionProgressInfo;

        DailyMissionProgressInfo.CurrentPlayTime = 0;
        DailyMissionProgressInfo.CurrentMonsterKill = 0;
        DailyMissionProgressInfo.CurrentSwordSummon = 0;
        DailyMissionProgressInfo.CurrentRingSummon = 0;
        // 업적
        AchievementDatas = Managers.Resource.Load<AchievementTableSO>("AchievementTableSO").AchievementDatas;

        for (int i = 0; i < 5; i++)
        {
            _playerData.AchievementLevel[i] = 0;
        }
        _playerData.TotalEquipmentEnhance = 0;
        _playerData.TotalEquipmentMerge = 0;
    }
    // 일일 미션
    public void IncreasePlayTime(float time)
    {
        DailyMissionProgressInfo.CurrentPlayTime += time;
        _playerManager.OnDailyMissionValueChanged?.Invoke(DailyMissionType.PlayTime);
    }

    public void IncreaseMonsterKill()
    {
        DailyMissionProgressInfo.CurrentMonsterKill++;
        _playerManager.OnDailyMissionValueChanged?.Invoke(DailyMissionType.MonsterKill);
    }

    public void IncreaseSwordSummon()
    {
        DailyMissionProgressInfo.CurrentSwordSummon++;
        _playerManager.OnDailyMissionValueChanged?.Invoke(DailyMissionType.SwordSummon);
    }

    public void IncreaseRingSummon()
    {
        DailyMissionProgressInfo.CurrentRingSummon++;
        _playerManager.OnDailyMissionValueChanged?.Invoke(DailyMissionType.RingSummon);
    }

    public void CompleteDailyMission(int slotNum)
    {
        _playerManager.PlayerInfoSystem.GainDiamond(DailyMissionData[slotNum].rewardValue);
        DailyMissionData[slotNum].hasReward = true;
        _playerManager.OnDailyMissionValueChanged?.Invoke(DailyMissionData[slotNum].dailyMissionType);
    }
    // 업적
    public int GetAchievementTargetValue(int slotNum)
    {
        return (_playerData.AchievementLevel[slotNum] + 1)
               * AchievementDatas[slotNum].increaseValue;
    }

    public void CompleteAchievement(int slotNum)
    {
        _playerManager.PlayerInfoSystem.GainDiamond(AchievementDatas[slotNum].rewardValue);
        _playerData.AchievementLevel[slotNum]++;
        _playerManager.OnAchievementValueChanged?.Invoke((AchievementType)slotNum);
    }
    
    // Red Dot 조건 검사
    public bool HasDailyMissionNotification()
    {
        for (int i = 0; i < DailyMissionData.Length; i++)
        {
            int currentValue = 0;

            switch (DailyMissionData[i].dailyMissionType)
            {
                case DailyMissionType.PlayTime:
                    currentValue = Mathf.FloorToInt(DailyMissionProgressInfo.CurrentPlayTime);
                    break;

                case DailyMissionType.MonsterKill:
                    currentValue = DailyMissionProgressInfo.CurrentMonsterKill;
                    break;

                case DailyMissionType.SwordSummon:
                    currentValue = DailyMissionProgressInfo.CurrentSwordSummon;
                    break;

                case DailyMissionType.RingSummon:
                    currentValue = DailyMissionProgressInfo.CurrentRingSummon; 
                    break;
            }

            // 목표 달성 + 아직 보상 미수령
            if (currentValue >= DailyMissionData[i].targetValue && DailyMissionData[i].hasReward == false)
            {
                return true;
            }
        }
        return false;
    }
    public bool HasAchievementNotification()
    {
        for (int i = 0; i < AchievementDatas.Length; i++)
        {
            int currentValue = 0;

            switch (AchievementDatas[i].achievementType)
            {
                case AchievementType.CharacterLevelUp:
                    currentValue = _playerData.Level;
                    break;

                case AchievementType.AttackEnhance:
                    currentValue = _playerData.EnhanceData.AttackLevel;
                    break;

                case AchievementType.HpEnhance:
                    currentValue = _playerData.EnhanceData.HpIncreaseLevel;
                    break;

                case AchievementType.EquipmentEnhance:
                    currentValue = _playerData.TotalEquipmentEnhance;
                    break;

                case AchievementType.EquipmentMerge:
                    currentValue = _playerData.TotalEquipmentMerge;
                    break;
            }

            if (currentValue >= GetAchievementTargetValue(i))
            {
                return true;
            }
        }
        return false;
    }
    
    // Red Dot 갱신

    private void OnDailyMissionChanged(DailyMissionType type)
    {
        MissionRedDotCheck();
    }

    private void OnAchievementChanged(AchievementType type)
    {
        MissionRedDotCheck();
    }

    private void MissionRedDotCheck()
    {
        bool dailyMission = HasDailyMissionNotification();
        bool achievement = HasAchievementNotification();
        GameContainer.Instance.UIFeedbackManager.SetDailyMissionRedDot(dailyMission);
        GameContainer.Instance.UIFeedbackManager.SetAchievementRedDot(achievement);
        GameContainer.Instance.UIFeedbackManager.SetMissionRedDot(dailyMission || achievement);
    }
}