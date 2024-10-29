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
    private GameObject _warpButton;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        inWarp = false;
        warpActive = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _warpButton = Instantiate(warpButton, warpLayout); // UI button 생성
        _warpButton.gameObject.SetActive(false); // UI button 비활성화
        spriteRenderer.material = unActiveWarp;
    }

    // warp point 접근확인
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
                // E키 입력받아 워프 활성화
                warpActive = true;
                spriteRenderer.material = ActiveWarp;

                // E키 입력으로 워프 UI(또는 버튼) 조종 
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
        else // 워프포인트 밖으로 나오면 자동으로 UI 종료
        {
            warpUIActive = false;
        }

        if (warpUIActive)
        {
            GameObject.FindWithTag("Warp").SetActive(true); // 모든 warp 활성화
        }
        else
        {
            GameObject.FindWithTag("Warp").SetActive(false); // 모든 warp 비활성화
        }
    }
}
