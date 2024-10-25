using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum State {Idle, Run, Jump, Fall, Size}
    [SerializeField] State _curState = State.Idle;
    private BaseState[] _states = new BaseState[(int)State.Size];

    public PlayerModel playerModel;
    public PlayerView playerView;
    

    public SpriteRenderer renderer;

    public float moveSpeed;        // �̵��ӵ�
    public float maxMoveSpeed;
    public float lowJumpForce;     // �������� ��
    public float highJumpForce;    // �������� ��
    public float maxJumpTime;     // �ִ����� �ð�
    public float jumpStartSpeed;   // �������� �ӵ�
    public float jumpEndSpeed;     // �������� �ӵ�

    //�⺻ �̵��ӵ��� ���� ��ȭ�Ǵ� ���� ����x
    [HideInInspector] public float moveSpeedInAir;    // ���߿��� �÷��̾��� �ӵ�
    [HideInInspector] public float maxMoveSpeedInAir; // ���߿��� �÷��̾��� �ӵ��� �ִ밪

    public float speedAdjustmentOffsetInAir; // ���߿����� �ӵ� = �������� �ӵ� * �ش� ����
    public Rigidbody2D rigid;
    public bool isGrounded = false;        // ĳ���Ϳ� ������ üũ
    public bool isJumped = false;          // �������������� üũ
    public float jumpChargingTime = 0f;     // �����̽��� �����ð� üũ

    private void Awake()
    {
        playerModel = new PlayerModel();
        if(playerModel == null)
        {
            Debug.LogError("�� ���� ����");
        }

        _states[(int)State.Idle] = new IdleState(this);
        _states[(int)State.Run] = new RunState(this);
        _states[(int)State.Jump] = new JumpState(this);
        _states[(int)State.Fall] = new FallState(this);
        moveSpeedInAir = moveSpeed * speedAdjustmentOffsetInAir;
        maxMoveSpeedInAir = maxMoveSpeed * speedAdjustmentOffsetInAir;
    }


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerView = GetComponent<PlayerView>();
        

        _states[(int)_curState].Enter();
    }

    void Update()
    {
        _states[(int)_curState].Update();
        TagePlayer();
        
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
            playerModel.TagElement(); // �Ӽ� ������ ������ curNature�� �ٲ���
            //playerView.PlayAnimation((int)_curState);
        }
    }

    private void OnDestroy()
    {
       
    }

    // ���̾� �� üũ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}


