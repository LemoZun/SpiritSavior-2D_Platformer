using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SpawnState : PlayerState
{
    public SpawnState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        animationIndex = (int)PlayerController.State.Spawn;
        player.playerView.PlayAnimation(animationIndex);
        player.playerModel.SpawnPlayerEvent();
        ResetPlayer();
        

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
        //player.hasJumped = false;
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
        //player.playerModel.curNature = PlayerModel.Nature.Red;
        player.playerModel.hp = player.playerModel.curMaxHP;

    }

}
