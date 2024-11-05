using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : PlayerState
{
    Vector2 dashDirection;
    public DashState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Dash;
        ability = PlayerModel.Ability.Dash;
    }

    public override void Enter()
    {
        player.isDashUsed = true;
        player.dashDeltaTime = 0;
        player.playerView.PlayAnimation(animationIndex);
        player.playerModel.DashPlayerEvent();

        dashDirection = new Vector2(player.moveInput, 0);

        player.rigid.gravityScale = 0;
        dashDirection = new Vector2(player.moveInput, 0);
        player.rigid.velocity = dashDirection * player.dashForce;

        if (player.isSlope) // �ϴ� ���鿡�� �ٸ��� ó���ϰ� �ٲܰ�츦 ����ؼ� ��������
        {
            dashDirection = new Vector2(player.moveInput, 0);
            player.rigid.velocity = dashDirection * (player.dashForce);
        }
        else
        {
            dashDirection = new Vector2(player.moveInput, 0);
            player.rigid.velocity = dashDirection * player.dashForce;
        }
    }

    public override void Update()
    {
        PlayAnimationInUpdate();

        if (player.playerView.IsAnimationFinished())
        {
            //Debug.Log("��� �ִϸ��̼� ����");
            player.ChangeState(PlayerController.State.Fall);
        }
        //player.AdjustDash();
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        player.rigid.gravityScale = 1;
    }


}
