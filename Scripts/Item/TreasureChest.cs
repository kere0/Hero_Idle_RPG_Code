using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TreasureChest : BaseMonster
{
    [SerializeField] private ParticleSystem expDropParticle;
    [SerializeField] private ParticleSystem goldDropParticle;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public override void Init(int maxHp, Vector3 pos, float treasureChestSize, MonsterType monsterType)
    {
        hpBar.gameObject.SetActive(true);
        transform.position = pos;
        MonsterType = MonsterType.TreasureChest;
        size = 3.5f;
        _attackRange = size / 2 + 2.3f;
        _isDead = false;
        _maxHp = maxHp;
        _currentHp = _maxHp;
        hpBar.SetFillAmount(_currentHp / _maxHp);
    }
    protected override void OnHit()
    {
        _spriteRenderer.color = Color.red;
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
        }
        _resetCoroutine = StartCoroutine(ResetColorCoroutine());
    }
    protected override IEnumerator ResetColorCoroutine()
    {
        yield return ResetColorDelay;
        _spriteRenderer.color = Color.white;
    }
    protected override void DeadEffect()
    {
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
        }
        _resetCoroutine = StartCoroutine(ResetColorCoroutine());
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOVirtual.Float(0f, 1f, 0.7f, t =>
        {
            Color color = Color.Lerp(Color.red, DeadColor, t);
            _spriteRenderer.color = color;
        }));
        sequence.AppendCallback(() => hpBar.gameObject.SetActive(false));
        sequence.Append(DOVirtual.Float(1f, 0f, 0.5f, a =>
        {
            Color color = DeadColor;
            color.a = a;
            _spriteRenderer.color = color;
        }));
        sequence.OnComplete(() =>
        {
            Managers.Pool.ObjPush(gameObject);
        });
    }
    public override void TakeDamage(int damage)
    {
        if (_isDead == true) return;
        OnHit();
        _currentHp -= damage;
        float fillAmount = (float)_currentHp / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        if (_currentHp <= 0)
        {
            DropEffect go = Managers.Resource.Instantiate("ExpDropEffect", pooling: true).GetComponent<DropEffect>();
            go.Init(EffectPos.position);
            go = Managers.Resource.Instantiate("GoldDropEffect", pooling: true).GetComponent<DropEffect>();
            go.Init(EffectPos.position);
            Dead();
            DeadEffect();
        }
    }
}
