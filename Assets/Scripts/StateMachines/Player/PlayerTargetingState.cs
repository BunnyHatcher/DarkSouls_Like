using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree"); //Hash to be used in BlendTree Animation method

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        //Play BlendTree Animation via Hash
        stateMachine.Animator.Play(TargetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        Debug.Log(stateMachine.Targeter.CurrentTarget.name);
        
        //when no more target is in reach, we switch back to the FreeLook State
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
