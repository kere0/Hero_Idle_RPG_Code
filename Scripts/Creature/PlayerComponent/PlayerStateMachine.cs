using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private BasePlayerState _currentState;

    public readonly MoveState MoveState;
    public readonly AttackState AttackState;
    public readonly SkillState SkillState;
    public readonly KnockBackState KnockBackState;
    public readonly DeathState DeathState;
    public PlayerStateMachine(PlayerController playerController)
    {
        MoveState = new MoveState(playerController, this);
        AttackState = new AttackState(playerController, this);
        SkillState = new SkillState(playerController, this);
        KnockBackState = new KnockBackState(playerController, this);
        DeathState = new DeathState(playerController, this);
        
    }
    public void Start()
    {
        _currentState = MoveState;
        _currentState.Enter();
    }
    public void ChangeState(BasePlayerState state)
    {
        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }
    public void Update()
    {
        _currentState.Update();
    }

    public BasePlayerState GetCurrentState()
    {
        return _currentState;
    }
}
