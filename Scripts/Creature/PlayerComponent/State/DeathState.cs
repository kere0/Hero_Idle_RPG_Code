using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BasePlayerState
{
    public DeathState(PlayerController playerController, PlayerStateMachine playerStateMachine) : base(playerController, playerStateMachine)
    {
    }
    public override void Enter()
    {
        _player.Dead();
        _player.animator.Play(PlayerController.DEAD, 0, 0f);
    }

    public override void Update() { }

    public override void Exit()
    {
    }
}
