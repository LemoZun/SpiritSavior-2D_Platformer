using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerUI _playerUI;
    [SerializeField] float _invincibility;

    bool _canTakeDamage = true;

    /// <summary>
    /// �÷��̾��� ������ �ǰ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (_canTakeDamage == false) return;

        playerModel.hp -= damage;
        _playerUI.SetHp(playerModel.hp);
        if (playerModel.hp <= 0)
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
        _playerUI.SetHp(playerModel.hp);
        if (playerModel.hp > playerModel.MaxHP)
        {
            playerModel.hp = playerModel.MaxHP;
        }
    }

    void Die()
    {
        // �÷��̾� ��� ����
        _playerUI.ShowDeadFace();
        Destroy(gameObject);
    }

    IEnumerator InvincibilityRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_invincibility);
        _canTakeDamage = false;
        yield return delay;
        _canTakeDamage = true;

    }
}
