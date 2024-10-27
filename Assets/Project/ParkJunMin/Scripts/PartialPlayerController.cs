using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerUI _playerUI;
    [SerializeField] float _invincibility = 1;

    bool _canTakeDamage = true;
    bool _isDead;

    WaitForSeconds _dieDelay = new WaitForSeconds(0.767f);

    private void OnEnable()
    {
        if (_isDead)
        {
            InitRespawnPlayer();
        }
    }

    /// <summary>
    /// �÷��̾��� ������ �ǰ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (_canTakeDamage == false) return;

        playerModel.hp -= damage;
        _playerUI.SetHp(playerModel.hp);
        StartCoroutine(InvincibilityRoutine());
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

    /// <summary>
    /// ĳ���� ���� �޼���
    /// </summary>
    void Die()
    {
        _playerUI.ShowDeadFace();
        StartCoroutine(DieRoutine());
    }

    /// <summary>
    /// ĳ���� ���� �ִϸ��̼� ������ �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DieRoutine()
    {
        _isDead = true;
        ChangeState(State.Dead);
        yield return _dieDelay;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �ǰݹ����ð� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator InvincibilityRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_invincibility);
        _canTakeDamage = false;
        yield return delay;
        _canTakeDamage = true;
    }

    /// <summary>
    /// �������ÿ� ĳ���� ���� �ʱ�ȭ 
    /// </summary>
    void InitRespawnPlayer()
    {
        _isDead = false;
        transform.position = Manager.Game.RespawnPoint;
        playerModel.curNature = PlayerModel.Nature.Red;
        ChangeState(State.Idle);
        playerModel.hp = playerModel.MaxHP;
        _playerUI.SetHp(playerModel.hp);
    }
}
