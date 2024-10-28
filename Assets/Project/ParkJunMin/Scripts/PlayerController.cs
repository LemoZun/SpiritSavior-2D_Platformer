using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public enum State {Idle, Run, Jump, DoubleJump, Fall, Damaged, Dead, Size}
    [SerializeField] State _curState = State.Idle;
    private BaseState[] _states = new BaseState[(int)State.Size];

    public PlayerModel playerModel = new PlayerModel();
    public PlayerView playerView;

    private Coroutine _checkGroundRayRoutine;
    [SerializeField] Transform _rayPoint;

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
    public bool isGrounded = false;        // ĳ���Ϳ� ������ üũ
    public bool isJumped = false;          // �������������� üũ
    public float jumpChargingTime = 0f;     // �����̽��� �����ð� üũ
    public bool isDoubleJumpUsed; // �������� ��� ������ ��Ÿ���� ����
    public bool isDead = false; // �׾����� Ȯ��
    
    public float hp;


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
        _states[(int)State.Damaged] = new DamagedState(this, knockbackForce);
        _states[(int)State.Dead] = new DeadState(this);
        moveSpeedInAir = moveSpeed * speedAdjustmentOffsetInAir;
        maxMoveSpeedInAir = maxMoveSpeed * speedAdjustmentOffsetInAir;
        

        if (_rayPoint == null)
            _rayPoint = transform.Find("BottomPivot");

        if (_checkGroundRayRoutine == null)
            _checkGroundRayRoutine = StartCoroutine(CheckGroundRayRoutine());

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

        //�ӽ� �ǰ� Ʈ����
        if (Input.GetKeyDown(KeyCode.O))
        {
            playerModel.TakeDamage(1); // �ӽ�
        }

        //�ӽ� ���� Ʈ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerModel.DiePlayer();
            Debug.Log("����");
        }

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
    }

    public void TagePlayer()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            playerView.ChangeSprite(); // ��� �ִϸ��̼� ��� ���¶� ��� ����
            playerModel.TagPlayer(); // �Ӽ� ������ ������ curNature�� �ٲ��� + �±� �̺�Ʈ Invoke
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


    private void OnDestroy()
    {
        UnsubscribeEvents();

        if (_checkGroundRayRoutine != null)
            StopCoroutine(_checkGroundRayRoutine);
    }

    private void SubscribeEvents()
    {
        playerModel.OnPlayerDamageTaken += HandlePlayerDamaged;
        playerModel.OnPlayerDied += HandlePlayerDied;
        playerModel.OnPlayerSpawn += SpawnPlayer;
    }

    private void UnsubscribeEvents()
    {
        playerModel.OnPlayerDamageTaken -= HandlePlayerDamaged;
        playerModel.OnPlayerDied -= HandlePlayerDied;
        playerModel.OnPlayerSpawn -= SpawnPlayer;
    }

    /// <summary>
    /// �÷��̾� �ʱ�ȭ �� ���� �۾�
    /// </summary>
    public void SpawnPlayer()
    {
        isDead = false;
        transform.position = Manager.Game.RespawnPoint;
        playerModel.curNature = PlayerModel.Nature.Red;
        ChangeState(State.Idle);
        playerModel.hp = playerModel.curMaxHP;
        // _playerUI.SetHp(playerModel.hp); // �ϴ� �ּ�ó��, �������� ������ �÷��̾�� �ؾ��Ҽ��� ����
    }

    IEnumerator CheckGroundRayRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);
        while (true)
        {
            Debug.DrawRay(_rayPoint.position, Vector2.down * 0.1f, Color.green);
            if (Physics2D.Raycast(_rayPoint.position, Vector2.down, 0.1f)) //_rayPoint.up * -1
            {
                isGrounded = true;
            }
            else
            {
                isGrounded= false;
            }
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


