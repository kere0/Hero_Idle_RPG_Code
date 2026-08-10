using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPanelUI : MonoBehaviour
{
    [SerializeField] private AchievementSlot[] _achievementSlots;
    private MissionSystem _missionSystem;
    private PlayerData _playerData;
    private bool _isInit = false;

    private void Awake()
    {
        foreach (AchievementSlot slot in _achievementSlots)
        {
            slot.achievementPanelUI = this;
        }
    }
    private void Start()
    { 
        _missionSystem = Managers.PlayerManager.MissionSystem;
        _playerData = Managers.PlayerManager.playerData;
        Init();
        _isInit = true;
    }
    private void OnEnable()
    {
        Managers.PlayerManager.OnAchievementValueChanged += Refresh;
        Managers.PlayerManager.OnLevelUp += LevelUpAchievement;
        if (_isInit == true)
        {
            Init();
        }
    }
    private void Init()
    {
        AchievementTableSO.AchievementData[] achievementDatas = _missionSystem.AchievementDatas;
        int currentValue = 0;
        for (int i = 0; i < _achievementSlots.Length; i++)
        {
            switch (i)
            {
                case (int)AchievementType.CharacterLevelUp:
                    currentValue = _playerData.Level;
                    break;
                case (int)AchievementType.AttackEnhance:
                    currentValue = _playerData.EnhanceData.AttackLevel;
                    break;
                case (int)AchievementType.HpEnhance:
                    currentValue = _playerData.EnhanceData.HpIncreaseLevel;
                    break;
                case (int)AchievementType.EquipmentEnhance:
                    currentValue = _playerData.TotalEquipmentEnhance;
                    break;
                case (int)AchievementType.EquipmentMerge:
                    currentValue = _playerData.TotalEquipmentMerge;
                    break;
            }
            _achievementSlots[i].InitInfo(i, 
                achievementDatas[i].description, 
                currentValue, 
                _missionSystem.GetAchievementTargetValue(i), 
                achievementDatas[i].rewardValue,
                currentValue >= _missionSystem.GetAchievementTargetValue(i)); 
        }
    }
    private void Refresh(AchievementType achievementType)
    {
        int maxValue = 0;
        int currentValue = 0;
        maxValue = _missionSystem.GetAchievementTargetValue((int)achievementType);
        switch (achievementType)
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
        _achievementSlots[(int)achievementType].Refresh(currentValue ,maxValue, currentValue >= maxValue);
    }

    private void LevelUpAchievement()
    {
        int maxValue = 0;
        int currentValue = 0;

        maxValue = _missionSystem.GetAchievementTargetValue((int)AchievementType.CharacterLevelUp);
        currentValue = _playerData.Level;
        _achievementSlots[(int)AchievementType.CharacterLevelUp].Refresh(currentValue ,maxValue, currentValue >= maxValue);

    }
    public void AchievementCompleteButtonClick(int slotNum)
    {
        _missionSystem.CompleteAchievement(slotNum);
        Refresh((AchievementType)slotNum);
    }
    private void OnDisable()
    {
        Managers.PlayerManager.OnAchievementValueChanged -= Refresh;
    }
}
