using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : PlayerState
{
    

    public DeadState(PlayerController player) : base(player)
    {
    }

    

    public override void Enter()
    {
        animationIndex = (int)PlayerController.State.Dead;
        player.playerView.PlayAnimation(animationIndex);
        
        //player.playerModel.DiePlayer(); // ������ �̺�Ʈ ������ ��������� ���� ����
        //�ִϸ��̼� ������ �ڵ� ������
    }

    public override void Update()
    {
        if(player.playerView.IsAnimationFinished())
        { 
            player.ChangeState(PlayerController.State.Spawn);
        }
            
    }

    public override void Exit()
    { 

    }
}
