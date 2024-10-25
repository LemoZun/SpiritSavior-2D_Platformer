using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("��ȸ��?")]
    [SerializeField] bool _isDisposable;

    int test;

    protected virtual void Start()
    {
        if (_isDisposable) 
        {
            bool keeping = SceneChanger.Instance.CheckKeepingTrap(transform.position);
            if (!keeping) 
            {
                gameObject.SetActive(false);
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDisposable) 
        {
            if (collision.gameObject.tag == "Player")
            {
                SceneChanger.Instance.SetKeepingTrap(transform.position, false);              
            }
        }
    }
}
