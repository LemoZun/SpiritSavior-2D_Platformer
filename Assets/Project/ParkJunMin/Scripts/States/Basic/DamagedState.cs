using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : PlayerState
{
    private float _knockbackForce;
    private float _minKnockback = 0.5f;
    public DamagedState(PlayerController player, float knockbackForce) : base(player)
    {
        animationIndex = (int)PlayerController.State.Damaged;
        this._knockbackForce = knockbackForce;
    }

    public override void Enter()
    {
        Debug.Log("�ǰ� ���� ����");

        //�˹�� ���� ����
        //�� �ุ �˹���� �� �� ��� �˹������ ���� ����
        Vector2 knockbackDirection = -player.rigid.velocity.normalized;
        //Debug.Log($"{knockbackDirection.x},  {knockbackDirection.y}");


        //knockbackDirection = new Vector2(knockbackDirection.x, knockbackDirection.y + 1.0f);
        //������ ��� �ʱ�ȭ
        player.rigid.velocity = Vector2.zero;

        //�������� 
        player.playerModel.invincibility = true;
        
        player.playerView.PlayAnimation(animationIndex);
        player.hp = player.playerModel.hp;

        //�ǰݽ� �˹�
        
        player.rigid.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
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
        Debug.Log("�ǰ� ���� Ż��");
    }

    private void KnockbackPlayer()
    {

    }

}
