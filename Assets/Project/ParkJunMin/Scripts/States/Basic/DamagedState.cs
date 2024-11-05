using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamagedState : PlayerState
{
    private Vector2 knockbackDirection;
    private bool knockbackFlag;
    public DamagedState(PlayerController player) : base(player)
    {
        animationIndex = (int)PlayerController.State.Damaged;
    }

    public override void Enter()
    {
        knockbackFlag = true;

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
    }

    private void KnockbackPlayer()
    {
        //�˹�� ���� ����
        knockbackDirection = -player.rigid.velocity.normalized;

        //������ ��� �ʱ�ȭ
        player.rigid.velocity = Vector2.zero;

        //�˹�

        player.rigid.AddForce(knockbackDirection * player.knockbackForce, ForceMode2D.Impulse);

        knockbackFlag = false;
    }

}
