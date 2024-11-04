using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static SoundData;

public partial class PlayerController : MonoBehaviour
{
    public enum State {Idle, Run, Dash, Jump, DoubleJump, Fall, Land, WallGrab, WallSliding, WallJump, Damaged, WakeUp, Dead, Spawn, Size}
    //public PlayerData playerData;
    [SerializeField] State _curState;
    private PlayerState[] _states = new PlayerState[(int)State.Size];

    [HideInInspector] public PlayerModel.Ability unlockedAbilities = PlayerModel.Ability.None;

    [HideInInspector] public PlayerModel playerModel = new PlayerModel();
    [HideInInspector] public PlayerView playerView;

    private Collider2D _playerCollider;
    public int _groundLayerMask;
    private int _wallLayerMask;
    private int _ignorePlayerLayerMask;

    //public SpriteRenderer renderer;
    [Header("Player Setting")]
    public float moveSpeed;        // �̵��ӵ�
    public float dashForce;         // ��� ��
    public float dashCoolTime; // ��� ��� �� ��Ÿ��
    public float jumpForce;    // �������� ��
    public float doubleJumpForce; // ���� ������ �󸶳� ���� �ö��� ����
    public float knockbackForce; // �ǰݽ� �󸶳� �ڷ� �з��� �� ����
    public float wallJumpPower; // ������ ��
    public float maxAngle; // �̵� ������ �ִ� ����
    public float speedAdjustmentOffsetInAir; // ���߿����� �ӵ� = �������� �ӵ� * �ش� ����
    
    // "SpeedInAir = SpeedInGround * x")
    [HideInInspector] public float moveSpeedInAir;    // ���߿��� �÷��̾��� �ӵ�
    //�⺻ �̵��ӵ��� ���� ��ȭ�Ǵ� ���� ����x

    [Space(30)]
    [Header("Checking")]
    public bool isDoubleJumpUsed; // �������� ��� ������ ��Ÿ���� ����
    public bool isDashUsed; // ��ø� ����ߴ��� ������ ��Ÿ���� ����
    
    public bool isDead = false; // �׾����� Ȯ��
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public float hp;
    [HideInInspector] public float dashDeltaTime;
    
    [Space(30)]
    [Header("Ground & Slope & Wall Checking")]
    public Transform bottomPivot;

    [SerializeField] Transform _groundCheckPoint1;
    [SerializeField] Transform _groundCheckPoint2;

    //[SerializeField] Vector2 _originalCheckPoint1;
    //[SerializeField] Vector2 _originalCheckPoint2;
    //private Vector2 _changedPoint1;
    //private Vector2 _changedPoint2;


    public Transform _wallCheckPoint;
    private float _wallCheckDistance = 0.01f;
    [SerializeField] private float _wallCheckHeight; //2.25f; // �ʹ� ��� ��絵 ������ �ν���
    [SerializeField] private float _groundCheckDistance;
    public float groundAngle;
    public int isPlayerRight = 1;
    public bool isGrounded;        // ĳ���Ͱ� ���� �پ��ִ��� üũ
    [HideInInspector] public Vector2 perpAngle;
    [HideInInspector] public bool isSlope;
    [HideInInspector] public RaycastHit2D groundHit1;
    [HideInInspector] public RaycastHit2D groundHit2;
    [HideInInspector] public RaycastHit2D chosenHit;
    [HideInInspector] public RaycastHit2D wallHit;
    [HideInInspector] public RaycastHit2D[] boxHits;
    public bool isWall;                  // ĳ���Ͱ� ���� �پ��ִ��� üũ
    public bool isWallJumpUsed;         // ������ �������� ��� �ߴ��� üũ
    private Vector2 _wallCheckBoxSize;
    Coroutine _wallCheckDisplayRoutine;

    [Header("Input")]
    [HideInInspector] public float moveInput;
    // �ڿ��� Ÿ��
    [HideInInspector] public float coyoteTime = 0.2f;
    [HideInInspector] public float coyoteTimeCounter;
    //���� ����
    [HideInInspector] public float jumpBufferTime = 0.2f;
    [SerializeField] public float jumpBufferCounter;

    /*
    //���
    [HideInInspector] public float lowJumpForce;     // �������� ��
    [HideInInspector] public float maxMoveSpeed;     // �̵��ӵ��� �ִ밪
    [HideInInspector] public float maxJumpTime;     // �ִ����� �ð�
    [HideInInspector] public float slopeJumpBoost; // ���鿡���� �߰� ���� ������ �� // ���
    [HideInInspector] public float jumpCirticalPoint; // ��������, ���������� ������ ���� // ���
    [HideInInspector] public float maxMoveSpeedInAir; // ���߿��� �÷��̾��� �ӵ��� �ִ밪
    [HideInInspector] public float jumpChargingTime = 0f;     // �����̽��� �����ð� üũ
    [HideInInspector] public float maxFlightTime; // ���� �� �ٷ� fall ���·� ���� �ʱ� ���� ����
    [HideInInspector] public RaycastHit2D slopeHit;
    Coroutine _groundCheckRoutine;
    [SerializeField] private float _slopeCheckDistance;
    public bool isStuck; // ���� �������� Ȯ��
    */
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


        if (bottomPivot == null)
            bottomPivot = transform.Find("BottomPivot");

        if (_groundCheckPoint1 == null)
            _groundCheckPoint1 = transform.Find("GroundCheckPoint1");

        if (_groundCheckPoint2 == null)
            _groundCheckPoint2 = transform.Find("GroundCheckPoint2");

        if (_wallCheckPoint == null)
            _wallCheckPoint = transform.Find("WallCheckPoint");

        if (_wallCheckDisplayRoutine == null) // �ۼ���
            _wallCheckDisplayRoutine = StartCoroutine(CheckWallDisplayRoutine());

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
        _wallLayerMask = LayerMask.GetMask("Wall");
        _groundLayerMask = LayerMask.GetMask("Ground");
        _ignorePlayerLayerMask = LayerMask.GetMask("Ignore Player");
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
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    playerModel.TakeDamageEvent(1); // �ӽ�
        //}

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

        groundHit1 = Physics2D.Raycast(_groundCheckPoint1.position, Vector2.down, _groundCheckDistance, _groundLayerMask);
        groundHit2 = Physics2D.Raycast(_groundCheckPoint2.position, Vector2.down, _groundCheckDistance, _groundLayerMask);
        
        Debug.DrawLine(_groundCheckPoint1.position, (Vector2)_groundCheckPoint1.position + Vector2.down * _groundCheckDistance, Color.cyan);
        Debug.DrawLine(_groundCheckPoint2.position, (Vector2)_groundCheckPoint2.position + Vector2.down * _groundCheckDistance, Color.yellow);
        //slopeHit = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _slopeCheckDistance, groundLayerMask);
        //��ֺ��ͷ� ������ ����


        if (groundHit1 || groundHit2)
        {
            isGrounded = true;
            if(groundHit1 && groundHit2)
            {
                //�� �� �� distance�� �� ª�� ray�� ����
                chosenHit = groundHit1.distance <= groundHit2.distance ? groundHit1 : groundHit2;
            }
            else
            {
                chosenHit = groundHit1 ? groundHit1 : groundHit2;
            }

        }
        else
        {
            isGrounded = false;
        }

        // Vector2.Perpendicular(Vector2 A) : A�� ������ �ݽð� �������� 90�� ȸ���� ���Ͱ��� ��ȯ

        if(isGrounded)
        {
            //if (rigid.sharedMaterial.friction != 0.6f)
            //    rigid.sharedMaterial.friction = 0.6f;

            perpAngle = Vector2.Perpendicular(chosenHit.normal).normalized; // 
            groundAngle = Vector2.Angle(chosenHit.normal, Vector2.up);

            if(groundAngle != 0)
                isSlope = true;
            else
                isSlope = false;


            if(groundAngle > maxAngle)
            {
                moveInput = 0;
            }
            else
            {
                //Debug.Log(groundAngle);
            }


            //��������, ���鿡�� ����
            Debug.DrawLine(chosenHit.point, chosenHit.point + chosenHit.normal, Color.blue);

            // ���������� ������ ����, ����
            Debug.DrawLine(chosenHit.point, chosenHit.point + perpAngle, Color.red);

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

        if ((_ignorePlayerLayerMask & (1 << wallHit.collider.gameObject.layer)) != 0)
        {
            return;
        }

        if (HasAbility(PlayerModel.Ability.WallJump) && (_wallLayerMask & (1 << wallHit.collider.gameObject.layer)) != 0)// ��Ÿ�� ������ ���� ���
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

    public void AdjustDash()
    {
        boxHits = Physics2D.BoxCastAll(_wallCheckPoint.position, _wallCheckBoxSize, 0, Vector2.right * isPlayerRight, _wallCheckDistance);
        if (boxHits.Length > 0)
        {
            bool allHits = false;
            float closetDistance = float.MaxValue;
            RaycastHit2D closetHit = new RaycastHit2D();
            foreach (RaycastHit2D hit in boxHits)
            {
                if (hit.distance < closetDistance)
                {
                    closetDistance = hit.distance;
                    closetHit = hit;
                }
            }

            if(closetHit.collider != null)
            {
                if(!isGrounded)
                {
                    Vector2 curPosition = transform.position;
                    Vector2 hitPositon = closetHit.point;

                    if(curPosition.y != hitPositon.y)
                    {
                        transform.position = new Vector2(curPosition.x, Mathf.Lerp(curPosition.y, hitPositon.y,0.5f));
                    }
                }
            }
            else
            {
                Debug.Log("���� closetHit ����");
            }
             

        }
    }

    public void MoveInAir()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveInput * moveSpeedInAir, rigid.velocity.y);
        FlipPlayer(moveInput);
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
        AdjustColliderOffset();
        AdjustCheckPoint();
    }

    private void AdjustCheckPoint()
    {
        _groundCheckPoint1.localPosition = new Vector2(Mathf.Abs(_groundCheckPoint1.localPosition.x) * -isPlayerRight, _groundCheckPoint1.localPosition.y);
        _groundCheckPoint2.localPosition = new Vector2(Mathf.Abs(_groundCheckPoint2.localPosition.x) * isPlayerRight, _groundCheckPoint2.localPosition.y);
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

        if(_wallCheckDisplayRoutine != null)
            StopCoroutine(_wallCheckDisplayRoutine);
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


