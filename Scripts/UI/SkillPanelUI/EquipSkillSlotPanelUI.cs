using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSkillSlotPanelUI : MonoBehaviour
{
    [SerializeField] private SkillPanelUI skillPanelUI;
    [SerializeField] private EquipSkillSlot[] _equipSkillSlots = new EquipSkillSlot[6];
    [SerializeField] private UnlockLevelViewPanel _unlockLevelViewPanel; 
    private void Awake()
    {
        for (int i = 0; i < _equipSkillSlots.Length; i++)
        {
            _equipSkillSlots[i].equipSkillSlotPanelUI = this;
            _equipSkillSlots[i].slotNum = i;
        }
    }
    private void Start()
    {
        Managers.PlayerManager.SkillSystem.OnQuickSlotUnlock += UnlockQuickSlot;
    }
    private void UnlockQuickSlot(int slotNum)
    {
        if (_equipSkillSlots[slotNum].isUnlocked == false)
        {
            _equipSkillSlots[slotNum].SlotUnlocked();
        }
    }
    public void EquipSkillSlotClick(int slotNum)
    {
        bool result = skillPanelUI.QuickSlotClick(slotNum);
        SkillInstance equipSkillInstance = Managers.PlayerManager.playerData.EquippedSkillInstances[slotNum];
        // 장착할 스킬이 없으면
        if (result == false)
        {
            if (_equipSkillSlots[slotNum].isUnlocked == false)
            {
                string unlockLevelText = Managers.PlayerManager.SkillSystem.skillSlotUnlockData[slotNum].unlockLevel.ToString();
                _unlockLevelViewPanel.SetUnlockLevelText(unlockLevelText);
                _unlockLevelViewPanel.gameObject.SetActive(true);
                return;
            }
            if (equipSkillInstance != null)
            {
                if (equipSkillInstance.skill.skillInfo.skillCategory == SKillCategory.Passive) return;
                GameContainer.Instance.Player.UseSkill(slotNum);
            }
        }
        else if (result == true)
        {
            Debug.Log(equipSkillInstance.skill.skillInfo.skillName);
            foreach (EquipSkillSlot equipSkillSlot in _equipSkillSlots)
            {
                if (equipSkillSlot.isUnlocked == false || equipSkillSlot.equipSkillInstance == null) continue;
                if (equipSkillSlot.equipSkillInstance == equipSkillInstance)
                {
                    Debug.Log("교체");
                    Debug.Log(equipSkillSlot.equipSkillInstance.skill.skillInfo.skillName);
                    equipSkillSlot.SetSlot(null);
                }
            }
            _equipSkillSlots[slotNum].SetSlot(equipSkillInstance);
        }
    }
}
