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
        // target�� ������ �� �����̰�, �����ϰ� ���� ���� ���� ������ ���� ���
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
