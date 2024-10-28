using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : PlayerState
{
    private float _knockbackForce;
    public DamagedState(PlayerController player, float knockbackForce) : base(player)
    {
        this._knockbackForce = knockbackForce;
    }

    public override void Enter()
    {
        Debug.Log("�ǰ� ���� ����");

        //�˹�� ���� ����
        //�� �ุ �˹���� �� �� ��� �˹������ ���� ����
        Vector2 knockbackDirection = -player.rigid.velocity.normalized;
        //������ ��� �ʱ�ȭ
        player.rigid.velocity = Vector2.zero;

        //�������� 
        player.invincibility = true;
        

        animationIndex = (int)PlayerController.State.Damaged;
        player.playerView.PlayAnimation(animationIndex);
        player.playerModel.TakeDamage(1); // �ϴ� ������ 1 ������

        //�ǰݽ� �˹�
        
        player.rigid.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);

    }

    public override void Update()
    {
        // �ǰݻ��°� �����°� Ȯ��
        if(player.rigid.velocity.magnitude < 0.1f) // �˹��� ���� ���� ������� ��
        {
            player.invincibility = false;

            if (player.playerModel.hp > 0)
                player.ChangeState(PlayerController.State.Idle);
            else
                player.ChangeState(PlayerController.State.Dead);
        }

    }

    public override void Exit()
    {
        Debug.Log("�ǰ� ���� Ż��");
    }
}
