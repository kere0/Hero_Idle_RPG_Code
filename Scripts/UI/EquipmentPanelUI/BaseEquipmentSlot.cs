using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BaseEquipmentSlot : MonoBehaviour
{
    protected int _slotNum = -1;
    [SerializeField] protected TextMeshProUGUI _starGradeText;
    [SerializeField] protected TextMeshProUGUI _enhanceLevelText;
    [SerializeField] protected TextMeshProUGUI _countText;
    [SerializeField] protected Image _frameImage;
    [SerializeField] protected Image _backgroundImage;
    [SerializeField] protected Image _countImage;
    [SerializeField] protected GameObject _unlockImage;
    [SerializeField] protected GameObject _equippedImage;
    
    [SerializeField] protected Button _slotButton;
    protected virtual void Awake()
    {
        TryGetComponent(out _slotButton);
        _slotButton.onClick.AddListener(SlotClick);
    }
    protected virtual void SlotClick() { }
    public virtual void InitInfo(int slotNum, int starGrade, int enhanceLevel, int count, bool isUnlock, EquipmentType equipmentType)
    {
        _slotNum = slotNum;
        _starGradeText.text = starGrade.ToString();
        if (enhanceLevel != 0)
        {
            _enhanceLevelText.text = $"+{enhanceLevel}";
        }
        else
        {
            _enhanceLevelText.text = "";
        }
        _countText.text = $"{count} / 5";
        int maxCount = Mathf.Min(count, 5);
        _countImage.fillAmount = maxCount / 5f;
        if (_slotNum < 4)
        {
            _frameImage.color = EquipmentGachaManager.NormalFrameColor;
            _backgroundImage.color = EquipmentGachaManager.NormalBackgroundColor;
        }
        else if (_slotNum < 8)
        {
            _frameImage.color = EquipmentGachaManager.RareFrameColor;
            _backgroundImage.color = EquipmentGachaManager.RareBackgroundColor;
        }
        else if (_slotNum < 12)
        {
            _frameImage.color = EquipmentGachaManager.UniqueFrameColor;
            _backgroundImage.color = EquipmentGachaManager.UniqueBackgroundColor;
        }
        else if (_slotNum < 16)
        {
            _frameImage.color = EquipmentGachaManager.LegendFrameColor;
            _backgroundImage.color = EquipmentGachaManager.LegendBackgroundColor;
        }
        _unlockImage.SetActive(!isUnlock); 
        if (_equippedImage != null)
        {
            _equippedImage.SetActive(false);
        }
    }
    public void EquipEquipment()
    {
        if (_equippedImage != null)
        {
            _equippedImage.SetActive(true);
        }
    }
    public void UnequipEquipment()
    {
        if (_equippedImage != null)
        {
            _equippedImage.SetActive(false);
        }
    }
    public virtual void MergeRefresh() { }

    public virtual void EnhanceRefresh() { }

}
