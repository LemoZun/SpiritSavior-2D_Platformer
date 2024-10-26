using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrapObject : MonoBehaviour
{
    WaitForSeconds _lifeTimeDelay;

    Coroutine _lifeTimeRoutine;

    bool _canAttack = true;
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
            // �÷��̾�� �������� �ְų�, �÷��̾� ���
            if (_canAttack)
            {
                Destroy(collision.gameObject);
            }
        }
        else
        {
            _canAttack = false;
            _lifeTimeRoutine = _lifeTimeRoutine == null ? StartCoroutine(LifeTimeRoutine()) : _lifeTimeRoutine; 
        }
    }

    IEnumerator LifeTimeRoutine()
    {
        yield return _lifeTimeDelay;
        Destroy(gameObject);
    }
}
