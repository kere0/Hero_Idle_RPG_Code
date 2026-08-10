using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SkillState : BasePlayerState
{
    private int _useSkillSlotIndex = -1;
    private bool _isAttacked = false;
    private int _currentAnimationHash = -1;
    private SkillInstance _skillInstance;
    private float _dashTimer;
    private bool isUseSkill = false;
    public SkillState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }
    public override void Enter()
    {
        if (_useSkillSlotIndex == -1) return;
        _skillInstance = _player.PlayerSkillController.GetSkill(_useSkillSlotIndex);
        if (_skillInstance == null) return;
        _player.isCastingSkill = true;
        isUseSkill = false;
        _currentAnimationHash = _skillInstance.skill.skillInfo.animationHash;
        if (_currentAnimationHash != -1)
        {
            _player.animator.Play(_currentAnimationHash, 0, 0);
        }

        if (_currentAnimationHash == PlayerController.VOIDSTRIKE)
        {
            SoundManager.Instance.PlaySFX("VoidStrike");
        }
        _isEnterEnd = true;
    }
    public void SetUseSkillSlotIndex(int slotIndex)
    {
        _useSkillSlotIndex = slotIndex;
    }
    public override void Update()
    {
        if (_isEnterEnd == true)
        {
            if (_skillInstance == null) return;
            if (_currentAnimationHash != -1)
            {
                Attack();
            }
            Dash();

        }
    }

    private void Dash()
    {
        if (_currentAnimationHash == PlayerController.ThunderCharge)
        {
            _dashTimer += Time.deltaTime;
            BaseMonster monsters = null;
            if (GameContainer.Instance.BattleManager.targetList.Count > 0)
            {
                monsters = GameContainer.Instance.BattleManager.targetList[0];
            }
            if (monsters == null)
            {
                _player.transform.position += Vector3.right * (_player.speed * Time.deltaTime * 9f);
            }
            else
            {
                float attackRange = monsters.AttackRange;
                float dist = Mathf.Abs(_player.transform.position.x - monsters.transform.position.x);
                if (dist > attackRange)
                {
                    _player.transform.position += Vector3.right * (_player.speed * Time.deltaTime * 9f);
                }
            }
        }
    }
    private void Attack()
    {
        AnimatorStateInfo state = _player.animator.GetCurrentAnimatorStateInfo(0);
        if (_currentAnimationHash == PlayerController.ATTACK)
        {
            CheckAttackTiming(state, 0.5f);
        }
        else
        {
            CheckAttackTiming(state, 1f);
        }
    }
    private void CheckAttackTiming(AnimatorStateInfo state, float attackEndTime)
    {
        if (state.shortNameHash != _currentAnimationHash) return;
        if (state.normalizedTime >= _skillInstance.skill.skillInfo.castTiming && _isAttacked == false)
        {
            if (_isAttacked == false)
            {
                _isAttacked = true;
                isUseSkill = true;
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
                _player.PlayerSkillController.UseSkill(_useSkillSlotIndex, attack, isCritical);
            }
        }

        if (_currentAnimationHash == PlayerController.ThunderCharge)
        {
            if (_dashTimer > 0.2f)
            {
                _stateMachine.ChangeState(_stateMachine.MoveState);
                DOVirtual.DelayedCall(0.35f, () => _player.isCastingSkill = false);
            }
        }
        else
        {
            if (state.normalizedTime >= attackEndTime)
            {
                _stateMachine.ChangeState(_stateMachine.MoveState);
            }
        }
    }
    public override void Exit()
    {
        if (_currentAnimationHash == PlayerController.ThunderCharge)
        {
            _dashTimer = 0f;
        }
        else
        {
            _player.isCastingSkill = false;
        }
        if (isUseSkill == false)
        {
            _skillInstance.cooldown = 0;
        }
        isUseSkill = false;
        _currentAnimationHash = -1;
        _useSkillSlotIndex = -1;
        _isAttacked = false;
        _player.timer = 0f;
        _skillInstance = null;
    }
}
