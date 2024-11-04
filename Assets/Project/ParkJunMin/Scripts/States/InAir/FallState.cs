using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallState : PlayerState
{
    private bool _isFalling;
    public FallState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Fall;
    }

    public override void Enter()
    {
        // ������ �� ���� ��������
        // ĳ���Ͱ� �ϰ���
        //Debug.Log("Fall ���� ����");
        
        player.playerView.PlayAnimation(animationIndex);
        _isFalling = true;
        //player.rigid.gravityScale = 5;
        //player.rigid.velocity += Vector2.up * Physics2D.gravity.y * (player.jumpEndSpeed - 1) * Time.deltaTime;
    }
    
    public override void Update()
    {
        PlayAnimationInUpdate();
        player.MoveInAir();

        // �������� ���¿��� ���������� ���º�ȯ (���������� �Ƚ��� ���)
        if (!player.isDoubleJumpUsed && Input.GetKeyDown(KeyCode.C))
        {
            player.ChangeState(PlayerController.State.DoubleJump);
        }
        
        //if(player.isSlope && player.rigid.velocity.y > 0.01f) // ���� �̻��� �Ʒ��� ��� �� ���ľ���
        //{
        //    player.ChangeState(PlayerController.State.Land);
        //}

        if(player.isGrounded)
        {
            if (player.isSlope)
            {
                if (player.groundAngle < player.maxAngle)
                {
                    player.ChangeState(PlayerController.State.Land);
                }
                else
                {
                    if(player.rigid.velocity.y >= 0) // �� �̲���������
                    {
                        player.ChangeState(PlayerController.State.Land);
                    }
                }
            }
            else
            {
                player.ChangeState(PlayerController.State.Land);
            }
        }


        //if (player.isGrounded)// && player.rigid.velocity.y < -0.01f) //player.coyoteTimeCounter > 0) //
        //{
        //    player.ChangeState(PlayerController.State.Land);
        //}
    }

    public override void Exit()
    {
        _isFalling = false;

        // ���� ���� ������ �����ϱ� ���� ��Ÿ�� �Ұ����� ������ 0���� �ְ�
        // �� ���¸� ����� �ٽ� ���󺹱� ���ְ������ 
        // ���� enter������ �� �־��ִ� ��� �ܿ� �� ���� ����� ������?
        if(player.rigid.sharedMaterial != null)
        {
            //player.rigid.sharedMaterial.friction = 0.6f;
        }
        


        // player.rigid.gravityScale = 1;
    }
}
