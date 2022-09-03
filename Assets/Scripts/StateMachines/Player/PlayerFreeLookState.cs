using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{


    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = 0.1f;


    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        //Targeting
        stateMachine.InputReader.TargetEvent += OnTarget;
    }


    public override void Tick(float deltaTime)
    {
        //Movement Events
        Vector3 movement = CalculateMovement();

        stateMachine.CharacterController.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        //Moving into direction the player is facing
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMoveDirection(movement, deltaTime);

    }


    public override void Exit()
    {
        // stop targeting
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        //switch target state
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    //Calculating the movement method
    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y +
                right * stateMachine.InputReader.MovementValue.x;
     
    }

    //method to make player turn into direction they are facing
    private void FaceMoveDirection(Vector3 movement, float deltaTime) 
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }

}
