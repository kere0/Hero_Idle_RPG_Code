using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSkillSlot : MonoBehaviour
{
    public EquipSkillSlotPanelUI equipSkillSlotPanelUI;
    [SerializeField] private Image _skillImage;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private Button button;
    [SerializeField] private Image _defaultImage;
    [SerializeField] private Image _lockImage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    public int slotNum;
    public bool isUnlocked = false;
    public SkillInstance equipSkillInstance;
    private void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(SlotClick);
        //button.interactable = false;
    }
    private void SlotClick()
    {
        //if (isUnlocked == false) return;
        equipSkillSlotPanelUI.EquipSkillSlotClick(slotNum);
    }
    public void SlotUnlocked()
    {
        isUnlocked = true;
        _lockImage.gameObject.SetActive(false);
        _cooldownImage.gameObject.SetActive(true);
        button.interactable = true;
    }
    public void SetSlot(SkillInstance skillInstance)
    {
        if (skillInstance == null)
        {
            equipSkillInstance = null;
            _skillImage.gameObject.SetActive(false);
            _defaultImage.gameObject.SetActive(true);
            _cooldownImage.fillAmount = 0;
            cooldownText.gameObject.SetActive(false);
            Managers.PlayerManager.playerData.EquippedSkillInstances[slotNum] = null;
            return;
        }
        if (equipSkillInstance != null)
        {
            if (equipSkillInstance.skill.skillInfo.skillCategory == SKillCategory.Passive)
            {
                equipSkillInstance.Reset();
                if (equipSkillInstance.skill.skillInfo.id == SkillID.Acceleration)
                {
                    Managers.PlayerManager.BuffSystem.ResetBuff(BuffType.AttackSpeedBuff);
                }
                else if (equipSkillInstance.skill.skillInfo.id == SkillID.FightingSpirit)
                {
                    Managers.PlayerManager.BuffSystem.ResetBuff(BuffType.AttackBuff);
                }
                GameContainer.Instance.BuffPanelUI.BuffSlotReset(equipSkillInstance.skill.skillInfo);
            }
        }
        equipSkillInstance = skillInstance;
        _skillImage.gameObject.SetActive(true);
        _defaultImage.gameObject.SetActive(false);
        _skillImage.sprite = equipSkillInstance.skill.skillInfo.sprite;
    }
    private void Update()
    {
        if (equipSkillInstance == null) return;
        if (equipSkillInstance.cooldown > 0)
        {
            if (cooldownText.gameObject.activeSelf == false)
            {
                cooldownText.gameObject.SetActive(true);
            }
            float cooldown = equipSkillInstance.cooldown / equipSkillInstance.skill.skillInfo.cooldown;
            _cooldownImage.fillAmount = cooldown;
            cooldownText.text = Mathf.CeilToInt(equipSkillInstance.cooldown).ToString();
        }
        else
        {
            if (cooldownText.gameObject.activeSelf == true)
            {
                float cooldown = equipSkillInstance.cooldown / equipSkillInstance.skill.skillInfo.cooldown;
                _cooldownImage.fillAmount = cooldown;
                cooldownText.gameObject.SetActive(false);
            }
        }
    }
}
