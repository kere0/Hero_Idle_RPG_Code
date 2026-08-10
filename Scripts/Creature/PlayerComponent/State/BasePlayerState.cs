using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerState
{
    public BasePlayerState(PlayerController playerController, PlayerStateMachine playerStateMachine)
    {
        _player = playerController;
        _stateMachine = playerStateMachine;
    }
    protected PlayerController _player;
    protected PlayerStateMachine _stateMachine;
    protected bool _isEnterEnd = false;
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
