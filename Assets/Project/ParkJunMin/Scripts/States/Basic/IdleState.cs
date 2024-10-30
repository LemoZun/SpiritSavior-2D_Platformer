using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Idle;
        //player.rigid.sharedMaterial.friction = 1.0f;
    }

    public override void Enter()
    {
        
        prevNature = player.playerModel.curNature;
        player.isDoubleJumpUsed = false;
        //Debug.Log("Idle 상태 진입");
        player.playerView.PlayAnimation(animationIndex);
    }

    public override void Update()
    {
        PlayAnimationInUpdate();
        //if (prevNature != player.playerModel.curNature)
        //{
        //    player.playerView.PlayAnimation(animationIndex);
        //    prevNature = player.playerModel.curNature;
        //}

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
        {
            player.ChangeState(PlayerController.State.Run);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.ChangeState(PlayerController.State.Jump);
        }
    }

    public override void Exit()
    {
        //Debug.Log("Idle 상태 종료");
    }
}
