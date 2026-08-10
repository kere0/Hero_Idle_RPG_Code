using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType
{
    GoldPotion,
    ExpPotion,
    AttackPotion
}
public class AdBuffSystem
{
    public float GoldAdBuffMultiplier { get; private set; } = 1;
    public float ExpAdBuffMultiplier { get; private set; } = 1;
    public float AttackAdBuffMultiplier { get; private set; } = 1;
    public void ApplyAdBuff(PotionType potionType, float time, float rateValue)
    {
        GameContainer.Instance.ViewPotionBuffPanel.ApplyAdBuff(potionType, time);
        switch (potionType)
        {
            case PotionType.GoldPotion:
                GoldAdBuffMultiplier = rateValue / 100f;
                break;
            case PotionType.ExpPotion:
                ExpAdBuffMultiplier = rateValue / 100f;
                break;
            case PotionType.AttackPotion:
                AttackAdBuffMultiplier = rateValue / 100f;
                break;
        }
    }
    public void EndAdBuff(PotionType potionType)
    {
        switch (potionType)
        {
            case PotionType.GoldPotion:
                GoldAdBuffMultiplier = 1f;
                break;
            case PotionType.ExpPotion:
                ExpAdBuffMultiplier = 1f;
                break;
            case PotionType.AttackPotion:
                AttackAdBuffMultiplier = 1f;
                break;
        }
    }
}
