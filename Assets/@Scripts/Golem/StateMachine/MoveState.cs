using UnityEngine;

public class MoveState : State<GolemController>
{
    Rigidbody _rigidbody;

    public override void OnInitialized()
    {
        _rigidbody = context?.GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _rigidbody.linearVelocity = Vector3.zero;
        context.Speed = 3;
        context.IsAttacking = false;
        context.ComboCount = 0;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        // target이 없으면 안 움직이고, 존재하고 공격 범위 내에 있으면 공격 모드
        if (context.Target == null)
        {
            stateMachine.ChangeState<IdleState>();
        }
        if (context.IsAvailableAttack)
        {
            stateMachine.ChangeState<AttackState>();
        }
        context.RotateToTarget();
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);
        context.MoveToTarget();
    }

    public override void OnExit()
    {
        base.OnExit();
        context.Speed = 0;
        _rigidbody.linearVelocity = Vector3.zero;
    }
}
