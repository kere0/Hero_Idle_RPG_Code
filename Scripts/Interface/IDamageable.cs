using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public CreatureType CreatureType { get; }
    public Collider2D Collider { get; }
    public Vector3 Position { get; }
    public Transform EffectPos { get; }
    public float AttackRange { get; }
    public bool IsDead { get; }
    public float MaxHp  { get;}
    public float CurrentHp  { get; set; }
    public void TakeDamage(int damage);
}
