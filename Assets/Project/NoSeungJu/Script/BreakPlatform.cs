using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �������� ����
/// </summary>
public class BreakPlatform : MonoBehaviour
{
    [Header("���� ��")]
    [SerializeField] GameObject _platform;

    [Header("�� ���� �׷�")]
    [SerializeField] GameObject _rockPieceGroup;

    [Header("�� ���� ������� �ð�")]
    [SerializeField] float _rockLifeTime;

    [Header("���� ����� �ð�")]
    [SerializeField] float _respawnTime;


    PolygonCollider2D _platformCollider;
    [SerializeField] BrokenRockPiece[] _rockPieces;
    WaitForSeconds _respawnDelay;
     
    private void Awake()
    {
        _platformCollider = GetComponent<PolygonCollider2D>();   
        _respawnDelay = new WaitForSeconds(_respawnTime);
    }
    private void Start()
    {
        InitRockPiecesLifeTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Break();
        }
    }

    void Break()
    {
        _platform.SetActive(false);
        _platformCollider.enabled =false;
        OnEnableRockPieces();
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return _respawnDelay;
        _platform.SetActive(true);
        _platformCollider.enabled = true;
    }


    void InitRockPiecesLifeTime()
    {
        _rockPieces = _rockPieceGroup.GetComponentsInChildren<BrokenRockPiece>();
        WaitForSeconds rockLifeTime = new WaitForSeconds(_rockLifeTime);
        for (int i = 0; i < _rockPieces.Length; i++)
        {
            _rockPieces[i].SetLifeTime(rockLifeTime);
        }
        OnDiableRockPieces();
    }

    void OnEnableRockPieces()
    {
        foreach (BrokenRockPiece piece in _rockPieces) 
        {
            piece.gameObject.SetActive(true);
        }
    }
    void OnDiableRockPieces()
    {
        foreach (BrokenRockPiece piece in _rockPieces)
        {
            piece.gameObject.SetActive(false);
        }
    }
}
