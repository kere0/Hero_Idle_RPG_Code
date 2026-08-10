using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;
    public const int MergeRequiredCount = 5;
    public EquipmentSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;
    }
    public void InitData()
    {
        // 장비 인스턴스 정보 세팅
        // Sword
        _playerData.SwordInstances = new SwordDataInstance[16];
        for (int i = 0; i < _playerData.SwordInstances.Length; i++)
        {
            _playerData.SwordInstances[i] = new SwordDataInstance();
            _playerData.SwordInstances[i].EquipmentData = Managers.Data.SwordSlotData[i];
            _playerData.SwordInstances[i].EnhanceLevel = 0;
            _playerData.SwordInstances[i].Count = 0;
            _playerData.SwordInstances[i].IsUnlocked = false;
        }
        _playerData.SwordInstances[0].Count = 1;
        _playerData.EquippedSwordSlotNum = 0;
        _playerData.SwordInstances[0].IsUnlocked = true;
        // Ring
        _playerData.RingInstances = new RingDataInstance[16];
        for (int i = 0; i < _playerData.SwordInstances.Length; i++)
        {
            _playerData.RingInstances[i] = new RingDataInstance();
            _playerData.RingInstances[i].EquipmentData = Managers.Data.RingSlotData[i];
            _playerData.RingInstances[i].EnhanceLevel = 0;
            _playerData.RingInstances[i].Count = 0;
            _playerData.RingInstances[i].IsUnlocked = false;
        }
        _playerData.RingInstances[0].Count = 1;
        _playerData.EquippedRingSlotNum = 0;
        _playerData.RingInstances[0].IsUnlocked = true;
    }

    public void EqipEquipment(EquipmentType equipmentType, int index)
    {
        switch (equipmentType)
        {
            case EquipmentType.Sword:
                _playerData.EquippedSwordSlotNum = index;
                break;
            case EquipmentType.Ring:
                _playerData.EquippedRingSlotNum = index;
                break;
        }
    }
    // 장비 강화석 강화
    public void UpgradeEquipment(EquipmentType equipmentType, int itemId)
    {
        switch (equipmentType)
        {
            case EquipmentType.Sword:
                _playerData.EnhanceStone -= GetEnhanceStoneCost(_playerData.SwordInstances[itemId].EnhanceLevel);
                _playerData.SwordInstances[itemId].EnhanceLevel++;
                break;
            case EquipmentType.Ring:
                _playerData.EnhanceStone -= GetEnhanceStoneCost(_playerData.RingInstances[itemId].EnhanceLevel);
                _playerData.RingInstances[itemId].EnhanceLevel++;
                break;
        }
        _playerData.TotalEquipmentEnhance++;
        _playerManager.OnAchievementValueChanged?.Invoke(AchievementType.EquipmentEnhance);
    }
    public int GetEnhanceStoneCost(int level)
    {
        return 10 * Mathf.RoundToInt(10 * Mathf.Pow(1.2f, level - 1));
    }
    // 합성
    public void MergeEquipment(BaseEquipmentDataInstance equipmentDataInstance, int itemId)
    {
        equipmentDataInstance.Count -= 5;
        Managers.PlayerManager.playerData.SwordInstances[itemId].Count++;
        _playerData.TotalEquipmentMerge++;
        _playerManager.OnAchievementValueChanged?.Invoke(AchievementType.EquipmentMerge);
    }
    public void MergeAllEquipment(EquipmentType equipmentType)
    {
        BaseEquipmentDataInstance[] equipmentDataInstance = new BaseEquipmentDataInstance[BaseEquipmentDataInstance.MaxCount];
        switch (equipmentType)
        {
            case EquipmentType.Sword:
                equipmentDataInstance = _playerData.SwordInstances;
                break;
            case EquipmentType.Ring:
                equipmentDataInstance = _playerData.RingInstances;
                break;
        }
        for (int i = 0; i < equipmentDataInstance.Length -1; i++)
        {
            while (equipmentDataInstance[i].Count >= MergeRequiredCount)
            {
                _playerData.TotalEquipmentMerge++;
                _playerManager.OnAchievementValueChanged?.Invoke(AchievementType.EquipmentMerge);
                equipmentDataInstance[i].Count -= 5;
                if (equipmentDataInstance[i+1].IsUnlocked == false)
                {
                    equipmentDataInstance[i+1].IsUnlocked = true;
                }
                equipmentDataInstance[i+1].Count++;
            }
        }
    }
    public SwordDataInstance GetWeaponInfo(EquipmentRarity equipmentRarity, int starInfo)
    {
        return _playerData.SwordInstances[(int)equipmentRarity * 4 + starInfo -1];
    }
    public RingDataInstance GetRingInfo(EquipmentRarity equipmentRarity, int starInfo)
    {
        return _playerData.RingInstances[(int)equipmentRarity * 4 + starInfo -1];
    }
    public bool CanEquipBetterSword()
    {
        int equippedIndex = _playerData.EquippedSwordSlotNum;

        for (int i = equippedIndex + 1; i < _playerData.SwordInstances.Length; i++)
        {
            if (_playerData.SwordInstances[i].Count > 0)
            {
                return true;
            }
        }
        return false;
    }
}
