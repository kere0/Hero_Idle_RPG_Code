using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisBoss : StageBoss
{
    private bool _firstAttack = false;
    private bool _secondAttack = false;
    protected override void Attack()
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
                if (_firstAttack == false && stateInfo.normalizedTime >= 0.57f)
                {
                    Debug.Log("첫 번째 공격");
                    _firstAttack = true;
                    AttackPlayer();
                }
                if (_secondAttack == false && stateInfo.normalizedTime >= 0.9f)
                {
                    Debug.Log("두번째 공격");
                    _secondAttack = true;
                    AttackPlayer();
                }
                if (stateInfo.normalizedTime >= 1)
                {
                    _isAttacking = false;
                    _firstAttack = false;
                    _secondAttack = false;
                    animator.Play(IDLE, 0, 0f);
                }
            }
        }
    }
    private void AttackPlayer()
    {
        CombatEvent combatEvent = new CombatEvent()
        {
            Receiver = _player,
            Damage = attack,
            DamageType = DamageType.Normal
        };
        GameContainer.Instance.CombatSystem.AddCombatEvent(combatEvent);
    }
}
