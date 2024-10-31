using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [Header("�Ķ�����")]
    [SerializeField] GameObject LightningB;
    [Header("��������")]
    [SerializeField] GameObject LightningR;
    [Header("���� ĥ ��ġ")]
    [SerializeField] Transform hitSpot;
    [Header("���� ġ�� �ֱ�")]
    [SerializeField] float hittingPeriod;
    [Header("���� ġ�� �ֱ�")]
    [SerializeField] float strikeSpeed;
    [Header("���� ������")]
    [SerializeField] int lightningDamage;
    [Header("���� ����")]
    [SerializeField] bool isRandom;


    [SerializeField] PlayerModel.Nature _lightingNature;


    PlayerController _player;


    private int _defaultLayer;
    private int _ignorePlayerLayer;

    private bool _canAttack = false;


    private void Awake()
    {
        // Layer �߰�
        _defaultLayer = gameObject.layer;
        _ignorePlayerLayer = LayerMask.NameToLayer("Ignore Player");
    }
    void Start()
    {
        if (_player != null)
        {
            return;
        }
        _player = Manager.Game.Player;
        _player.playerModel.OnPlayerTagged += SetActiveCollider;

    }


    void Update()
    {
        
    }


    /*
     * Tag ���� != ��������, ȸ�ǰ���
     * Tag ���� == ��������, ���ع���
     */

    private void SetActiveCollider(PlayerModel.Nature nature)
    {
        if (nature == _lightingNature)
        {
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _player.playerModel.TakeDamageEvent(lightningDamage);
        }
    }
}
