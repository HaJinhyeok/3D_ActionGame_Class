using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Inventory Inventory;

    // * 컴포넌트
    Transform _character;
    Animator _animator;
    Rigidbody _rigidbody;

    // * 카메라
    Camera _camera;
    Transform _camAxis;
    float _camSpeed = 8f;
    float _mouseX = 0;
    float _mouseY = 4;
    float _wheel = -10;
    Vector3 _offset = Vector3.zero;

    // * 애니메이션
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

    public bool IsNextCombo
    {
        get { return _animator.GetBool(Define.isNextCombo); }
        set { _animator.SetBool(Define.isNextCombo, value); }
    }

    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.isAttacking); }
        set { _animator.SetBool(Define.isAttacking, value); }
    }

    void Start()
    {
        // * 컴포넌트
        _character = transform.GetChild(0);
        _animator = _character.GetComponent<Animator>();
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        _rigidbody.freezeRotation = true;

        // * 콜라이더
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.center = new Vector3(0, 2.95f, 0);
        capsule.radius = 1f;
        capsule.height = 6f;

        // * 카메라
        _camera = Camera.main;
        _camAxis = new GameObject("CamAxis").transform;
        _camera.transform.parent = _camAxis;
        _camera.transform.position = new Vector3(0, 5, -10);
    }

    void Update()
    {
        Attack();
        Zoom();
        CameraMove();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Attack()
    {
        // UI 클릭 여부 반환 함수
        bool isUIClick = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        bool isLeftMouseDown = Input.GetMouseButtonDown(0);
        if (isLeftMouseDown == false || isUIClick)
            return;
        if (IsAttacking == false)
        {
            IsAttacking = true;
        }
        else
        {
            IsNextCombo = true;
        }
    }

    void Zoom()
    {
        _wheel += Input.GetAxis(Define.MouseScroll) * 10;
        if (_wheel >= -10)
            _wheel = -10;
        if (_wheel <= -20)
            _wheel = -20;

        _camera.transform.localPosition = new Vector3(0, 0, _wheel);
    }

    void Move()
    {
        if (Input.GetButton(Define.Horizontal) || Input.GetButton(Define.Vertical))
        {
            float h = Input.GetAxis(Define.Horizontal);
            float v = Input.GetAxis(Define.Vertical);

            Vector3 movement = new Vector3(h, _rigidbody.linearVelocity.y, v);
            transform.rotation = Quaternion.Euler(new Vector3(0, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);

            // local 방향에서 world 방향으로 바꿔주는 역할???
            Vector3 localMovement = transform.TransformDirection(movement);
            _rigidbody.linearVelocity = localMovement.normalized * 10;

            _character.transform.localRotation = Quaternion.Slerp(_character.transform.localRotation, Quaternion.LookRotation(movement), 5 * Time.deltaTime);
        }
        else
        {
            // 속도 0으로 만들어주기
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
        }
        Speed = _rigidbody.linearVelocity.sqrMagnitude;
        _camAxis.position = transform.position + new Vector3(0, 4, 0);
        _character.eulerAngles = new Vector3(0, _character.eulerAngles.y, 0);

    }

    void CameraMove()
    {
        _mouseX += Input.GetAxis(Define.MouseX);
        _mouseY -= Input.GetAxis(Define.MouseY);

        if (_mouseY > 10)
            _mouseY = 10;
        if (_mouseY < 0)
            _mouseY = 0;

        _camAxis.rotation = Quaternion.Euler(new Vector3(_camAxis.rotation.x + _mouseY, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);
    }

    public bool PickUpItem(Item item, int amount = -1)
    {
        if (item != null && Inventory.AddItem(item, amount))
        {
            Destroy(item.gameObject);
            return true;
        }
        return false;
    }
}
