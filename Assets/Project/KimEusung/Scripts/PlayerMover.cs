using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// teg 0 : �� Ground �߰�

public class PlayerMover : MonoBehaviour
{
    private float _moveSpeed = 10f;        // �̵��ӵ�
    private float _lowJumpForce = 10f;     // �������� ��
    private float _highJumpForce = 25f;    // �������� ��
    private float _maxJumpTime = 0.2f;     // �ִ����� �ð�
    private float _jumpStartSpeed = 18f;   // �������� �ӵ�
    private float _jumpEndSpeed = 10f;     // �������� �ӵ�
    
    private Rigidbody2D _rigid;
    private bool _iGround = false;        // ĳ���Ϳ� ������ üũ
    private bool _iJump = false;          // �������������� üũ
    private float _spacebarTime = 0f;     // �����̽��� �����ð� üũ

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �̵�
        float moveInput = Input.GetAxis("Horizontal");
        _rigid.velocity = new Vector2(moveInput * _moveSpeed, _rigid.velocity.y);

        // ���� ��ŸƮ
        if (Input.GetKeyDown(KeyCode.Space) && _iGround)
        {
            _iJump = true;
            _spacebarTime = 0f;
            _rigid.velocity = new Vector2(_rigid.velocity.x, _lowJumpForce); // 1������
        }

        // �����̽��ٸ� ������ ���� ������ ����
        if (Input.GetKey(KeyCode.Space) && _iJump)
        {
            _spacebarTime += Time.deltaTime;

            if (_spacebarTime < _maxJumpTime && _rigid.velocity.y > 0)  // ��� �� �߰� ������
            {
                float jumpForce = Mathf.Lerp(_lowJumpForce, _highJumpForce, _spacebarTime / _maxJumpTime);
                _rigid.velocity = new Vector2(_rigid.velocity.x, jumpForce);  // ���� ����
            }
        }

        // �����̽��� ���� ���� ����
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _iJump = false;
        }

        // ���� �ӵ��� ������
        if (_rigid.velocity.y > 0)  // ĳ���Ͱ� ��� ��
        {
            _rigid.velocity += Vector2.up * Physics2D.gravity.y * (_jumpStartSpeed - 1) * Time.deltaTime;
        }

        // �������� ���� �������� 
        if (_rigid.velocity.y < 0)  // ĳ���Ͱ� �ϰ� ��
        {
            _rigid.velocity += Vector2.up * Physics2D.gravity.y * (_jumpEndSpeed - 1) * Time.deltaTime;
        }
    }

    // �±� �� üũ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _iGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _iGround = false;
        }
    }
}