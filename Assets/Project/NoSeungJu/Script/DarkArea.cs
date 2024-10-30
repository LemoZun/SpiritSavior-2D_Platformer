using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class DarkArea : Trap
{
    [Header("�������� ������ �ٽ� ��ο� ���°�?")]
    [SerializeField] bool _isToggle;


    SpriteRenderer[] _areas;

    private void Awake()
    {
        _areas = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in _areas)
        {
            sprite.sortingLayerName = "Player";
            sprite.sortingOrder = 10;
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Active();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_isToggle)
            {
                UnActive();
            }
        }
    }

    protected override void ProcessActive()
    {
        // ���� ��������Ʈ ������Ʈ�� ��Ȱ��ȭ
        foreach (SpriteRenderer sprite in _areas) 
        {
            sprite.gameObject.SetActive(false);
        }   
           
    }

    protected override void ProcessUnActive()
    {
        // ���� ��������Ʈ ������Ʈ�� Ȱ��ȭ
        foreach (SpriteRenderer sprite in _areas)
        {
            sprite.gameObject.SetActive(true);
        }
    }
}
