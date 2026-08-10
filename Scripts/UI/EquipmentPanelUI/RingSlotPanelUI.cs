using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSlotPanelUI : MonoBehaviour
{
    [SerializeField] private RingSlot[] _ringSlot = new RingSlot[16];
    [SerializeField] private EquipmentInfoViewUI _equipmentInfoViewUI;
    private int _currentEquippedSlotIndex = -1;
    private void Awake()
    {
        foreach (var slot in _ringSlot)
        {
            slot.ringSlotPanelUI = this;
        }
    }
    private void Start()
    {
        Managers.PlayerManager.OnRingEnhanceComplete += EnhanceCompleteRefresh;
        Managers.PlayerManager.OnRingMergeComplete += MergeCompleteRefresh;
        Managers.PlayerManager.OnRingChanged += Refresh;
    }
    private void OnEnable()
    {
        Refresh();
    }
    public void Refresh()
    {
        Init();
        if (_currentEquippedSlotIndex != -1)
        {
            _ringSlot[_currentEquippedSlotIndex].UnequipEquipment();
        }
        int equippedSlot = Managers.PlayerManager.playerData.EquippedRingSlotNum;
        _ringSlot[equippedSlot].EquipEquipment();
        _currentEquippedSlotIndex = equippedSlot;
    }
    private void Init()
    {
        RingDataInstance[] equipmentDataInstance = Managers.PlayerManager.playerData.RingInstances;
        for (int i = 0; i < _ringSlot.Length; i++)
        {
            _ringSlot[i].InitInfo(equipmentDataInstance[i].EquipmentData.ItemId,
                equipmentDataInstance[i].EquipmentData.StarGrade,
                equipmentDataInstance[i].EnhanceLevel,
                equipmentDataInstance[i].Count,
                equipmentDataInstance[i].IsUnlocked,
                EquipmentType.Ring); 
        }
    }
    public void EquipmentInfoView(int id)
    {
        _equipmentInfoViewUI.gameObject.SetActive(true);
        _equipmentInfoViewUI.EquipmentInfoInit(Managers.PlayerManager.playerData.RingInstances[id]);
    }
    private void EnhanceCompleteRefresh(int slotNum)
    {
        _ringSlot[slotNum].EnhanceRefresh();
    }
    private void MergeCompleteRefresh(int slotNum)
    {
        _ringSlot[slotNum].MergeRefresh();
        _ringSlot[slotNum+1].MergeRefresh();
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnRingEnhanceComplete -= EnhanceCompleteRefresh;
        Managers.PlayerManager.OnRingMergeComplete -= MergeCompleteRefresh;
        Managers.PlayerManager.OnRingChanged -= Refresh;
    }
}
