using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    private bool _hasJumped;
    public JumpState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Jump;
    }

    public override void Enter()
    {
        //Debug.Log("���� ���� ����");
        
        player.playerView.PlayAnimation(animationIndex);
        player.playerModel.JumpPlayerEvent();
        _hasJumped = true;
        player.jumpChargingTime = 0f;
        //player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.lowJumpForce); // 1������
    }

    public override void Update()
    {   
        PlayAnimationInUpdate();
        if (Input.GetKey(KeyCode.Space) && _hasJumped) // �����̽��ٸ� ������ ���� ������ ����
        {
            player.jumpChargingTime += Time.deltaTime;
            if(player.jumpChargingTime >= player.jumpCirticalPoint)
            {
                // ���������� �����ɸ�ŭ �����̽��ٸ� �Ӱ� �ð� �̻� �������
                player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.highJumpForce);
                _hasJumped = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && _hasJumped)
        {
            // �������� ����
            player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.lowJumpForce);
            _hasJumped = false;
        }

        player.MoveInAir();

        // ���� ���¿��� ���������� ���º�ȯ
        if (!player.isDoubleJumpUsed && Input.GetKeyDown(KeyCode.Space))
        {
            player.ChangeState(PlayerController.State.DoubleJump);
        }


        if (player.rigid.velocity.y < 0)
        {
            //Debug.Log(player.rigid.velocity.y);
            player.ChangeState(PlayerController.State.Fall);
        }

        //if(player.isGrounded)
        //{
        //    player.ChangeState(PlayerController.State.Idle);
        //}
            
    }

    public override void Exit()
    {
        //Debug.Log("���� ���� ����");
    }

}
