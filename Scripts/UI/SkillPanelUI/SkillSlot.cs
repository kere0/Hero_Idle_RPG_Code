using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    private int _slotNum = -1;
    public SkillPanelUI skillPanelUI;
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private TextMeshProUGUI _useInfo;
    [SerializeField] private Image _skillImage;
    [SerializeField] private Image _lockImage;
  
    [SerializeField] private TextMeshProUGUI _skillLevelText;
    
    [SerializeField] private Button _slotButton;
    public Button upgradeButton;
    private SkillInstance _skillInstance;
    public bool isUnlocked = false;
    
    private void Awake()
    {
        _slotButton.onClick.AddListener(SlotClick);
        upgradeButton.onClick.AddListener(UpgradeButtonClick);
        Managers.PlayerManager.OnSkillLevelChanged += RefreshUI;
        Managers.PlayerManager.OnLevelUp += RefreshUI;
        Managers.PlayerManager.OnLevelUp += UnLockSkill;
        //upgradeButton.interactable = false;
        _slotButton.interactable = false;
    }
    private void SlotClick()
    {
        skillPanelUI.OpenSkillEquipmentUI(_slotNum, _skillImage.transform.position);
    }
    private void UpgradeButtonClick()
    {
        Managers.PlayerManager.SkillSystem.UpgradeSkill(_slotNum);
        Canvas_Menu.Instance.PlayUpgradeEffect(_skillImage.transform.position);
        if(_skillInstance.Level != 0)
        {
            _slotButton.interactable = true;
        }
    }
    public void InitInfo(SkillInstance skillInstance)
    {
        _skillInstance = skillInstance;
        SkillTableSO.SkillInfo skillInfo = _skillInstance.skill.skillInfo;
        _slotNum = (int)skillInfo.id;
        _skillName.text = skillInfo.skillName;
        string description = skillInfo.description.Replace("{value}", _skillInstance.Value.ToString());
        _skillDescription.text = description;
        _useInfo.text = $"마나 {skillInfo.mana}\n쿨다운 {skillInfo.cooldown}";
        _skillImage.sprite = skillInfo.sprite;
        if (_skillInstance.isUnlocked == true)
        {
            isUnlocked = true;
            upgradeButton.interactable = true;
            _lockImage.gameObject.SetActive(false);
            RefreshUI();
        }
        else
        {
            isUnlocked = false;
            upgradeButton.interactable = false;
            _lockImage.gameObject.SetActive(true);
            _skillLevelText.text = $"해금 레벨 : LV.{skillInfo.unlockLevel}";
        }
    }
    private void UnLockSkill()
    {
        if (_skillInstance.isUnlocked == true)
        {
            isUnlocked = true;
            _lockImage.gameObject.SetActive(false);
            RefreshUI();
            Managers.PlayerManager.OnLevelUp -= UnLockSkill;
        }
    }
    private void RefreshUI()
    {
        if (isUnlocked == true)
        {
            int level = Managers.PlayerManager.playerData.SkillInstances[_slotNum].Level;
            string description = _skillInstance.skill.skillInfo.description.Replace("{value}", _skillInstance.Value.ToString());
            _skillDescription.text = description;
            _skillLevelText.text = $"LV.{level}";
        }
        // 데미지, 마나, 쿨다운
        skillPanelUI.ActiveCheck();
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnSkillLevelChanged -= RefreshUI;
    }
}
