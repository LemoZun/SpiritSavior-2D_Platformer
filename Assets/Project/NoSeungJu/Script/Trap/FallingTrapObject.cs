using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrapObject : MonoBehaviour
{
    [Space(10)]
    int _damage;   

    WaitForSeconds _lifeTimeDelay;

    Coroutine _lifeTimeRoutine;

    bool _canAttack = true;


    int _ignorePlayerLayer;
    private void Awake()
    {
        _ignorePlayerLayer = LayerMask.NameToLayer("Ignore Player");
    }

    /// <summary>
    /// �� ������ ����
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    /// <summary>
    /// ������Ÿ�� ������ ����
    /// </summary>
    /// <param name="lifeTimeDelay"></param>
    public void SetLifeTimeDelay(WaitForSeconds lifeTimeDelay)
    {
        _lifeTimeDelay = lifeTimeDelay;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_canAttack)
            {
                Manager.Game.Player.playerModel.TakeDamage(_damage);             
            }
        }
        else
        {
            _lifeTimeRoutine = _lifeTimeRoutine == null ? StartCoroutine(LifeTimeRoutine()) : _lifeTimeRoutine; 
        }
        ProcessCollision();
    }

    /// <summary>
    /// ��ü ���鿡 ������ �� ���� ��� ��ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator LifeTimeRoutine()
    {
        yield return _lifeTimeDelay;
        Destroy(gameObject);
    }

    /// <summary>
    /// �浹 ���� ó��
    /// </summary>
    void ProcessCollision()
    {
        _canAttack = false;
        gameObject.layer = _ignorePlayerLayer;
    }
}
