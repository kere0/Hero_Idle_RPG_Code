using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPanelUI : MonoBehaviour
{
    public List<BuffSlot> buffSlots = new List<BuffSlot>();
    public void BuffSlotCreate(SkillTableSO.SkillInfo skillInstance)
    {
        BuffSlot buffSlot = Managers.Resource.Instantiate("BuffSlot", pooling : true).GetComponent<BuffSlot>();
        buffSlot.transform.SetParent(transform, false);
        buffSlot.transform.localScale = Vector3.one;
        buffSlot.SetInfo(skillInstance, this);
        buffSlots.Add(buffSlot);
    }
    public void BuffSlotsReset()
    {
        for (int i = buffSlots.Count - 1; i >= 0; i--)
        {
            Managers.Pool.ObjPush(buffSlots[i].gameObject);
        }
        buffSlots.Clear();
    }

    public void BuffSlotReset(SkillTableSO.SkillInfo skillInfo)
    {
        for (int i = buffSlots.Count - 1; i >= 0; i--)
        {
            if (skillInfo == buffSlots[i].currentSkillInfo)
            {
                buffSlots[i].Reset();
            }
        }
    }
}
