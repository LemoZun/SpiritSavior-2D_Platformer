using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �������� ����
/// </summary>
public class BreakPlatform : Trap
{
    [Header("���� �� ������Ʈ")]
    [SerializeField] GameObject _platform;

    [Header("�� ���� �׷� ������Ʈ")]
    [SerializeField] GameObject _rockPieceGroup;

    [Header("��ƼŬ �׷� ������Ʈ")]
    [SerializeField] GameObject _particleGroup;

    [Header("�� ���� ������� �ð�")]
    [SerializeField] float _rockLifeTime;

    [Header("���� ����� �ð�")]
    [SerializeField] bool _canRespawn;
    [SerializeField] float _respawnTime;


    PolygonCollider2D _platformCollider;
    BrokenRockPiece[] _rockPieces;
    WaitForSeconds _respawnDelay;
     
    private void Awake()
    {
        _platformCollider = GetComponent<PolygonCollider2D>();   
        _respawnDelay = new WaitForSeconds(_respawnTime);
    }
    protected override void Start()
    {
        base.Start();
        InitRockPiecesLifeTime();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "Player")
        {
            Break();
        }
    }

    void Break()
    {
        _platform.SetActive(false);
        _platformCollider.enabled =false;
        _particleGroup.SetActive(true);
        OnEnableRockPieces();

        if(_canRespawn)
            StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        if(_isDisposable == true) yield break;

        yield return _respawnDelay;
        _platform.SetActive(true);
        _platformCollider.enabled = true;
        _particleGroup.SetActive(false);
        OnDiableRockPieces();
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
