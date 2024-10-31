using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SpawnState : PlayerState
{
    public SpawnState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Spawn;
    }

    public override void Enter()
    {
        Debug.Log("����");
        ResetPlayer();
        player.playerView.PlayAnimation(animationIndex);
        
        player.playerModel.SpawnPlayerEvent();
        
    }

    public override void Update()
    {
        if(player.playerView.IsAnimationFinished())
        {
            player.ChangeState(PlayerController.State.Idle);
        }
    }
    public override void Exit()
    {

    }

    private void ResetPlayer()
    {
        prevNature = player.playerModel.curNature;
        player.playerModel.hp = player.playerModel.curMaxHP;
        player.isDead = false;
        player.playerModel.invincibility = false;

        if (Manager.Game.RespawnPoint != null )
        {
            player.transform.position = Manager.Game.RespawnPoint;
        }
        else
        {
            Debug.Log("������ ����Ʈ ����");
        }

        

    }

}
