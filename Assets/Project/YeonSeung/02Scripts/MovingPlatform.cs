using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Trap
{
    [SerializeField] GameObject _movingPlatform;
    PolygonCollider2D _movingPlatformCollider;

    // [SerializeField] float _movingTime; // �ð����� �����̴� ������� ���� ���Եȴٸ�
    [Header("�����Ӱ��� \t �ӵ�: ")]
    [SerializeField] float _movingSpeed;
    [Header("������ �Ÿ�: ")]
    [SerializeField] float _movingDistance;
    private float _movingDirection;

    [Header("�¿� = ��ĭ / ���Ʒ� = üũ")]
    [SerializeField] bool _fourWay;
    [Header("���� = ��ĭ / ������ = üũ")]
    [SerializeField] bool _horizontal; // F = ����(-) T = ������(+)
    [Header("�Ʒ� = ��ĭ / �� = üũ")]
    [SerializeField] bool _vertical; // F = ����(-) T = ������(+)


    Vector2 finalTarget;
    Vector2 target;
    private void Awake()
    {
        //_movingPlatform = GetComponent<GameObject>();
        _movingPlatformCollider = GetComponent<PolygonCollider2D>();

        if (_fourWay == false) // F = �¿�
        {

            // �¿� ������ Check
            if (_horizontal == false)
            {
                _movingDirection = -1;
            }
            else
            {
                _movingDirection = 1;
            }
        }
        else if (_fourWay == true)// ���Ʒ�
        {
            // ���Ʒ� ������ Check
            if (_vertical == false)
            {
                _movingDirection = -1;
            }
            else
            {
                _movingDirection = 1;
            }
        }
        else
            return;


     //   // �¿� ������ Check
     //   if (_horizontal == false)
     //   {
     //       _movingDirection = -1;
     //   }
     //   else
     //   {
     //       _movingDirection = 1;
     //   }

     //   // ���Ʒ� ������ Check
     //   if (_vertical == false)
     //   {
     //       _movingDirection = -1;
     //   }
     //   else
     //   {
     //       _movingDirection = 1;
     //   }



        SetDestinationX();
        SetDestinationY();

    }
    void Start()
    {
        
    }


    void Update()
    {
        
        _movingPlatform.transform.position = Vector2.MoveTowards(transform.position, target, _movingSpeed * Time.deltaTime);
    }
    private void SetDestination()
    {
        // Left or Right
        _movingDistance = _movingDirection * _movingDistance + transform.position.x;
        target = new Vector2(_movingDistance, transform.position.x);

    }
    private void SetDestinationX()
    {
        // Left or Right
        _movingDistance = _movingDirection * _movingDistance + transform.position.x;
        target = new Vector2(_movingDistance, transform.position.x);
    }

    private void SetDestinationY()
    {

        // UP or Down
        _movingDistance = _movingDirection * _movingDistance + transform.position.y;
        target = new Vector2(_movingDistance, transform.position.y);
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if(collision.gameObject.tag == "Player")
        {
            // ������(�浹�����Ǹ�) �θ� �Ͽ� �ּ� �̵� ����.
            collision.transform.SetParent(transform);

            // ��� 1���Ŀ� �������� ���߿� �� ���� ���� ���Ǵø��Եǰų� �ϸ�
            // Destroy(gameObject,1f);
        }
    }
    protected void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // ���˳����� �ڽ�����
            collision.transform.SetParent(null);
        }

    }

}
