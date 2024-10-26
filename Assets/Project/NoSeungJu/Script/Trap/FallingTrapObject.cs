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
                Manager.Game.Player.TakeDamage(_damage);
                _canAttack = false;
            }
        }
        else
        {
            _lifeTimeRoutine = _lifeTimeRoutine == null ? StartCoroutine(LifeTimeRoutine()) : _lifeTimeRoutine; 
        }
    }

    /// <summary>
    /// ��ü ���鿡 ������ �� ���� ��� ��ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator LifeTimeRoutine()
    {
        _canAttack = false;
        yield return _lifeTimeDelay;
        Destroy(gameObject);
    }
}
