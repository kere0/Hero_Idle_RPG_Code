using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public readonly struct GuideQuestUIInfo
{
    public readonly GuideQuestType GuideQuestType;
    public readonly string Description;
    public readonly string Progress;
    public readonly int Reward;
    public readonly bool Completed;
    public GuideQuestUIInfo(GuideQuestType guideQuestType, string description, string progress, int reward, bool completed)
    {
        GuideQuestType = guideQuestType;
        Description = description;
        Progress = progress;
        Reward = reward;
        Completed = completed;
    }
}
public class GuideQuestSystem
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;
    private GuideQuestTableSO.GuideQuestData[] _guideQuestData;
    private GuideQuestTableSO.GuideQuestData _currentGuideQuestData;
    private int _currentCycle;
    public GuideQuestSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;
    }
    public void InitData()
    {
        _guideQuestData = Managers.Resource.Load<GuideQuestTableSO>("GuideQuestTableSO").guideQuests;
        _currentGuideQuestData = _guideQuestData[_playerData.GuideQuestIndex];
        _currentCycle = _playerData.GuideQuestCycle;
    }
    public void CompleteQuest()
    {
        _playerManager.PlayerInfoSystem.GainDiamond(_guideQuestData[_playerData.GuideQuestIndex].rewardValue);
        _playerData.GuideQuestIndex++;
        if (_playerData.GuideQuestIndex >= _guideQuestData.Length)
        {
            _playerData.GuideQuestIndex = 0;
            _playerData.GuideQuestCycle++;
        }
        _currentGuideQuestData = _guideQuestData[_playerData.GuideQuestIndex];
        _currentCycle = _playerData.GuideQuestCycle;
    }
    public GuideQuestUIInfo GetGuideQuestInfo()
    {
        string description = _currentGuideQuestData.description;
        int currentLevel = 0;
        int rewardValue = _currentGuideQuestData.rewardValue;
        bool completed = false;
        switch (_currentGuideQuestData.guideQuestType)
        {
            case GuideQuestType.Attack:
                currentLevel = _playerData.EnhanceData.AttackLevel;
                break;
            case GuideQuestType.HpIncrease:
                currentLevel = _playerData.EnhanceData.HpIncreaseLevel;
                break;
            case GuideQuestType.HpRecovery:
                currentLevel = _playerData.EnhanceData.HpRecoveryLevel;
                break;
            case GuideQuestType.CritAttack:
                currentLevel = _playerData.EnhanceData.CritAttackLevel;
                break;
            case GuideQuestType.EquipmentSummon:
                currentLevel = _playerData.EquipmentSummonQuestCount;
                break;
        }
        int guideTargetLevel = _currentGuideQuestData.startValue + _currentCycle * _currentGuideQuestData.increaseValue;
        if (currentLevel >= guideTargetLevel)
        {
            completed = true;
            if (_currentGuideQuestData.guideQuestType == GuideQuestType.EquipmentSummon)
            {
                _playerData.EquipmentSummonQuestCount = 0;
            }
        }
        GuideQuestType guideQuestType = _currentGuideQuestData.guideQuestType;
        string progress = $"{currentLevel} / {guideTargetLevel}";
        return new GuideQuestUIInfo(guideQuestType, description, progress, rewardValue, completed);
    }
}
