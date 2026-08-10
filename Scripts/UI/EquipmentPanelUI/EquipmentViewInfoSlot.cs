using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentViewInfoSlot : BaseEquipmentSlot
{
    [SerializeField] private Image _equipmentImage;
    [SerializeField] private Sprite _swordIcon;
    [SerializeField] private Sprite _ringIcon;
    protected override void Awake() { }

    public override void InitInfo(int slotNum, int starGradeText, int enhanceLevelText, int count, bool isUnlock, EquipmentType equipmentType)
    {
        base.InitInfo(slotNum, starGradeText, enhanceLevelText, count, isUnlock, equipmentType);
        if (equipmentType == EquipmentType.Sword)
        {
            _equipmentImage.sprite = _swordIcon;
        }
        else
        {
            _equipmentImage.sprite = _ringIcon;
        }
    }
}
