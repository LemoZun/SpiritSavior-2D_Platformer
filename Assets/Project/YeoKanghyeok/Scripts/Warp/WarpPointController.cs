using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPointController : Warp
{
    [SerializeField] Transform warpLayout;
    [SerializeField] Material unActiveWarp;
    [SerializeField] Material ActiveWarp;
    private SpriteRenderer _spriteRenderer;
    private bool _inWarp;
    public bool warpActive;
    

    private void Start()
    {
        _inWarp = false;
        warpActive = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = unActiveWarp;
    }

    // warp point ����Ȯ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _inWarp = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _inWarp = false;
        }
    }
    
    // warpActive : ���� Ȱ��ȭ
    // warpUIActive : ���� UI Ȱ��ȭ(Ȱ��ȭ�� ������ ���ϴ� button Ȱ��ȭ)
    public void Update()
    {
        if (_inWarp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // EŰ �Է¹޾� ���� Ȱ��ȭ
                warpActive = true;
                _spriteRenderer.material = ActiveWarp;

                // EŰ �Է����� ���� UI(�Ǵ� ��ư) ���� 
                if (warpUIActive)
                {
                    warpUIActive = false;
                }
                else
                {
                    warpUIActive = true;
                }
            }
        }
        else // ��������Ʈ ������ ������ �ڵ����� UI ����
        {
            warpUIActive = false;
        }
    }
}
