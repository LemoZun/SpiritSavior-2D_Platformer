using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public enum State {Idle, Run, Dash, Jump, DoubleJump, Fall, Land, WallGrab, WallSliding, WallJump, Damaged, WakeUp, Dead, Spawn, Size}

    [SerializeField] State _curState;
    //public State prevState;
    //private BaseState[] _states = new BaseState[(int)State.Size];
    private PlayerState[] _states = new PlayerState[(int)State.Size];

    public PlayerModel.Ability unlockedAbilities = PlayerModel.Ability.None;

    public PlayerModel playerModel = new PlayerModel();
    public PlayerView playerView;

    private Collider2D _playerCollider;
    private int groundLayerMask;
    private int wallLayerMask; 

    

    //public SpriteRenderer renderer;
    [Header("Player Setting")]
    public float moveSpeed;        // �̵��ӵ�
    //public float maxMoveSpeed;     // �̵��ӵ��� �ִ밪
    public float dashForce;         // ��� ��
    [HideInInspector] public float lowJumpForce;     // �������� �� // ���
    public float jumpForce;    // �������� ��
    public float maxJumpTime;     // �ִ����� �ð�
    [HideInInspector] public float slopeJumpBoost; // ���鿡���� �߰� ���� ������ �� // ���
    [HideInInspector] public float jumpCirticalPoint; // ��������, ���������� ������ ���� // ���
    public float doubleJumpForce; // ���� ������ �󸶳� ���� �ö��� ����
    public float knockbackForce; // �ǰݽ� �󸶳� �ڷ� �з��� �� ����

    //�⺻ �̵��ӵ��� ���� ��ȭ�Ǵ� ���� ����x
    [HideInInspector] public float moveSpeedInAir;    // ���߿��� �÷��̾��� �ӵ�
    [HideInInspector] public float maxMoveSpeedInAir; // ���߿��� �÷��̾��� �ӵ��� �ִ밪

    [Header("SpeedInAir = SpeedInGround * x")]
    public float speedAdjustmentOffsetInAir; // ���߿����� �ӵ� = �������� �ӵ� * �ش� ����

    [Header("Checking")]
    [HideInInspector] public Rigidbody2D rigid;
    public float hp;
    
    //public bool hasJumped = false;          //
    [HideInInspector] public float jumpChargingTime = 0f;     // �����̽��� �����ð� üũ
    public bool isDoubleJumpUsed; // �������� ��� ������ ��Ÿ���� ����
    public bool isDashUsed; // ��ø� ����ߴ��� ������ ��Ÿ���� ����
    public float dashCoolTime; // ��� ��� �� ��Ÿ��
    [HideInInspector] public float dashDeltaTime;
    public bool isStuck; // ���� �������� Ȯ��
    public bool isDead = false; // �׾����� Ȯ��
    
    [Header("Ground & Slope & Wall Checking")]
    [SerializeField] Transform _groundCheckPoint;
    public Transform _wallCheckPoint;
    private float _wallCheckDistance = 0.01f;
    private float _wallCheckHeight = 2.25f; // �ʹ� ��� ��絵 ������ �ν���

    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _slopeCheckDistance;
    public float groundAngle;
    public Vector2 perpAngle;
    public bool isSlope;
    public float maxAngle; // �̵� ������ �ִ� ����

    //[HideInInspector] public float maxFlightTime; // ���� �� �ٷ� fall ���·� ���� �ʱ� ���� ����

    public int isPlayerRight = 1;
    public bool isGrounded;        // ĳ���Ͱ� ���� �پ��ִ��� üũ

    public RaycastHit2D groundHit;
    public RaycastHit2D slopeHit;
    public RaycastHit2D wallHit;


    public bool isWall;                  // ĳ���Ͱ� ���� �پ��ִ��� üũ
    public bool isWallJumpUsed;         // ������ �������� ��� �ߴ��� üũ
    public float wallSlidingSpeed = 0.5f; // �߷°�� �������� ���� �����ؾ���
    public float wallJumpPower;

    private Vector2 _wallCheckBoxSize;
    Coroutine _wallCheckRoutine;
    //Coroutine _groundCheckRoutine;

    [Header("Input")]
    public float moveInput;

    // �ڿ��� Ÿ��
    public float coyoteTime = 0.2f;
    [HideInInspector] public float coyoteTimeCounter;

    //���� ����
    public float jumpBufferTime = 0.2f;
    [HideInInspector] public float jumpBufferCounter;
    //[HideInInspector]
    //[HideInInspector]

    private void Awake()
    {
        if(playerModel != null)
        {
            playerModel.curNature = PlayerModel.Nature.Red;
        }
        else
        {
            Debug.LogError("�� ���� ����");
        }

        rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
            Debug.LogError("rigidBody����");
        _playerCollider = GetComponent<CapsuleCollider2D>();
        

        _states[(int)State.Idle] = new IdleState(this);
        _states[(int)State.Run] = new RunState(this);
        _states[(int)State.Dash] = new DashState(this);
        _states[(int)State.Jump] = new JumpState(this);
        _states[(int)State.DoubleJump] = new DoubleJumpState(this);
        _states[(int)State.Fall] = new FallState(this);
        _states[(int)State.Land] = new LandState(this);
        _states[(int)State.WallGrab] = new WallGrabState(this);
        _states[(int)State.WallSliding] = new WallSlidingState(this);
        _states[(int)State.WallJump] = new WallJumpState(this);
        _states[(int)State.Damaged] = new DamagedState(this);
        _states[(int)State.WakeUp] = new WakeupState(this);
        _states[(int)State.Dead] = new DeadState(this);
        _states[(int)State.Spawn] = new SpawnState(this);
        moveSpeedInAir = moveSpeed * speedAdjustmentOffsetInAir;
        //maxMoveSpeedInAir = maxMoveSpeed * speedAdjustmentOffsetInAir;


        if (_groundCheckPoint == null)
            _groundCheckPoint = transform.Find("BottomPivot");

        if (_wallCheckPoint == null)
            _wallCheckPoint = transform.Find("WallCheckPoint");

        //if (_groundCheckRoutine == null)
        //    _groundCheckRoutine = StartCoroutine(CheckGroundRayRoutine());

        if (_wallCheckRoutine == null) // �ۼ���
            _wallCheckRoutine = StartCoroutine(CheckWallDisplayRoutine());

        _wallCheckBoxSize = new Vector2(_wallCheckDistance, _wallCheckHeight);

        //�ӽ� ü�� Ȯ�ο�
        hp = playerModel.hp;
    }


    void Start()
    {
        playerView = GetComponent<PlayerView>();
        _curState = State.Spawn;
        _states[(int)_curState].Enter();
        SubscribeEvents();
        wallLayerMask = LayerMask.GetMask("Wall");
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        _states[(int)_curState].Update();
        TagePlayer();
        CheckDashCoolTime();

        //��üũ�� ��� fixedUpdate���� �����ϸ� wallGrab �ִϸ��̼��� ���� ����� �ȵȴ�
        //�� üũ �ֱ��� ��������. Update���� �ϴ� ������ �����
        CheckWall();
        ControlCoyoteTime();
        ControlJumpBuffer();
        //CheckGroundRaycast();



        //// �̲����� ����1
        //if (player.moveInput == 0)
        //{
        //    player.rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePosition;
        //}
        //else
        //{
        //    player.rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        //}
        // �̲��� ����2
        //if(moveInput == 0)
        //{
        //    rigid.velocity = new Vector2(0,rigid.velocity.y);
        //}

        //�ӽ� �ǰ� Ʈ����
        if (Input.GetKeyDown(KeyCode.O))
        {
            playerModel.TakeDamageEvent(1); // �ӽ�
        }

        ////�ӽ� ���� Ʈ����
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    playerModel.DiePlayer();
        //    Debug.Log("����");
        //}

        ////�ӽ� �ɷ� �ر� Ʈ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UnlockAbility(PlayerModel.Ability.Tag);
            //Debug.Log("�±� �ر�");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnlockAbility(PlayerModel.Ability.Dash);
            //Debug.Log("��� �ر�");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UnlockAbility(PlayerModel.Ability.WallJump);
            //Debug.Log("������ �ر�");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UnlockAbility(PlayerModel.Ability.DoubleJump);
           // Debug.Log("�������� �ر�");
        }



        //�ӽ� ü�� Ȯ�ο�
        //hp = playerModel.hp;

    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0) 
            return;
        _states[(int)(_curState)].FixedUpdate();
        //���⼭ �ٴ�üũ�� �ϴϱ� �ϳ��� �ذ��..
        CheckGroundRaycast();
        //CheckWall();
    }

    public void CheckDashCoolTime()
    {
        if (!isDashUsed)
            return;

        // �뽬�� ���� ��Ÿ�Ӹ�ŭ �������
        if(dashDeltaTime >= dashCoolTime)
        {
            isDashUsed = false;
            //dashDeltaTime�� 0���� �ʱ�ȭ���ִ°� ������Խ� ����
        }
        else
        {
            dashDeltaTime += Time.deltaTime;
        }
    }

    public void ChangeState(State nextState)
    {
        // �����Ƽ�� �رݵƴ��� Ȯ���ϴ� ����

        ////���1.
        //if (_states[(int)nextState].ability != Ability.None)
        //{
        //    if (HasAbility(_states[(int)nextState].ability))
        //    {
        //        _states[(int)_curState].Exit();
        //        _curState = nextState;
        //        _states[(int)_curState].Enter();
        //    }
        //    else
        //    {
        //        Debug.Log("���� �ر����� ���� �ɷ�");
        //    }
        //}
        //else
        //{
        //    _states[(int)_curState].Exit();
        //    _curState = nextState;
        //    _states[(int)_curState].Enter();
        //}

        // ���������� ���ܻ��� ó��
        if(_curState == State.WallJump && nextState == State.DoubleJump)
        {
            _states[(int)_curState].Exit();
            _curState = nextState;
            _states[(int)_curState].Enter();
        }


        //���2. �ߺ� �ڵ带 ����
        if (_states[(int)nextState].ability == PlayerModel.Ability.None || HasAbility(_states[(int)nextState].ability))
        {
            _states[(int)_curState].Exit();
            _curState = nextState;
            _states[(int)_curState].Enter();
        }
        else
        {
            //Debug.Log("���� �ر����� ���� �ɷ�");
        }
        
    }

    private void CheckGroundRaycast()
    {
        // �� üũ�� ���� �������� �������� üũ�ϴ� �޼���

        groundHit = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckDistance, groundLayerMask);
        slopeHit = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _slopeCheckDistance, groundLayerMask);
        //��ֺ��ͷ� ������ ����
        isGrounded = groundHit;
        // Vector2.Perpendicular(Vector2 A) : A�� ������ �ݽð� �������� 90�� ȸ���� ���Ͱ��� ��ȯ

        if(isGrounded)
        {
            //if (rigid.sharedMaterial.friction != 0.6f)
            //    rigid.sharedMaterial.friction = 0.6f;

            perpAngle = Vector2.Perpendicular(groundHit.normal).normalized; // 
            groundAngle = Vector2.Angle(groundHit.normal, Vector2.up);

            if(groundAngle != 0)
                isSlope = true;
            else
                isSlope = false;


            if(groundAngle > maxAngle)
            {
                Debug.Log(groundAngle);
                moveInput = 0;
            }
            else
            {
                //Debug.Log(groundAngle);
            }


            //��������, ���鿡�� ����
            Debug.DrawLine(groundHit.point, groundHit.point + groundHit.normal, Color.blue);

            // ���������� ������ ����, ����
            Debug.DrawLine(groundHit.point, groundHit.point + perpAngle, Color.red);

        }
    }
    private void CheckWall()
    {
        wallHit = Physics2D.BoxCast(_wallCheckPoint.position, _wallCheckBoxSize, 0, Vector2.right * isPlayerRight, _wallCheckDistance);
        isWall = wallHit;

        if (wallHit.collider == null)
            return;

        // Ʈ���ſ����� return
        if (wallHit.collider.isTrigger)
            return;

        if (HasAbility(PlayerModel.Ability.WallJump) && (wallLayerMask & (1 << wallHit.collider.gameObject.layer)) != 0) //��Ʈ�������� ���̾� ��ġ ���� Ȯ�� (���� ������) // ��Ÿ�� ������ ���� ���
        {
            if (isGrounded || _curState == State.WallJump || _curState == State.WallGrab || _curState == State.WallSliding ) //�ʹ� �䵥
                return;

            if (moveInput == isPlayerRight && moveInput != 0) //&& _curState != State.WallGrab && _curState != State.WallSliding)
                ChangeState(State.WallGrab);
        }
        else // ��Ÿ�� �Ұ����� ���̾��� ���
        {
            float wallAngle = Vector2.Angle(Vector2.up, wallHit.normal);
            if (wallAngle > maxAngle)
            {
                Vector2 slideDirection = Vector2.Perpendicular(wallHit.normal).normalized;
                //Debug.Log("a");
                //rigid.velocity = new Vector2(0, rigid.velocity.y);
                rigid.velocity = new Vector2(slideDirection.x * rigid.velocity.x, rigid.velocity.y);

            }
            //if(rigid.sharedMaterial.friction != 0)
            //    rigid.sharedMaterial.friction = 0f;


            //float slopeAngle = Vector2.Angle(Vector2.up, wallHit.normal); // ���� ���� ���Ϳ� ���� ������ ����
            //if (slopeAngle > 45f) // ���� ���, 45�� �̻��� ����
            //{
            //    // ������ ��� ������ �����ϰų� ������ ó���� �մϴ�.
            //    if (rigid.velocity.y > 0) // ���� �÷��̾ ���� �����ϰ� �ִٸ�
            //    {
            //        rigid.velocity = new Vector2(rigid.velocity.x, 0); // y�� �ӵ��� 0���� �����Ͽ� ������ ���߰� ��
            //    }
            //}

            ////Debug.Log($"���� ���� {rigid.velocity}");
            //// ���� ������ ��

            //if (moveInput != 0 && rigid.velocity.y == Vector2.zero.y)
            //{
            //    if (moveInput == Mathf.Sign(-wallHit.normal.x))
            //    {
            //        Debug.Log("aa");
            //        // �̷��� �������� ������ �������
            //        Vector2 pushBack = new Vector2(wallHit.normal.x * 0.1f, 0f);
            //        rigid.position += pushBack;
            //        moveInput = 0; // �÷��̾� �Է� ����
            //        //rigid.velocity = new Vector2(0, -5.0f);  //rigid.velocity.y*2.0f);
            //        // �ʹ� ������ ����ε� �ٸ������ ������
            //    }
            //}
        }
    }

    public void MoveInAir()
    {
        // �� ���� ���� ������ ����
        // �������� 0���� �δ°��� ������� �����°��� ��Ȯ�� ���� �ʿ䰡 ����
        //rigid.sharedMaterial.friction = 0f;
        if (!isStuck)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        

        rigid.velocity = new Vector2(moveInput * moveSpeedInAir, rigid.velocity.y);

        FlipPlayer(moveInput);

        //RaycastHit2D hit = Physics2D.BoxCast(_wallCheckPoint.position, _wallCheckBoxSize, 0, Vector2.right * isPlayerRight, _wallCheckDistance);
        //CheckWall();
        //Dash ���·� ��ȯ
        CheckDashable();
    }

    private void ControlCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void ControlJumpBuffer()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    public void UnlockAbility(PlayerModel.Ability ability)
    {
        if(HasAbility(ability))
        {
            Debug.Log("�̹� �رݵ� �ɷ��Դϴ�.");
            return;
        }

        unlockedAbilities |= ability;
        playerModel.UnlockAbilityEvent(ability);
        Debug.Log($"{ability} �ر�");
    }

    public bool HasAbility(PlayerModel.Ability ability)
    {
        return (unlockedAbilities & ability) == ability;
    }

    public void FlipPlayer(float _moveDirection)
    {
        playerView.FlipRender(_moveDirection);
        AdjustWallCheckPoint();
        AdjustColliderOffset();
    }

    private void AdjustWallCheckPoint()
    {
        _wallCheckPoint.localPosition = new Vector2(Mathf.Abs(_wallCheckPoint.localPosition.x) * isPlayerRight, _wallCheckPoint.localPosition.y);
    }

    private void AdjustColliderOffset()
    {
        _playerCollider.offset = new Vector2(Mathf.Abs(_playerCollider.offset.x) * isPlayerRight, _playerCollider.offset.y);
    }

    public void TagePlayer()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(HasAbility(PlayerModel.Ability.Tag))
            {
                // playerView.ChangeSprite(); // ��� �ִϸ��̼� ��� ���¶� ��� ����
                playerModel.TagPlayerEvent(); // �Ӽ� ������ ������ curNature�� �ٲ��� + �±� �̺�Ʈ Invoke
            }
            else
            {
                Debug.Log("�±� �ɷ� �ر� �ȵ�");
            }
        }
    }

    public void CheckDashable()
    {
        //Dash ���·� ��ȯ
        if (moveInput != 0)
        {
            if (isDashUsed && Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("��� ��Ÿ�����Դϴ�.");
            }
            else if (!isDashUsed && Input.GetKeyDown(KeyCode.X))
            {
                ChangeState(State.Dash);
            }
        }
    }


    private void HandlePlayerDied()
    {
        //ChangeState(State.Dead);
    }

    private void HandlePlayerDamaged()
    {
        ChangeState(State.Damaged);
    }

    /// <summary>
    /// �÷��̾� �ʱ�ȭ �� ���� �۾�
    /// </summary>
    public void HandlePlayerSpawn()
    {
        //ChangeState(State.Spawn);
        // _playerUI.SetHp(playerModel.hp); // �ϴ� �ּ�ó��, �������� ������ �÷��̾�� �ؾ��Ҽ��� ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnDestroy()
    {
        UnsubscribeEvents();

        //if (_groundCheckRoutine != null)
        //    StopCoroutine(_groundCheckRoutine);

        if(_wallCheckRoutine != null)
            StopCoroutine(_wallCheckRoutine);
    }

    private void SubscribeEvents()
    {
        playerModel.OnPlayerDamageTaken += HandlePlayerDamaged;
        playerModel.OnPlayerDied += HandlePlayerDied;
        playerModel.OnPlayerSpawn += HandlePlayerSpawn;
        //playerModel.OnAbilityUnlocked += 
    }

    private void UnsubscribeEvents()
    {
        playerModel.OnPlayerDamageTaken -= HandlePlayerDamaged;
        playerModel.OnPlayerDied -= HandlePlayerDied;
        playerModel.OnPlayerSpawn -= HandlePlayerSpawn;
    }

    IEnumerator CheckWallDisplayRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        //while (true)
        //{
        //    Debug.DrawRay(_wallCheckPoint.position, Vector2.right * isPlayerRight * _wallCheckDistance, Color.red);
        //    isWall = Physics2D.Raycast(_wallCheckPoint.position, Vector2.right * isPlayerRight, _wallCheckDistance, wallLayerMask);
        //    yield return delay;
        //}

        //BoxCast�� ���� ���� üũ�� ������ ������
        while (true)
        {
            Vector2 origin = _wallCheckPoint.position;
            Vector2 direction = Vector2.right * isPlayerRight;
            Vector2 offset = direction * _wallCheckDistance;

            Vector2 topLeft = origin + (Vector2.up * _wallCheckBoxSize.y / 2) + (Vector2.left * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;
            Vector2 topRight = origin + (Vector2.up * _wallCheckBoxSize.y / 2) + (Vector2.right * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;
            Vector2 bottomLeft = origin + (Vector2.down * _wallCheckBoxSize.y / 2) + (Vector2.left * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;
            Vector2 bottomRight = origin + (Vector2.down * _wallCheckBoxSize.y / 2) + (Vector2.right * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;

            Debug.DrawLine(topLeft, topRight, Color.red);
            Debug.DrawLine(topRight, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, bottomLeft, Color.red);
            Debug.DrawLine(bottomLeft, topLeft, Color.red);

            //isWall = Physics2D.BoxCast(_wallCheckPoint.position, _wallCheckBoxSize, 0, Vector2.right * isPlayerRight, _wallCheckDistance, wallLayerMask);
            yield return delay;
        }

    }

    //public void Freeze()
    //{
    //    Invoke("DelayWallJump", 0.3f);
    //}

    //public void DelayWallJump()
    //{
    //    isWallJumpUsed = false;
    //}


    // ���̾� �� üũ
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}

    //public void MoveInAir()
    //{
    //    float moveInput = Input.GetAxisRaw("Horizontal");

    //    Vector2 targetVelocity = rigid.velocity + new Vector2(moveInput * moveSpeed*Time.deltaTime, 0);
    //    targetVelocity = Vector2.ClampMagnitude(targetVelocity, maxMoveSpeedInAir); // �ӵ�����
    //    rigid.velocity = targetVelocity;

    //    FlipPlayer(moveInput);

    //    //���� ��������� ã�ƾ���
    //    isWall = Physics2D.BoxCast(_wallCheckPoint.position, _wallCheckBoxSize, 0, Vector2.right * isPlayerRight, _wallCheckDistance, wallLayerMask);

    //    if (isWall && _curState != State.WallJump)
    //    {
    //        if (moveInput == isPlayerRight && moveInput != 0)
    //        ChangeState(State.WallGrab);
    //    }
    //}

    //IEnumerator CheckGroundRayRoutine()
    //{
    //    WaitForSeconds delay = new WaitForSeconds(0.1f);
    //    while (true)
    //    {

    //        //Debug.DrawRay(_groundCheckPoint.position, Vector2.down * _groundCheckDistance, Color.green);
    //        //isGrounded = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckDistance,groundLayerMask); //_rayPoint.up * -1
    //        yield return delay;
    //    }
    //}
}


