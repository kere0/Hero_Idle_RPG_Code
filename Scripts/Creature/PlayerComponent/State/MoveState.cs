using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BasePlayerState
{
    public MoveState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }
    public override void Enter()
    {
        _isEnterEnd = true;
    }
    public override void Update()
    {
        if (_isEnterEnd == true)
        {
            _player.transform.position += Vector3.right * (_player.speed * Time.deltaTime);
            DetectTarget();
        }
    }
    private void DetectTarget()
    {
        AnimatorStateInfo state = _player.animator.GetCurrentAnimatorStateInfo(0);

        List<BaseMonster> monsters = GameContainer.Instance.BattleManager.targetList;
        if (monsters.Count > 0)
        {
            if (monsters[0] == null) return;
            float attackRange = monsters[0].AttackRange;
            float dist = Mathf.Abs(_player.transform.position.x - monsters[0].transform.position.x);
            if (dist <= attackRange)
            {
                _player.CurrentTarget = monsters[0];
                _stateMachine.ChangeState(_stateMachine.AttackState);
            }
            else
            {
                _player.CurrentTarget = null;
                if (state.shortNameHash != PlayerController.MOVE)
                {
                    _player.animator.Play(PlayerController.MOVE);
                }
            }
        }
        else
        {
            _player.CurrentTarget = null;
            if (state.shortNameHash != PlayerController.MOVE)
            {
                _player.animator.Play(PlayerController.MOVE);
            }
        }
    }
    public override void Exit()
    {
        _isEnterEnd = false;
    }
}
