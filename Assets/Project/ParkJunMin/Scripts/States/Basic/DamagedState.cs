using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamagedState : PlayerState
{
    private float _knockbackForce;
    //private float _minKnockback = 0.5f;
    private Vector2 knockbackDirection;
    private bool knockbackFlag;
    public DamagedState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Damaged;
        this._knockbackForce = player.knockbackForce;
    }

    public override void Enter()
    {
        knockbackFlag = true;
        if (player.rigid.sharedMaterial != null)
        {
            player.rigid.sharedMaterial.friction = 0.6f;
        }

        //�������� 
        player.playerModel.invincibility = true;
        
        player.playerView.PlayAnimation(animationIndex);

        //�𵨿��� �̹� ������Ʈ �� hp�� �޾ƿ�
        player.hp = player.playerModel.hp;
    }

    public override void Update()
    {
        // �ǰݻ��°� �����°� Ȯ��
        if (!knockbackFlag && player.rigid.velocity.sqrMagnitude < 0.1f) // �˹��� ���� ���� ������� ��
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

    public override void FixedUpdate()
    {
        if(knockbackFlag)
            KnockbackPlayer();
    }

    public override void Exit()
    {
        knockbackDirection = Vector2.zero;
        if (player.rigid.sharedMaterial != null)
        {
            player.rigid.sharedMaterial.friction = 0f;
        }
    }

    private void KnockbackPlayer()
    {
        //�˹�� ���� ����
        knockbackDirection = -player.rigid.velocity.normalized;

        //������ ��� �ʱ�ȭ
        player.rigid.velocity = Vector2.zero;

        //�˹�

        player.rigid.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);

        knockbackFlag = false;
    }

}
