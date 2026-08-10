using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DungeonBoss : BaseMonster
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float _attackCooldown;
    private float _knockBackAttackCooldown;
    // 설정값
    private float _attackIntervalMax => 3f;
    private float _knockBackIntervalMax => 8f;
    private PlayerController _player;
    private bool _isAttacking = false;
    private bool _isAttacked = false;
    // 강화 공격
    [SerializeField] private Bar _knockBackGauge;
    public override void Init(int maxHp, Vector3 pos, float monsterSize, MonsterType monsterType)
    {
        _spriteRenderer.color = Color.white;
        animator.speed = 1;
        hpBar.gameObject.SetActive(true);
        _isDead = false;
        transform.position = pos;
        MonsterType = monsterType;
        size = monsterSize;
        _attackRange = size / 2 +  2.5f;
        _maxHp = maxHp;
        _currentHp = _maxHp;
        float fillAmount = Mathf.Max(0, _currentHp) / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        GameContainer.Instance.HUD.RefreshBossHpProgressBar(fillAmount);

        _player = GameContainer.Instance.Player;
        _attackCooldown = 0;
        _knockBackAttackCooldown = _knockBackIntervalMax;
        float rate = Mathf.Min(_knockBackIntervalMax - _knockBackAttackCooldown, _knockBackIntervalMax);
        _knockBackGauge.SetFillAmount(rate / _knockBackIntervalMax);
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
    private void Update()
    {
        if(_attackCooldown > 0) _attackCooldown -= Time.deltaTime;
        if(_knockBackAttackCooldown > 0) _knockBackAttackCooldown -= Time.deltaTime;
        float rate = Mathf.Min(_knockBackIntervalMax - _knockBackAttackCooldown, _knockBackIntervalMax);
        _knockBackGauge.SetFillAmount(rate / _knockBackIntervalMax);
        Attack();
    }
    private void Attack()
    {
        if(_isDead == true) return;
        if (_player == null) return;
        if(_player.IsDead == true) return;
        bool distanceCheck = Mathf.Abs(_player.transform.position.x - transform.position.x) <= _attackRange;
        if (distanceCheck == true && _isAttacking == false)
        {
            if(_knockBackAttackCooldown <= 0)
            {
                _isAttacking = true;
                animator.Play(KBATTACK, 0, 0f);
                _knockBackAttackCooldown = _knockBackIntervalMax;
                _attackCooldown = _attackIntervalMax;
            }
            else if (_attackCooldown <= 0)
            {
                _isAttacking = true;
                animator.Play(ATTACK, 0, 0f);
                _attackCooldown = _attackIntervalMax;
            }
        }
        if (_isAttacking == true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash != ATTACK && stateInfo.shortNameHash != KBATTACK) return;
            if (stateInfo.shortNameHash == ATTACK)
            {
                CheckAttackTiming(stateInfo, 0.59f, attack);
            }
            else if (stateInfo.shortNameHash == KBATTACK)
            {
                CheckAttackTiming(stateInfo, 0.65f, attack * 3, () => _player.KnockBack(20f,1.3f));
            }
            if (stateInfo.normalizedTime >= 1)
            {
                _isAttacking = false;
                _isAttacked = false;
                animator.Play(IDLE, 0, 0f);
            }
        }
    }
    private void CheckAttackTiming(AnimatorStateInfo state, float attackTime, int damage, Action action = null)
    {
        if (state.normalizedTime >= attackTime && _isAttacked == false)
        {
            _isAttacked = true;
            if (state.shortNameHash == KBATTACK)
            {
                SoundManager.Instance.PlaySFX("KnockBackAttackSFX");
                GameObject go = Managers.Resource.Instantiate("KnockBackImpact", pooling: true);
                GameContainer.Instance.CameraShakeManager.CameraShake(0.5f, 0.5f);
                go.transform.position = _player.EffectPos.position;
            }
            action?.Invoke();
            CombatEvent combatEvent = new CombatEvent()
            {
                Receiver = _player,
                Damage = damage,
                DamageType = DamageType.Normal
            };
            GameContainer.Instance.CombatSystem.AddCombatEvent(combatEvent);
        }
    }
    protected override void DeadEffect()
    {
        animator.Play(IDLE, 0, 0f);
        DropEffect dropEffect = new DropEffect();
        switch (MonsterType)
        {
            case MonsterType.GoldDungeon:
                dropEffect = Managers.Resource.Instantiate("GoldDropEffect", pooling: true).GetComponent<DropEffect>();
                break;
            case MonsterType.ExpDungeon:
                dropEffect = Managers.Resource.Instantiate("ExpDropEffect", pooling: true).GetComponent<DropEffect>();
                break;
            case MonsterType.EnhanceStoneDungeon:
                dropEffect = Managers.Resource.Instantiate("EnhanceStoneDropEffect", pooling: true).GetComponent<DropEffect>();
                break;
        }
        dropEffect.Init(EffectPos.transform.position);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOVirtual.Float(0f, 1f, 0.7f, t =>
        {
            Color color = Color.Lerp(Color.red, DeadColor, t);
            _spriteRenderer.color = color;
            animator.speed = 0;
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
        _currentHp -= damage;
        if (_currentHp > 0)
        {
            OnHit();
            if (_isAttacking == false)
            {
                animator.Play(HIT, 0, 0);    
            }
        }
        else
        {
            _currentHp = 0;
            
            Dead();
            DeadEffect();
        }
        float fillAmount = Mathf.Max(0, _currentHp) / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        GameContainer.Instance.HUD.RefreshBossHpProgressBar(fillAmount);
    }
}
