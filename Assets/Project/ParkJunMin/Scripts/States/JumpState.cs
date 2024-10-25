using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    private int idleAnimationIndex = (int)PlayerController.State.Jump;
    public JumpState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        Debug.Log("���� ���� ����");
        player.playerView.PlayAnimation(idleAnimationIndex);
        player.isJumped = true;
        player.jumpChargingTime = 0f;
        player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.lowJumpForce); // 1������
    }

    public override void Update()
    {                                                                        // �����̽��ٸ� ������ ���� ������ ����
        if (Input.GetKey(KeyCode.Space) && player.isJumped)
        {
            player.jumpChargingTime += Time.deltaTime;

            if (player.jumpChargingTime < player.maxJumpTime && player.rigid.velocity.y > 0)  // ��� �� �߰� ������
            {
                float jumpForce = Mathf.Lerp(player.lowJumpForce, player.highJumpForce, player.jumpChargingTime / player.maxJumpTime);
                player.rigid.velocity = new Vector2(player.rigid.velocity.x, jumpForce);  // ���� ����
            }
            else
            {
                player.isJumped = false;
            }
        }

        // �����̽��� ���� ���� ����
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.isJumped = false;
        }

        // ���� �ӵ��� ������
        if (player.rigid.velocity.y > 0)  // ĳ���Ͱ� ��� ��
        {
            player.rigid.velocity += Vector2.up * Physics2D.gravity.y * (player.jumpStartSpeed - 1) * Time.deltaTime;
        }

        player.MoveInAir();


        if (player.rigid.velocity.y < 0)
        {
            Debug.Log(player.rigid.velocity.y);
            player.ChangeState(PlayerController.State.Fall);
        }
            
    }

    public override void Exit()
    {
        Debug.Log("���� ���� ����");
    }

}
