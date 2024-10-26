using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    public PlayerController player;
    protected PlayerModel.Nature prevNature;
    public int animationIndex;
    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    /// <summary>
    /// ������Ʈ���� �Ӽ� ������ Ž���ϰ� �ִϸ��̼��� ����ϴ� �޼���
    /// </summary>
    protected void PlayAnimationInUpdate()
    {
        if (prevNature != player.playerModel.curNature)
        {
            player.playerView.PlayAnimation(animationIndex);
            prevNature = player.playerModel.curNature;
        }
        //�ִϸ��̼� ���� ��Ȳ�� ���� ���� ���� �� �ִϸ��̼��� ����Ǹ� �����Ű���
    }
}
