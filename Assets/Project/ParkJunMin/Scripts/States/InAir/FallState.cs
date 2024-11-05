using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallState : PlayerState
{
    //private bool _isFalling;
    public FallState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Fall;
    }

    public override void Enter()
    {
        player.playerView.PlayAnimation(animationIndex);
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
        
        if(player.isGrounded)
        {
            if (player.isSlope)
            {
                // ����ε� �÷��̾ ���� �� �ִ� ����� ���
                if (player.groundAngle < player.maxAngle)
                {
                    player.ChangeState(PlayerController.State.Land);
                }
                else
                {
                    // ���� �� ���� ��翡�� �� �̲������� ���
                    if(player.rigid.velocity.y >= 0) 
                    {
                        player.ChangeState(PlayerController.State.Land);
                    }
                }
            }
            else
            {
                // ��簡 �ƴ� �ٴ��̸� Land
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
    }
}
