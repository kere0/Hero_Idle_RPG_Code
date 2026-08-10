using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSlot : BaseEquipmentSlot
{
    public RingSlotPanelUI ringSlotPanelUI;
    protected override void SlotClick()
    {
        ringSlotPanelUI.EquipmentInfoView(_slotNum);
    }
    public override void MergeRefresh()
    {
        int currentCount = Managers.PlayerManager.playerData.RingInstances[_slotNum].Count;
        _countText.text = $"{currentCount} / 5";
        int maxCount = Mathf.Min(currentCount, 5);
        _countImage.fillAmount = maxCount / 5f;
        if (Managers.PlayerManager.playerData.RingInstances[_slotNum].IsUnlocked == false)
        {
            _unlockImage.SetActive(true); 
        }
    }
    public override void EnhanceRefresh()
    {
        _enhanceLevelText.text = $"+{Managers.PlayerManager.playerData.RingInstances[_slotNum].EnhanceLevel}";
    }
}
