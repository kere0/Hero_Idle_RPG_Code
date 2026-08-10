using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Table/DungeonBossTableSO")]
public class DungeonBossTableSO : ScriptableObject
{
    public DungeonBossTableData[] DungeonBossData;
    [Serializable]
    public class DungeonBossTableData
    {
        public DungeonType dungeonType;
        public int monsterId;
    }
}
