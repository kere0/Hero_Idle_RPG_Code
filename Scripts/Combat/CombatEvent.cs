using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    None,
    Normal,
    Skill
}
public struct CombatEvent
{
    public IDamageable Receiver;
    public DamageType DamageType;
    public int Damage;
    public bool IsCritical;
}
