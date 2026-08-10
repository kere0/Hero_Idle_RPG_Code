using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : BaseMonster
{
    public override void Init(int maxHp, Vector3 pos, float monsterSize, MonsterType monsterType)
    {
        animator.speed = 1;
        hpBar.gameObject.SetActive(true);
        transform.position = pos;
        _meshRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(ColorID, Color.white);
        _meshRenderer.SetPropertyBlock(_mpb);
        MonsterType = monsterType;
        size = monsterSize;
        _attackRange = size / 2 + 2.5f;
        _isDead = false;
        _maxHp = maxHp;
        _currentHp = _maxHp;
        hpBar.SetFillAmount(CurrentHp / MaxHp);
    }
}
