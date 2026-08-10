using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum MonsterType
{
    Normal,
    StageBoss,
    TreasureChest,
    GoldDungeon,
    ExpDungeon,
    EnhanceStoneDungeon
}
public class BaseMonster : BaseCreature
{
    public float size;
    public int attack;
    public MonsterType MonsterType { get; protected set; }
    public virtual void Init(int maxHp, Vector3 pos, float monsterSize, MonsterType monsterType){ }

    public void Start()
    {
        _creatureType = CreatureType.Monster;
    }
    public override void TakeDamage(int damage)
    {
        if (_isDead == true) return;
        OnHit();
        _currentHp -= damage;
        float fillAmount = _currentHp / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        // Effect
        if (_currentHp > 0)
        {
            animator.Play(HIT, 0, 0);
        }
        else
        {
            Dead();
            DeadEffect();
        }
    }

    public void SetAttack(int attack)
    {
        this.attack = attack;
    }
    public override void Dead()
    {
        _isDead = true;
        _currentHp = 0;
        GameContainer.Instance.BattleManager.OnMonsterDead.Invoke(this);
    }
    private void OnDisable()
    {
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }
    }
}
