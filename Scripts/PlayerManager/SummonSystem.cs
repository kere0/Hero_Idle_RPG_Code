using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSystem
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;
    public const int OneSummonDiamondCost = 50;
    public const int TenSummonDiamondCost = 500;
    private const int EquipmentSummonQuestMaxCount = 10;
    public SummonSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;
    }
    public int GetMaxSummonCount(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Sword:
                return _playerData.SwordSummonLevel * 50;
            
            case EquipmentType.Ring:
                return _playerData.RingSummonLevel * 50;
        }
        return 0;
    }
    public void InitData()
    {
        _playerData.SwordSummonLevel = 1;
        _playerData.SwordSummonCount = 0;
        _playerData.RingSummonLevel = 1;
        _playerData.RingSummonCount = 0;
        _playerData.EquipmentSummonQuestCount = 0;
    }

    public void SetSummonCount(int value)
    {
        if (_playerData.EquipmentSummonQuestCount >= EquipmentSummonQuestMaxCount)
            return;
        _playerData.EquipmentSummonQuestCount += value;
        _playerData.EquipmentSummonQuestCount = Mathf.Min(_playerData.EquipmentSummonQuestCount, EquipmentSummonQuestMaxCount);
        _playerManager.OnGuideQuestValueChanged?.Invoke(GuideQuestType.EquipmentSummon);
    }
    public void RefreshSummonGauge(EquipmentType equipmentType, int amount)
    {
        switch (equipmentType)
        {
            case EquipmentType.Sword:
                _playerData.SwordSummonCount += amount;
                if (_playerData.SwordSummonCount >= GetMaxSummonCount(equipmentType))
                {
                    _playerData.SwordSummonCount -= GetMaxSummonCount(equipmentType);
                    _playerData.SwordSummonLevel++;
                }
                _playerManager.OnSwordSummon?.Invoke();
                break;
            case EquipmentType.Ring:
                _playerData.RingSummonCount += amount;
                if (_playerData.RingSummonCount >= GetMaxSummonCount(equipmentType))
                {
                    _playerData.RingSummonCount -= GetMaxSummonCount(equipmentType);
                    _playerData.RingSummonLevel++;
                }
                _playerManager.OnRingSummon?.Invoke();
                break;
        }
    }
}
