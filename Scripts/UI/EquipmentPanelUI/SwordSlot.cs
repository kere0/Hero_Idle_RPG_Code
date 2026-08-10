using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlot : BaseEquipmentSlot
{
    public SwordSlotPanelUI swordSlotPanelUI;
    protected override void SlotClick()
    {
        swordSlotPanelUI.EquipmentInfoView(_slotNum);
    }
    public override void MergeRefresh()
    {
        int currentCount = Managers.PlayerManager.playerData.SwordInstances[_slotNum].Count;
        _countText.text = $"{currentCount} / 5";
        int maxCount = Mathf.Min(currentCount, 5);
        _countImage.fillAmount = maxCount / 5f;
        if (Managers.PlayerManager.playerData.SwordInstances[_slotNum].IsUnlocked == false)
        {
            _unlockImage.SetActive(true); 
        }
    }
    public override void EnhanceRefresh()
    {
        _enhanceLevelText.text = $"+{Managers.PlayerManager.playerData.SwordInstances[_slotNum].EnhanceLevel}";
    }
}
