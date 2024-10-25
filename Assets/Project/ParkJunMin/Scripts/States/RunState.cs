using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player)
    {

    }

    public override void Enter()
    {
        Debug.Log("Run 상태 진입");
        player.isGrounded = true;
    }
    public override void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        player.rigid.velocity = new Vector2(moveInput * player.moveSpeed, player.rigid.velocity.y);

        if (player.rigid.velocity.x > player.maxMoveSpeed)
        {
            player.rigid.velocity = new Vector2(player.maxMoveSpeed, player.rigid.velocity.y);
        }
        else if (player.rigid.velocity.x < -player.maxMoveSpeed)
        {
            player.rigid.velocity = new Vector2(-(player.maxMoveSpeed), player.rigid.velocity.y);
        }

        if (moveInput < 0)
            player.renderer.flipX = true;
        if (moveInput > 0)
            player.renderer.flipX = false;

        // Idle 상태로 전환
        if (Mathf.Abs(player.rigid.velocity.x) < 0.01f)
        {
            player.ChangeState(PlayerController.State.Idle);
        }

        // Jump 상태로 전환
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded) // 조건 나중에 뺄 수도 있음
        {
            player.ChangeState(PlayerController.State.Jump);
        }
    }

    public override void Exit()
    {
        Debug.Log("Run 상태 종료");
    }
}
