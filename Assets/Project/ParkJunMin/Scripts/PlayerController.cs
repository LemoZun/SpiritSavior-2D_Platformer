using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public enum State {Idle, Run, Jump, DoubleJump, Fall, WallGrab, WallSliding, WallJump, Damaged, WakeUp, Dead, Spawn, Size}
    [SerializeField] State _curState = State.Spawn;
    private BaseState[] _states = new BaseState[(int)State.Size];

    public PlayerModel playerModel = new PlayerModel();
    public PlayerView playerView;

    

    public SpriteRenderer renderer;
    [Header("Player Setting")]
    public float moveSpeed;        // �̵��ӵ�
    public float maxMoveSpeed;     // �̵��ӵ��� �ִ밪
    public float lowJumpForce;     // �������� ��
    public float highJumpForce;    // �������� ��
    public float maxJumpTime;     // �ִ����� �ð�
    public float jumpStartSpeed;   // �������� �ӵ�
    public float jumpEndSpeed;     // �������� �ӵ�
    public float doubleJumpForce; // ���� ������ �󸶳� ���� �ö��� ����
    public float knockbackForce; // �ǰݽ� �󸶳� �ڷ� �з��� �� ����
    

    //�⺻ �̵��ӵ��� ���� ��ȭ�Ǵ� ���� ����x
    [HideInInspector] public float moveSpeedInAir;    // ���߿��� �÷��̾��� �ӵ�
    [HideInInspector] public float maxMoveSpeedInAir; // ���߿��� �÷��̾��� �ӵ��� �ִ밪

    [Header("SpeedInAir = SpeedInGround * x")]
    public float speedAdjustmentOffsetInAir; // ���߿����� �ӵ� = �������� �ӵ� * �ش� ����

    [Header("Checking")]
    public Rigidbody2D rigid;
    public float hp;
    
    public bool isJumped = false;          // �������������� üũ
    public float jumpChargingTime = 0f;     // �����̽��� �����ð� üũ
    public bool isDoubleJumpUsed; // �������� ��� ������ ��Ÿ���� ����
    public bool isDead = false; // �׾����� Ȯ��
    

    [Header("Ground & Wall Checking")]
    [SerializeField] Transform _groundCheckPoint;
    public Transform _wallCheckPoint;
    private float _wallCheckDistance = 0.15f;
    private float _groundCheckDistance = 0.2f;
    public int isPlayerRight = 1;
    public bool isGrounded = false;        // ĳ���Ͱ� ���� �پ��ִ��� üũ
    [SerializeField] private bool _isWall;                  // ĳ���Ͱ� ���� �پ��ִ��� üũ
    public float wallSlidingSpeed = 0.5f; // �߷°�� �������� ���� �����ؾ���
    public float wallJumpPower;
    //public LayerMask wallLayer; // ��� ���� Ȯ��ġ ����
    //public Vector2 wallCheckSize;
    Coroutine _wallCheckRoutine;
    Coroutine _groundCheckRoutine;




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

        _states[(int)State.Idle] = new IdleState(this);
        _states[(int)State.Run] = new RunState(this);
        _states[(int)State.Jump] = new JumpState(this);
        _states[(int)State.DoubleJump] = new DoubleJumpState(this);
        _states[(int)State.Fall] = new FallState(this);
        _states[(int)State.WallGrab] = new WallGrabState(this);
        _states[(int)State.WallSliding] = new WallSlidingState(this);
        _states[(int)State.WallJump] = new WallJumpState(this);
        _states[(int)State.Damaged] = new DamagedState(this, knockbackForce);
        _states[(int)State.WakeUp] = new WakeupState(this);
        _states[(int)State.Dead] = new DeadState(this);
        _states[(int)State.Spawn] = new SpawnState(this);
        moveSpeedInAir = moveSpeed * speedAdjustmentOffsetInAir;
        maxMoveSpeedInAir = maxMoveSpeed * speedAdjustmentOffsetInAir;
        

        if (_groundCheckPoint == null)
            _groundCheckPoint = transform.Find("BottomPivot");

        if (_wallCheckPoint == null)
            _wallCheckPoint = transform.Find("WallCheckPoint");

        if (_groundCheckRoutine == null)
            _groundCheckRoutine = StartCoroutine(CheckGroundRayRoutine());

        //if (_wallCheckRoutine == null) // �ۼ���
        //    _wallCheckRoutine = StartCoroutine(CheckWallRoutine());

        //�ӽ� ü�� Ȯ�ο�
        hp = playerModel.hp;
    }


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerView = GetComponent<PlayerView>();
        _states[(int)_curState].Enter();
        SubscribeEvents();
    }

    void Update()
    {
        _states[(int)_curState].Update();
        TagePlayer();

        ////�ӽ� �ǰ� Ʈ����
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    playerModel.TakeDamage(1); // �ӽ�
        //}

        ////�ӽ� ���� Ʈ����
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    playerModel.DiePlayer();
        //    Debug.Log("����");
        //}

        //�ӽ� ü�� Ȯ�ο�
        //hp = playerModel.hp;

    }

    public void ChangeState(State nextState)
    {
        _states[(int)_curState].Exit();
        _curState = nextState;
        _states[(int)_curState].Enter();
    }



    public void MoveInAir()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rigid.velocity = new Vector2(moveInput * moveSpeedInAir, rigid.velocity.y);

        if (rigid.velocity.x > maxMoveSpeedInAir)
        {
            rigid.velocity = new Vector2(maxMoveSpeedInAir, rigid.velocity.y);
        }
        else if (rigid.velocity.x < -maxMoveSpeedInAir)
        {
            rigid.velocity = new Vector2(-(maxMoveSpeedInAir), rigid.velocity.y);
        }

        playerView.FlipRender(moveInput);

        if (_isWall)
            ChangeState(State.WallGrab);
    }

    public void TagePlayer()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            playerView.ChangeSprite(); // ��� �ִϸ��̼� ��� ���¶� ��� ����
            playerModel.TagPlayerEvent(); // �Ӽ� ������ ������ curNature�� �ٲ��� + �±� �̺�Ʈ Invoke
        }
    }

    private void HandlePlayerDied()
    {
        isDead = true;
        ChangeState(State.Dead);
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
        ChangeState(State.Spawn);
        // _playerUI.SetHp(playerModel.hp); // �ϴ� �ּ�ó��, �������� ������ �÷��̾�� �ؾ��Ҽ��� ����
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();

        if (_groundCheckRoutine != null)
            StopCoroutine(_groundCheckRoutine);

        if(_wallCheckRoutine != null)
            StopCoroutine(_wallCheckRoutine);
    }

    private void SubscribeEvents()
    {
        playerModel.OnPlayerDamageTaken += HandlePlayerDamaged;
        playerModel.OnPlayerDied += HandlePlayerDied;
        playerModel.OnPlayerSpawn += HandlePlayerSpawn;
    }

    private void UnsubscribeEvents()
    {
        playerModel.OnPlayerDamageTaken -= HandlePlayerDamaged;
        playerModel.OnPlayerDied -= HandlePlayerDied;
        playerModel.OnPlayerSpawn -= HandlePlayerSpawn;
    }

    IEnumerator CheckGroundRayRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        while (true)
        {
            Debug.DrawRay(_groundCheckPoint.position, Vector2.down * _groundCheckDistance, Color.green);
            isGrounded = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckDistance); //_rayPoint.up * -1
            yield return delay;
        }
    }

    IEnumerator CheckWallRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        while (true)
        {
            Debug.DrawRay(_wallCheckPoint.position, Vector2.right * isPlayerRight * _wallCheckDistance, Color.green);
            _isWall = Physics2D.Raycast(_wallCheckPoint.position, Vector2.right * isPlayerRight, _wallCheckDistance);
            yield return delay;
        }
    }

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
}


