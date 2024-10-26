using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Trap
{
    [SerializeField] GameObject _movingPlatform;
    PolygonCollider2D _movingPlatformCollider;

    [Header("�����Ӱ���")]
    [SerializeField] float _movingTime;
    [SerializeField] float _movingSpeed;
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
            // ��� 1���Ŀ� ��������
            // Destroy(gameObject,1f);
        }
    }

}
