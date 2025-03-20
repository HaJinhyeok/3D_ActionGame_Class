using UnityEngine;

public class GolemController : MonoBehaviour
{
    float _attackRange = 7;
    Rigidbody _rigidbody;
    FieldOfView _fieldOfView;
    Animator _animator;

    protected StateMachine<GolemController> _stateMachine;

    public Transform Target => _fieldOfView?.NearestTarget;

    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.isAttacking); }
        set { _animator.SetBool(Define.isAttacking, value); }
    }

    public float Speed
    {
        get { return _animator.GetFloat(Define.Speed); }
        set { _animator.SetFloat(Define.Speed, value); }
    }

    public int ComboCount
    {
        get { return _animator.GetInteger(Define.ComboCount); }
        set { _animator.SetInteger(Define.ComboCount, value); }
    }

    public bool IsAvailableAttack
    {
        get
        {
            if (!Target) return false;
            float distance = (Target.position - transform.position).sqrMagnitude;
            return distance <= (_attackRange * _attackRange);
        }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _fieldOfView = GetComponent<FieldOfView>();

        _stateMachine = new StateMachine<GolemController>(this, new IdleState());
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
    }

    void Update()
    {
        _stateMachine.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _stateMachine.OnFixedUpdate(Time.fixedDeltaTime);
    }

    public void RotateToTarget()
    {
        if (!Target) return;

        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public void MoveToTarget()
    {
        // rotate를 통해 target 바라보게 회전은 되어있으므로 앞으로 가기만 하면 됨
        _rigidbody.MovePosition(transform.position + (transform.forward * 3 * Time.deltaTime));
    }
}
