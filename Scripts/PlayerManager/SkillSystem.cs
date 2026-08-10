using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillID
{
    ExplosionStrike, // 폭렬격
    ThunderBall, // 썬더볼
    ManaRecovery, // 마나 회복
    VoidStrike, // 공허격
    ThunderStrike, // 낙뢰
    Acceleration, // 가속
    FightingSpirit, // 투지
    Frenzy, // 광란
    Berserk, // 광폭화
    ThunderCharge // 썬더차지
}
public class SkillSystem
{
    public const int EquipSkillSlotCount = 6;
    private PlayerManager _playerManager;
    private PlayerData _playerData;
    public Action<int> OnQuickSlotUnlock;
    public SkillSlotUnlockTableSO.SkillSlotUnlockData[] skillSlotUnlockData;
    
    public SkillSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;
    }
    public void InitData()
    {
        _playerManager.OnLevelUp += SkillUnlockCheck;
        _playerManager.OnLevelUp += SkillQuickSlotUnlockCheck;
        _playerData.EquippedSkillInstances = new SkillInstance[EquipSkillSlotCount];
        _playerData.SkillPoint = 0;
        int count = Enum.GetValues(typeof(SkillID)).Length;
        _playerData.SkillInstances = new SkillInstance[count];
        skillSlotUnlockData = Managers.Resource.Load<SkillSlotUnlockTableSO>("SkillSlotUnlockTableSO").unlockTable;
        SkillTableSO skillTableSo = Managers.Resource.Load<SkillTableSO>("SkillTableSO");
        // 스킬 데이터 세팅 및 생성
        foreach (SkillTableSO.SkillInfo skillInfo in skillTableSo.skills)
        {
            if (string.IsNullOrEmpty(skillInfo.animationName))
            {
                skillInfo.animationHash = -1;
            }
            else
            {
                skillInfo.animationHash = Animator.StringToHash(skillInfo.animationName);
            }
            SkillInstance skillInstance = SkillFactory.Create(skillInfo);
            _playerData.SkillInstances[(int)skillInfo.id] = skillInstance;
        }
    }
    public void Reset()
    {
        foreach (SkillInstance skillInstance in _playerData.EquippedSkillInstances)
        {
            if (skillInstance == null) continue;
            skillInstance.Reset();
            skillInstance.skill.Reset();
        }
    }
    public void UpgradeSkill(int num)
    {
        _playerData.SkillPoint--;
        _playerData.SkillInstances[num].SkillLevelUp();
        _playerManager.OnSkillLevelChanged.Invoke();
    }
    private void SkillUnlockCheck()
    {
        bool allUnlocked = true;
        foreach (SkillInstance skillInstance in _playerData.SkillInstances)
        {
            if (skillInstance.skill.skillInfo.unlockLevel <= _playerData.Level)
            {
                skillInstance.isUnlocked = true;
            }
            if (skillInstance.isUnlocked == false)
            {
                allUnlocked = false;
            }
        }
        if (allUnlocked == true)
        {
            _playerManager.OnLevelUp -= SkillUnlockCheck;
        }
    }
    private void SkillQuickSlotUnlockCheck()
    {
        for (int i = 0; i < skillSlotUnlockData.Length; i++)
        {
            if (_playerData.Level == skillSlotUnlockData[i].unlockLevel)
            {
                OnQuickSlotUnlock?.Invoke(skillSlotUnlockData[i].slotNum);
            }
        }

        if (skillSlotUnlockData[skillSlotUnlockData.Length - 1].unlockLevel == _playerData.Level)
        {
            _playerManager.OnLevelUp -= SkillQuickSlotUnlockCheck;
        }
    }
    // 스킬 퀵슬롯
    public void EquipSkill(SkillInstance skill, int slotNum)
    {
        _playerData.EquippedSkillInstances[slotNum] = skill;
    }
}
