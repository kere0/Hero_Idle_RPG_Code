using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EquipmentInfoViewUI : MonoBehaviour
{
    [SerializeField] private EquipmentViewInfoSlot _equipmentViewInfoSlot;
    [SerializeField] private GameObject _equipmentImage;
    [SerializeField] private TextMeshProUGUI _rarityText;
    [SerializeField] private Button _enhanceButton;
    [SerializeField] private Button _mergeButton;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _equipmentInfoEffectText;
    [SerializeField] private TextMeshProUGUI _applyOwnedEffectText;
    [SerializeField] private TextMeshProUGUI _enhanceCostText;
    [SerializeField] private TextMeshProUGUI _equipText;
    [SerializeField] private TextMeshProUGUI _ownedEnhanceStoneText;

    [SerializeField] private TextMeshProUGUI _equipmentEffectTitleText;
    [SerializeField] private TextMeshProUGUI _onwedEffectTitleText;
    
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    private BaseEquipmentDataInstance _CurrentEquipmentDataInstance;
    //private BaseEq
    public EquipmentType equipmentType;
    private void Awake()
    {
        _enhanceButton.onClick.AddListener(EnhanceButtonClick);
        _closeButton.onClick.AddListener(CloseButtonClick);
        _mergeButton.onClick.AddListener(MergeButtonClick);
        _equipButton.onClick.AddListener(EquipButtonClick);
        _leftButton.onClick.AddListener(LeftButtonClick);
        _rightButton.onClick.AddListener(RightButtonClick);
    }
    private void Start()
    {
        Managers.PlayerManager.OnEnhanceStoneChanged += RefreshEnhanceStoneText;
    }
    private void CloseButtonClick()
    {
        gameObject.SetActive(false);
    }
    private void LeftButtonClick()
    {
        if (equipmentType == EquipmentType.Sword)
        {
            int leftEquipmentId = _CurrentEquipmentDataInstance.EquipmentData.ItemId - 1;
            _CurrentEquipmentDataInstance = Managers.PlayerManager.playerData.SwordInstances[leftEquipmentId];
            EquipmentInfoInit(_CurrentEquipmentDataInstance);
        }
        else if (equipmentType == EquipmentType.Ring)
        {
            int leftEquipmentId = _CurrentEquipmentDataInstance.EquipmentData.ItemId - 1;
            _CurrentEquipmentDataInstance = Managers.PlayerManager.playerData.RingInstances[leftEquipmentId];
            EquipmentInfoInit(_CurrentEquipmentDataInstance);
        }
    }
    private void RightButtonClick()
    {
        if (equipmentType == EquipmentType.Sword)
        {
            int rightEquipmentId = _CurrentEquipmentDataInstance.EquipmentData.ItemId + 1;
            _CurrentEquipmentDataInstance = Managers.PlayerManager.playerData.SwordInstances[rightEquipmentId];
            EquipmentInfoInit(_CurrentEquipmentDataInstance);
        }
        else if (equipmentType == EquipmentType.Ring)
        {
            int rightEquipmentId = _CurrentEquipmentDataInstance.EquipmentData.ItemId + 1;
            _CurrentEquipmentDataInstance = Managers.PlayerManager.playerData.RingInstances[rightEquipmentId];
            EquipmentInfoInit(_CurrentEquipmentDataInstance);
        }
        
    }
    private void LeftRightCheck()
    {
        if (_CurrentEquipmentDataInstance.EquipmentData.ItemId == 0)
        {
            _leftButton.gameObject.SetActive(false);
        }
        else
        {
            _leftButton.gameObject.SetActive(true);
        }
        if (_CurrentEquipmentDataInstance.EquipmentData.ItemId != BaseEquipmentDataInstance.MaxCount - 1)
        {
            _rightButton.gameObject.SetActive(true);
        }
        else
        {
            _rightButton.gameObject.SetActive(false);
        }
    }
    // 장착 버튼 클릭
    private void EquipButtonClick()
    {
        Managers.PlayerManager.EquipmentSystem.EqipEquipment(_CurrentEquipmentDataInstance.EquipmentData.EquipmentType, _CurrentEquipmentDataInstance.EquipmentData.ItemId);
        EquipmentInfoInit(_CurrentEquipmentDataInstance);
        if (equipmentType == EquipmentType.Sword)
        {
            Managers.PlayerManager.OnSwordChanged?.Invoke();
        }
        else if (equipmentType == EquipmentType.Ring)
        {
            Managers.PlayerManager.OnRingChanged?.Invoke();
        }
    }
    // 강화 버튼 클릭
    private void EnhanceButtonClick()
    {
        if (_CurrentEquipmentDataInstance.IsUnlocked == false) return;
        if (Managers.PlayerManager.EquipmentSystem.GetEnhanceStoneCost(_CurrentEquipmentDataInstance.EnhanceLevel) <= Managers.PlayerManager.playerData.EnhanceStone)
        {
            Managers.PlayerManager.EquipmentSystem.UpgradeEquipment(equipmentType, _CurrentEquipmentDataInstance.EquipmentData.ItemId);
            EquipmentInfoInit(_CurrentEquipmentDataInstance);
        }
        if (equipmentType == EquipmentType.Sword)
        {
            Managers.PlayerManager.OnSwordEnhanceComplete?.Invoke(_CurrentEquipmentDataInstance.EquipmentData.ItemId);
        }
        else if (equipmentType == EquipmentType.Ring)
        {
            Managers.PlayerManager.OnRingEnhanceComplete?.Invoke(_CurrentEquipmentDataInstance.EquipmentData.ItemId);
        }
        Canvas_Menu.Instance.PlayUpgradeEffect(_equipmentImage.transform.position);
    }
    private void MergeButtonClick()
    {
        int nextValue = _CurrentEquipmentDataInstance.EquipmentData.ItemId + 1;
        BaseEquipmentDataInstance[] equipmentDataInstances = new BaseEquipmentDataInstance[BaseEquipmentDataInstance.MaxCount];
        if (equipmentType == EquipmentType.Sword)
        {
            equipmentDataInstances = Managers.PlayerManager.playerData.SwordInstances;
        }
        else if (equipmentType == EquipmentType.Ring)
        {
            equipmentDataInstances = Managers.PlayerManager.playerData.RingInstances;
        }
        // 수치 증가
        Managers.PlayerManager.EquipmentSystem.MergeEquipment(_CurrentEquipmentDataInstance, nextValue);
        if (equipmentDataInstances[nextValue].IsUnlocked == false)
        {
            equipmentDataInstances[nextValue].IsUnlocked = true;
            // 장착 버튼 활성화
            _equipButton.interactable = true;
            // 강화 버튼
            bool isEnhanceable = (Managers.PlayerManager.playerData.EnhanceStone >= Managers.PlayerManager.EquipmentSystem.GetEnhanceStoneCost(_CurrentEquipmentDataInstance.EnhanceLevel));
            // 강화 버튼
            _enhanceButton.interactable = isEnhanceable;
        }
        // 개수가 5고 마지막 장비가 아니라면
        bool result = (_CurrentEquipmentDataInstance.Count >= 5 && _CurrentEquipmentDataInstance.EquipmentData.ItemId != Managers.PlayerManager.playerData.SwordInstances.Length - 1);
        _mergeButton.interactable = result;
        if (equipmentType == EquipmentType.Sword)
        {
            Managers.PlayerManager.OnSwordMergeComplete?.Invoke(_CurrentEquipmentDataInstance.EquipmentData.ItemId);
        }
        else if (equipmentType == EquipmentType.Ring)
        {
            Managers.PlayerManager.OnRingMergeComplete?.Invoke(_CurrentEquipmentDataInstance.EquipmentData.ItemId);
        }
        EquipmentInfoInit(_CurrentEquipmentDataInstance);
    }

    private void RefreshEnhanceStoneText()
    {
        _ownedEnhanceStoneText.text = Managers.PlayerManager.playerData.EnhanceStone.ToString();
    }
    public void EquipmentInfoInit(BaseEquipmentDataInstance equipmentDataInstance)
    {
        _CurrentEquipmentDataInstance = equipmentDataInstance;
        // 강화 버튼
        bool isEnhanceable = ((Managers.PlayerManager.playerData.EnhanceStone >= Managers.PlayerManager.EquipmentSystem.GetEnhanceStoneCost(_CurrentEquipmentDataInstance.EnhanceLevel)) && _CurrentEquipmentDataInstance.IsUnlocked == true);
        _enhanceButton.interactable = isEnhanceable;
        // 장착버튼
        _equipButton.interactable = _CurrentEquipmentDataInstance.IsUnlocked;
        
        // 개수가 5고 마지막 장비가 아니라면
        bool result = (_CurrentEquipmentDataInstance.Count >= 5 && _CurrentEquipmentDataInstance.EquipmentData.ItemId != Managers.PlayerManager.playerData.SwordInstances.Length - 1);
        _mergeButton.interactable = result;
        if (_CurrentEquipmentDataInstance is SwordDataInstance)
        {
            equipmentType = EquipmentType.Sword;
            _equipmentInfoEffectText.text = $"+{Managers.PlayerManager.GetSwordAttackIncreasePercent(_CurrentEquipmentDataInstance.EquipmentData.ItemId):0.#}%";
            _applyOwnedEffectText.text = $"+{Managers.PlayerManager.GetSwordAttackIncreasePercent(_CurrentEquipmentDataInstance.EquipmentData.ItemId) / 100:0.#}%";
            if (_CurrentEquipmentDataInstance.EquipmentData.ItemId == Managers.PlayerManager.playerData.EquippedSwordSlotNum)
            {
                _equipText.text = "장착중";
                _equipButton.interactable = false;
            }
            else
            {
                _equipText.text = "장착";
            }
        }
        else if (_CurrentEquipmentDataInstance is RingDataInstance)
        {
            equipmentType = EquipmentType.Ring;
            _equipmentInfoEffectText.text = $"+{Managers.PlayerManager.GetRingManaRegenPercent(_CurrentEquipmentDataInstance.EquipmentData.ItemId):0.#}%";
            _applyOwnedEffectText.text = $"+{Managers.PlayerManager.GetRingManaRegenPercent(_CurrentEquipmentDataInstance.EquipmentData.ItemId) / 100:0.#}%";
            if (_CurrentEquipmentDataInstance.EquipmentData.ItemId == Managers.PlayerManager.playerData.EquippedRingSlotNum)
            {
                _equipText.text = "장착중";
                _equipButton.interactable = false;
            }
            else
            {
                _equipText.text = "장착";
            }
        }

        if (equipmentType == EquipmentType.Sword)
        {
            _equipmentEffectTitleText.text = "공격력 증가";
            _onwedEffectTitleText.text = "공격력 증가";
        }
        else if (equipmentType == EquipmentType.Ring)
        {
            _equipmentEffectTitleText.text = "마나회복량 증가";
            _onwedEffectTitleText.text = "마나회복량 증가";
        }
        _enhanceCostText.text = Managers.PlayerManager.EquipmentSystem.GetEnhanceStoneCost(_CurrentEquipmentDataInstance.EnhanceLevel).ToString();
        _ownedEnhanceStoneText.text = Managers.PlayerManager.playerData.EnhanceStone.ToString();
        _equipmentViewInfoSlot.InitInfo(_CurrentEquipmentDataInstance.EquipmentData.ItemId,
            _CurrentEquipmentDataInstance.EquipmentData.StarGrade, 
            _CurrentEquipmentDataInstance.EnhanceLevel, 
            _CurrentEquipmentDataInstance.Count, 
            _CurrentEquipmentDataInstance.IsUnlocked,
            equipmentType);
        LeftRightCheck();
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnEnhanceStoneChanged -= RefreshEnhanceStoneText;
    }
}
