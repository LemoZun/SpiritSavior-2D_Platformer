using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabState : PlayerState
{
    
    public WallGrabState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.WallGrab;
        //wallJump�� ���� ���ؼ� WallGrab�� �ʼ����̶� ���⿡ ��
        //wallGrab -> wallSliding -> wallJump ������ �����ϱ⶧���� wallGrab�� ��.
        ability = PlayerModel.Ability.WallJump;
    }

    public override void Enter()
    {
        player.isWallJumpUsed = false;
        player.playerView.PlayAnimation(animationIndex);
        player.playerModel.GrabWallEvent();
        //��ġ�� ������������� -> �߷��� ���� �ʰ�
        player.rigid.velocity = Vector2.zero;
        player.rigid.gravityScale = 0;
    }

    public override void Update()
    {
        PlayAnimationInUpdate();

        if (player.playerView.IsAnimationFinished())
        {
            player.ChangeState(PlayerController.State.WallSliding);
        }
    }

    public override void Exit()
    {
    }

}
