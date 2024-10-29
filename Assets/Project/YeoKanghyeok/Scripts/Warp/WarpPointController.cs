using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPointController : MonoBehaviour
{
    [SerializeField] GameObject warpButton;
    [SerializeField] Transform warpLayout;
    [SerializeField] Material unActiveWarp;
    [SerializeField] Material ActiveWarp;
    [SerializeField] bool inWarp;
    [SerializeField] bool warpActive;
    [SerializeField] bool warpUIActive;
    public GameObject _warpButton;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        inWarp = false;
        warpActive = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _warpButton = Instantiate(warpButton, warpLayout); // UI button ����
        _warpButton.gameObject.SetActive(false); // UI button ��Ȱ��ȭ
        spriteRenderer.material = unActiveWarp;
    }

    // warp point ����Ȯ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inWarp = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inWarp = false;
        }
    }

    private void Update()
    {
        if (inWarp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // EŰ �Է¹޾� ���� Ȱ��ȭ
                warpActive = true;
                spriteRenderer.material = ActiveWarp;

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

        // ���� �� �κ� ���� �� �ƽ��ϴ٤�
        if (warpUIActive)
        {
            // warpActive�� ��� button Ȱ��ȭ
        }
        else
        {
            // warpActive�� ��� button ��Ȱ��ȭ
        }
    }
}
