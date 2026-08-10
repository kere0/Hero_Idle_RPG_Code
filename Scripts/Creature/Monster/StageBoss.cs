using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBoss : BaseMonster
{
    protected float _attackCooldown;
    // 설정값
    protected float _attackIntervalMax => 3f;
    protected PlayerController _player;
    protected bool _isAttacking = false;
    protected bool _isAttacked = false;
    public override void Init(int maxHp, Vector3 pos, float monsterSize, MonsterType monsterType)
    {
        animator.speed = 1;
        hpBar.gameObject.SetActive(true);
        _player = GameContainer.Instance.Player;
        transform.position = pos;
        _meshRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(ColorID, Color.white);
        _meshRenderer.SetPropertyBlock(_mpb);
        MonsterType = monsterType;
        size = monsterSize;
        _attackRange = size / 2 + 2.3f;
        _isDead = false;
        _maxHp = maxHp;
        _currentHp = _maxHp;
        float fillAmount = Mathf.Max(0, _currentHp) / _maxHp;

        hpBar.SetFillAmount(fillAmount);
        GameContainer.Instance.HUD.RefreshBossHpProgressBar(fillAmount);
        _attackCooldown = 0;
    }
    private void Update()
    {
        if(_attackCooldown > 0) _attackCooldown -= Time.deltaTime;
        Attack();
    }
    protected virtual void Attack()
    {
        if(_isDead == true) return;
        if (_player == null) return;
        if(_player.IsDead == true) return;
        bool distanceCheck = Mathf.Abs(_player.transform.position.x - transform.position.x) <= _attackRange;
        if (distanceCheck == true && _isAttacking == false)
        {
            if (_attackCooldown <= 0)
            {
                _isAttacking = true;
                animator.Play(ATTACK, 0, 0f);
                _attackCooldown = _attackIntervalMax;
            }
        }
        if (_isAttacking == true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == ATTACK)
            {
                CheckAttackTiming(stateInfo, 0.59f, attack);
                if (stateInfo.normalizedTime >= 1)
                {
                    _isAttacking = false;
                    _isAttacked = false;
                    animator.Play(IDLE, 0, 0f);
                }
            }
        }
    }
    private void CheckAttackTiming(AnimatorStateInfo state, float attackTime, int damage)
    {
        if (state.normalizedTime >= attackTime && _isAttacked == false)
        {
            _isAttacked = true;
            CombatEvent combatEvent = new CombatEvent()
            {
                Receiver = _player,
                Damage = damage,
                DamageType = DamageType.Normal
            };
            GameContainer.Instance.CombatSystem.AddCombatEvent(combatEvent);
        }
    }
    public override void TakeDamage(int damage)
    {
        if (_isDead == true) return;
        _currentHp -= damage;
        float fillAmount = Mathf.Max(0, _currentHp) / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        GameContainer.Instance.HUD.RefreshBossHpProgressBar(fillAmount);
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
            DropEffect go = Managers.Resource.Instantiate("ExpDropEffect", pooling: true).GetComponent<DropEffect>();
            go.Init(EffectPos.position);
            go = Managers.Resource.Instantiate("GoldDropEffect", pooling: true).GetComponent<DropEffect>();
            go.Init(EffectPos.position);
            Dead();
            DeadEffect();
        }
        
    }
}
