using UnityEngine;

public class IdleState : State<GolemController>
{
    Rigidbody _rigidbody;

    public override void OnInitialized()
    {
        _rigidbody = context?.GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        context.Speed = 0;
        context.IsAttacking = false;
        context.ComboCount = 0;
        _rigidbody.linearVelocity = Vector3.zero;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (!context.Target)
            return;
        if(!context.IsAvailableAttack)
        {
            stateMachine.ChangeState<MoveState>();
        }
    }
}
