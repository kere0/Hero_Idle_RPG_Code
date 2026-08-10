using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BasePlayerState
{
    private int _attackNum;
    private bool _isAttacking = false;
    private bool _isAttacked = false;
    private float _defaultAttackAnimatorSpeed;

    public AttackState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }

    public override void Enter()
    {
        _isAttacking = false;
        _isAttacked = false;
        _player.animator.Play(PlayerController.IDLE, 0, 0f);
    }

    public override void Update()
    {
        _player.animator.speed = (1 / _player.PlayerManager.GetAttackInterval()) / _player.PlayerManager.playerData.DefaultAttackSpeed;
        if (_isAttacked == false && (_player.CurrentTarget == null || _player.CurrentTarget.IsDead == true))
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
        if (_player.PlayerManager.GetAttackInterval() <= _player.timer)
        {
            Attack();
        }
    }
    private void Attack()
    {
        AnimatorStateInfo state;
        if (_isAttacking == false)
        {
            _isAttacking = true;
            _attackNum = Random.Range(1, 3);
            if (_attackNum == 1)
            {
                _player.animator.Play(PlayerController.ATTACK, 0, 0f);
            }
            else if (_attackNum == 2)
            {
                _player.animator.Play(PlayerController.THRUSTATTACK, 0, 0f);
            }
            SoundManager.Instance.PlaySFX("Attack", 0.5f);
            _player.animator.Update(0);
            state = _player.animator.GetCurrentAnimatorStateInfo(0);
            float attackSpeedMultiplier = (1f / _player.PlayerManager.GetAttackInterval()) / _player.PlayerManager.playerData.DefaultAttackSpeed;
            float animationSpeed = state.length / (1 / _player.PlayerManager.playerData.DefaultAttackSpeed);
            _player.animator.speed = animationSpeed * attackSpeedMultiplier;
        }
        state = _player.animator.GetCurrentAnimatorStateInfo(0);
        if (_attackNum == 1)
        {
            if (state.shortNameHash == PlayerController.ATTACK)
            {
                CheckAttackTiming(state, 0.5f);
            }
        }
        else if (_attackNum == 2)
        {
            if (state.shortNameHash == PlayerController.THRUSTATTACK)
            {
                CheckAttackTiming(state, 1f);
            }
        }
    }
    private void CheckAttackTiming(AnimatorStateInfo state, float attackEndTime)
    {
        if (state.normalizedTime >= 0.3f && _isAttacked == false)
        {
            _isAttacked = true;
            int criticalChance = _player.PlayerManager.playerData.EnhanceData.CritChanceLevel;
            int attack;
            bool isCritical = Random.Range(0, 100) < criticalChance;
            if (isCritical == true)
            {
                attack = _player.PlayerManager.GetTotalCriticalDamage();
            }
            else
            {
                attack = _player.PlayerManager.GetTotalDamage();
            }
            CombatEvent combatEvent = new CombatEvent()
            {
                Receiver = _player.CurrentTarget,
                Damage = attack,
                DamageType = DamageType.Normal,
                IsCritical = isCritical
            };
            GameContainer.Instance.CombatSystem.AddCombatEvent(combatEvent);
        }
        if (_isAttacking == true && state.normalizedTime >= attackEndTime)
        {
            _isAttacking = false;
            _isAttacked = false;
            _player.timer = 0f;
            _player.animator.Play(PlayerController.IDLE, 0, 0f);
        }
    }
    public override void Exit()
    {
        _isAttacking = false;
        _isAttacked = false;
        _player.animator.speed = 1;
    }
}
