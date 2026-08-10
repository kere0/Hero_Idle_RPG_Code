using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum ItemType
{
    Exp,
    Gold,
    EnhanceStone,
    TreasureChest,
}
public class BaseItem : MonoBehaviour
{
    public ItemType itemType;
    public int value;

    public void OnDisable()
    {
        transform.DOKill();
    }
}
