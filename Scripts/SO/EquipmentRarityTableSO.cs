using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Table/EquipmentRarityTable")]
public class EquipmentRarityTableSO : ScriptableObject
{
    public RarityProbability[] rarityProbabilities;
    [Serializable]
    public class RarityProbability
    {
        public EquipmentRarity rarity;
        public float probability;
    }
}