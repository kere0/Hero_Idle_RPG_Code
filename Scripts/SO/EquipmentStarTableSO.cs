using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Table/EquipmentStarGradeTable")]

public class EquipmentStarTableSO : ScriptableObject
{
    public StarProbability[] starProbabilities;
    [Serializable]
    public class StarProbability
    {
        public int starGrade;
        public float probability;
    }
}
