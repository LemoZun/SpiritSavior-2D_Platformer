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
    [Header("���� = ��ĭ / ������ üũ")]
    [SerializeField] bool _direction; // F = ����(-) T = ������(+)

    Vector2 target;
    private void Awake()
    {
        //_movingPlatform = GetComponent<GameObject>();
        _movingPlatformCollider = GetComponent<PolygonCollider2D>();
        if (_direction == false)
        {
            _movingDirection = -1;
        }
        else
        {
            _movingDirection = 1;
        }
        SetDestination();

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
        target = new Vector2(_movingDistance, transform.position.y);
        // �ö󰡴� ������ �ʿ��Ϸ���
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
