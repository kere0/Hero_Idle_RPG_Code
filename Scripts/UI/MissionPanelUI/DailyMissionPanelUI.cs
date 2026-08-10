using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionPanelUI : MonoBehaviour
{
    [SerializeField] private DailyMissionSlot[] _dailyMissionSlots;
    private MissionSystem _missionSystem;
    private bool _isInit = false;
    private void Awake()
    {
        foreach (DailyMissionSlot slot in _dailyMissionSlots)
        {
            slot.dailyMissionPanel = this;
        }
    }
    private void Start()
    { 
        _missionSystem = Managers.PlayerManager.MissionSystem;
        Init();
        _isInit = true;
    }
    private void OnEnable()
    {
        Managers.PlayerManager.OnDailyMissionValueChanged += Refresh;
        if (_isInit == true)
        {
            Init();
        }
    }
    private void Init()
    {
        DailyMissionTableSO.DailyMissionData[] dailyMissionDatas = _missionSystem.DailyMissionData;
        DailyMissionProgressInfo progressInfo = _missionSystem.DailyMissionProgressInfo;
        int currentValue = 0;
        for (int i = 0; i < _dailyMissionSlots.Length; i++)
        {
            switch (i)
            {
                case (int)DailyMissionType.PlayTime:
                    currentValue = Mathf.FloorToInt(Managers.PlayerManager.playerData.TotalPlayTime);
                    break;
                case (int)DailyMissionType.MonsterKill:
                    currentValue = progressInfo.CurrentMonsterKill;
                    break;
                case (int)DailyMissionType.SwordSummon:
                    currentValue = progressInfo.CurrentSwordSummon;
                    break;
                case (int)DailyMissionType.RingSummon:
                    currentValue = progressInfo.CurrentRingSummon;
                    break;
            }
            _dailyMissionSlots[i].InitInfo(i, 
                dailyMissionDatas[i].description, 
                currentValue, 
                dailyMissionDatas[i].targetValue, 
                dailyMissionDatas[i].rewardValue,
                currentValue >= dailyMissionDatas[i].targetValue); 
        }
    }
    private void Refresh(DailyMissionType dailyMissionType)
    {
        int maxValue = 0;
        int currentValue = 0;
        DailyMissionProgressInfo progressInfo = _missionSystem.DailyMissionProgressInfo;
        maxValue = _missionSystem.DailyMissionData[(int)dailyMissionType].targetValue;
        switch (dailyMissionType)
        {
            case DailyMissionType.PlayTime:
                currentValue = Mathf.FloorToInt(Managers.PlayerManager.playerData.TotalPlayTime);
                break;
            case DailyMissionType.MonsterKill:
                currentValue = progressInfo.CurrentMonsterKill;
                break;
            case DailyMissionType.SwordSummon:
                currentValue = progressInfo.CurrentSwordSummon;
                break;
            case DailyMissionType.RingSummon:
                currentValue = progressInfo.CurrentRingSummon;
                break;
        }
        _dailyMissionSlots[(int)dailyMissionType].Refresh(currentValue ,maxValue, currentValue >= maxValue);
    }
    public void DailyMissionCompleteButtonClick(int slotNum)
    {
        _missionSystem.CompleteDailyMission(slotNum);
    }
    private void OnDisable()
    {
        Managers.PlayerManager.OnDailyMissionValueChanged -= Refresh;
    }
}
