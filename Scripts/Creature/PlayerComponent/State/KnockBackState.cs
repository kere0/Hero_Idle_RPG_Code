using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackState : BasePlayerState
{
    // 넉백
    public bool _isKnockback = false;
    public float _knockTime = 1f;
    public float _knockPower = 5f;
    public Vector3 _knockDir;
    public float _elapsed = 0f;
    public KnockBackState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }
    public override void Enter()
    {
        _player.animator.Play(PlayerController.KNOCKBACK, 0, 0f);
        _isEnterEnd = true;
    }
    public override void Update()
    {
        if (_isEnterEnd == true)
        {
            UpdateKnockBack();
        }
    }
    private void UpdateKnockBack()
    {
        _elapsed += Time.deltaTime;
        float t = 1f - (_elapsed / _knockTime); // 점점 줄어드는 힘
        _player.transform.position += _knockDir * (_knockPower * t * Time.deltaTime);
    
        if (_elapsed >= _knockTime)
        {
            _player.CurrentTarget = null;
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }
    public void ApplyKnockBack(Vector3 knockDir, float knockPower, float duration)
    {
       _knockDir = knockDir;
        _knockPower = knockPower;
        _knockTime = duration;
        _elapsed = 0f;
    }
    public override void Exit()
    {
        _player.timer = 0f;
        _isEnterEnd = false;
    }
}
