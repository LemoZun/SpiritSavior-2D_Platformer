using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RunState : PlayerState
{
    //float inputX;
    public RunState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Run;
    }

    public override void Enter()
    {
        //Debug.Log("Run 상태 진입");
        
        player.playerView.PlayAnimation(animationIndex);
        player.playerModel.RunPlayerEvent();
        //player.isGrounded = true; // 이건 이제 없어도 되지 않을까?
    }
    public override void Update()
    {
        //InputMove();
        Run();

        PlayAnimationInUpdate();
        // Idle 상태로 전환

        if (Mathf.Abs(player.rigid.velocity.x) < 0.01f)
        {
            player.ChangeState(PlayerController.State.Idle);
        }
        else if (player.coyoteTimeCounter <= 0 && player.rigid.velocity.y < 0f) //!player.isGrounded &&
        {
            player.ChangeState(PlayerController.State.Fall);
        }


        // Jump 상태로 전환
        else if (Input.GetKeyDown(KeyCode.C))//&& player.isGrounded) // 조건 나중에 뺄 수도 있음
        {
            player.ChangeState(PlayerController.State.Jump);
        }
        //Dash 상태로 전환
        player.CheckDashable();

        ////Dash 상태로 전환
        //if (player.isDashUsed && Input.GetKeyDown(KeyCode.X))
        //{
        //    Debug.Log("대시 쿨타임중입니다.");
        //}
        //else if(!player.isDashUsed && Input.GetKeyDown(KeyCode.X))
        //{
        //    player.ChangeState(PlayerController.State.Dash);
        //}

        ////Fall 상태로 전환 // 경사면은 어떻게?
        //if (player.rigid.velocity.y < 0)
        //{
        //    Debug.Log(player.rigid.velocity.y);
        //    player.ChangeState(PlayerController.State.Fall);
        //}

        //임시 피격 트리거
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    player.ChangeState(PlayerController.State.Damaged);
        //}


    }

    //public override void FixedUpdate()
    //{
    //    
    //}

    //private void InputMove()
    //{
    //    //inputX = Input.GetAxisRaw("Horizontal");
    //}

    private void Run()
    {
        player.moveInput = Input.GetAxisRaw("Horizontal");

        if (player.isSlope && player.isGrounded && player.groundAngle < player.maxAngle) //slope면 ground라 나중에 조건 간단히 변경
        {
            player.rigid.velocity = player.perpAngle * player.moveSpeed * player.moveInput * -1.0f;
        }
        else if (!player.isSlope && player.isGrounded)
        {
            player.rigid.velocity = new Vector2(player.moveInput * player.moveSpeed, player.rigid.velocity.y);
        }

        player.FlipPlayer(player.moveInput);
    }

    public override void Exit()
    {
        //Debug.Log("Run 상태 종료");
    }
}
