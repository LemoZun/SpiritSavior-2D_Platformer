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
        player.rigid.sharedMaterial.friction = 0.6f;

        KnockbackPlayer();

        //�������� 
        player.playerModel.invincibility = true;
        
        player.playerView.PlayAnimation(animationIndex);

        //�𵨿��� �̹� ������Ʈ �� hp�� �޾ƿ�
        player.hp = player.playerModel.hp;
    }

    public override void Update()
    {
        // �ǰݻ��°� �����°� Ȯ��
        if(player.rigid.velocity.magnitude < 0.1f) // �˹��� ���� ���� ������� ��
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
        knockbackDirection = Vector2.zero;
    }

    private void KnockbackPlayer()
    {
        //�˹�� ���� ����
        knockbackDirection = -player.rigid.velocity.normalized;

        //������ ��� �ʱ�ȭ
        player.rigid.velocity = Vector2.zero;

        //�˹�
        player.rigid.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
    }

}
