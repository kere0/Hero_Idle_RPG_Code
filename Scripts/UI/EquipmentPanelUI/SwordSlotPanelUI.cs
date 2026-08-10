using UnityEngine;

public class SwordSlotPanelUI : MonoBehaviour
{
    [SerializeField] private SwordSlot[] _swordSlots = new SwordSlot[16];
    [SerializeField] private EquipmentInfoViewUI _equipmentInfoViewUI;
    private int _currentEquippedSlotIndex = -1;
    private void Awake()
    {
        foreach (var slot in _swordSlots)
        {
            slot.swordSlotPanelUI = this;
        }
    }

    private void Start()
    {
        Managers.PlayerManager.OnSwordEnhanceComplete += EnhanceCompleteRefresh;
        Managers.PlayerManager.OnSwordMergeComplete += MergeCompleteRefresh;
        Managers.PlayerManager.OnSwordChanged += Refresh;
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
            _swordSlots[_currentEquippedSlotIndex].UnequipEquipment();
        }
        int equippedSlot = Managers.PlayerManager.playerData.EquippedSwordSlotNum;
        _swordSlots[equippedSlot].EquipEquipment();
        _currentEquippedSlotIndex = equippedSlot;
    }
    private void Init()
    {
        SwordDataInstance[] equipmentDataInstance = Managers.PlayerManager.playerData.SwordInstances;
        for (int i = 0; i < _swordSlots.Length; i++)
        {
            Debug.Log(equipmentDataInstance[i].EquipmentData.StarGrade);
            _swordSlots[i].InitInfo(equipmentDataInstance[i].EquipmentData.ItemId,
                equipmentDataInstance[i].EquipmentData.StarGrade,
                equipmentDataInstance[i].EnhanceLevel,
                equipmentDataInstance[i].Count,
                equipmentDataInstance[i].IsUnlocked,
                EquipmentType.Sword); 
        }
    }
    
    public void EquipmentInfoView(int id)
    {
        _equipmentInfoViewUI.gameObject.SetActive(true);
        _equipmentInfoViewUI.EquipmentInfoInit(Managers.PlayerManager.playerData.SwordInstances[id]);
    }
    private void EnhanceCompleteRefresh(int slotNum)
    {
        _swordSlots[slotNum].EnhanceRefresh();
    }
    private void MergeCompleteRefresh(int slotNum)
    {
        _swordSlots[slotNum].MergeRefresh();
        _swordSlots[slotNum+1].MergeRefresh();
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnSwordEnhanceComplete -= EnhanceCompleteRefresh;
        Managers.PlayerManager.OnSwordMergeComplete -= MergeCompleteRefresh;
        Managers.PlayerManager.OnSwordChanged -= Refresh;
    }
}
