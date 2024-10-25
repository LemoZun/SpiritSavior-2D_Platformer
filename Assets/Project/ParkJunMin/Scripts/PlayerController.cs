using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State {Idle, Run, Jump, Fall, Size}
    [SerializeField] State _curState = State.Idle;
    private BaseState[] _states = new BaseState[(int)State.Size];

    public SpriteRenderer renderer;

    [SerializeField] public float moveSpeed;        // �̵��ӵ�
    [SerializeField] public float maxMoveSpeed;
    [SerializeField] public float lowJumpForce = 10f;     // �������� ��
    [SerializeField] public float highJumpForce = 25f;    // �������� ��
    [SerializeField] public float maxJumpTime = 0.2f;     // �ִ����� �ð�
    [SerializeField] public float jumpStartSpeed = 18f;   // �������� �ӵ�
    [SerializeField] public float jumpEndSpeed = 10f;     // �������� �ӵ�

    public Rigidbody2D rigid;
    public bool isGrounded = false;        // ĳ���Ϳ� ������ üũ
    public bool isJumped = false;          // �������������� üũ
    public float spacebarTime = 0f;     // �����̽��� �����ð� üũ

    private void Awake()
    {
        _states[(int)State.Idle] = new IdleState(this);
        _states[(int)State.Run] = new RunState(this);
        _states[(int)State.Jump] = new JumpState(this);
        _states[(int)State.Fall] = new FallState(this);
    }


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        _states[(int)_curState].Enter();
    }

    void Update()
    {
        _states[(int)_curState].Update();
    }

    public void ChangeState(State nextState)
    {
        _states[(int)_curState].Exit();
        _curState = nextState;
        _states[(int)_curState].Enter();
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


