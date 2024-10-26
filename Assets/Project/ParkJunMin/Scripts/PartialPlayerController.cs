using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    /// <summary>
    /// �÷��̾��� ������ �ǰ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        playerModel.hp -= damage;
        if (playerModel.hp < 0)
        {
            playerModel.hp = 0;
            Die();
        }
    }

    /// <summary>
    /// �÷��̾ ���� ����
    /// </summary>
    /// <param name="healAmount"></param>
    public void TakeHeal(int healAmount)
    {
        playerModel.hp += healAmount;
        if (playerModel.hp > playerModel.MaxHP)
        {
            playerModel.hp = playerModel.MaxHP;
        }
    }

    void Die()
    {
        // �÷��̾� ��� ����
        Destroy(gameObject);
    }
}
