using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : PlayerState
{
    private float _knockbackForce;
    //private float _minKnockback = 0.5f;
    private Vector2 knockbackDirection;
    public DamagedState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Damaged;
        this._knockbackForce = player.knockbackForce;
    }

    public override void Enter()
    {
        Debug.Log("피격 상태 진입");

        player.rigid.sharedMaterial.friction = 0.6f;

        KnockbackPlayer();

        //무적상태 
        player.playerModel.invincibility = true;
        
        player.playerView.PlayAnimation(animationIndex);

        //모델에서 이미 업데이트 된 hp를 받아옴
        player.hp = player.playerModel.hp;
    }

    public override void Update()
    {
        // 피격상태가 끝나는걸 확인
        if(player.rigid.velocity.magnitude < 0.1f) // 넉백의 힘이 거의 사라졌을 때
        {
            if (player.playerModel.hp > 0)
            {
                player.ChangeState(PlayerController.State.WakeUp);
            }
            else
            {
                player.ChangeState(PlayerController.State.Dead);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("피격 상태 탈출");
        knockbackDirection = Vector2.zero;
    }

    private void KnockbackPlayer()
    {
        //넉백될 방향 정의
        knockbackDirection = -player.rigid.velocity.normalized;

        //기존의 운동량 초기화
        player.rigid.velocity = Vector2.zero;

        //넉백
        player.rigid.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
    }

}
