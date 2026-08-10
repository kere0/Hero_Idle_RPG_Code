using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Table/SkillSlotUnlockTableSO")]
public class SkillSlotUnlockTableSO : ScriptableObject
{
    public SkillSlotUnlockData[] unlockTable;
    [Serializable]
    public class SkillSlotUnlockData
    {
        public int unlockLevel;
        public int slotNum;
    }
}
