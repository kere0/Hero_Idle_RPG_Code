using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelUI : MonoBehaviour
{
    [SerializeField] private SkillSlot[] _skillSlots = new SkillSlot[9];
    [SerializeField] private GameObject _skillEquipmentPanelUI;
    [SerializeField] private GameObject _skillEquipmentUI;
    [SerializeField] private Button _equipmentButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _backgroundButton;
    [SerializeField] private TextMeshProUGUI _skillPoint;
    [SerializeField] private GameObject _skillTitleOutline;
    [SerializeField] private TextMeshProUGUI _skillTitleText;
    
    private int _currentSkillIndex = -1;
    private bool _isSelectable = false;
    private void Awake()
    {
        _equipmentButton.onClick.AddListener(SkillEquipmentButtonClick);
        _closeButton.onClick.AddListener(CloseButtonClick);
        _backgroundButton.onClick.AddListener(CloseButtonClick);
        foreach (var slot in _skillSlots)
        {
            slot.skillPanelUI = this;
        }
        ActiveCheck();
    }
    private void OnEnable()
    {
        Init();
        SkillPointRefresh();
        ActiveCheck();
    }
    private void Start()
    {
        Managers.PlayerManager.OnSkillLevelChanged += SkillPointRefresh;
        Managers.PlayerManager.OnLevelUp += SkillPointRefresh;
        _skillTitleText.color = Color.white;
        _skillTitleOutline.gameObject.SetActive(true);
    }
    private void Init()
    {
        SkillInstance[] skillInstances = Managers.PlayerManager.playerData.SkillInstances;
        for (int i = 0; i < _skillSlots.Length; i++)
        {
            _skillSlots[i].InitInfo(skillInstances[i]); 
        }
    }
    private void SkillPointRefresh()
    {
        _skillPoint.text = $"스킬 포인트 : {Managers.PlayerManager.playerData.SkillPoint}";

    }
    // 장착창 열기
    public void OpenSkillEquipmentUI(int id, Vector3 pos)
    {
        _skillEquipmentUI.transform.position = pos;
        _skillEquipmentPanelUI.gameObject.SetActive(true);
        _currentSkillIndex = id;
    }
    // 스킬 장착버튼 클릭
    private void SkillEquipmentButtonClick()
    {
        _isSelectable = true;
        _skillEquipmentPanelUI.gameObject.SetActive(false);
        // 퀵슬로 강조 UI
    }
    // 퀵슬롯 선택
    public bool QuickSlotClick(int slotNum) // 퀵슬롯 쪽에서
    {
        if (_isSelectable == false) return false;
        SkillInstance skillInstance = Managers.PlayerManager.playerData.SkillInstances[_currentSkillIndex];
       Managers.PlayerManager.SkillSystem.EquipSkill(skillInstance, slotNum);
       _isSelectable = false;
       _currentSkillIndex = -1;
       return true;
    }
    private void CloseButtonClick()
    {
        _isSelectable = false;
        _currentSkillIndex = -1;
        _skillEquipmentPanelUI.gameObject.SetActive(false);
    }
    public void ActiveCheck()
    {
        if (Managers.PlayerManager.playerData.SkillPoint == 0)
        {
            foreach (var slot in _skillSlots)
            {
                slot.upgradeButton.interactable = false;
            }
        }
        else
        {
            foreach (var slot in _skillSlots)
            {
                slot.upgradeButton.interactable = slot.isUnlocked;
            }
        }
    }
}
