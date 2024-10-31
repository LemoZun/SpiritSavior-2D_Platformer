using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    [Header("�Ķ�����")]
    [SerializeField] GameObject LightningB;
    [Header("��������")]
    [SerializeField] GameObject LightningR;
    [Header("��Ʈ �Ķ�")]
    [SerializeField] GameObject glimpseB;
    [Header("��Ʈ ����")]
    [SerializeField] GameObject glimpseR;
    [Header("���� ĥ ��ġ")]
    [SerializeField] Transform hitSpot;
    [Header("���� ġ�� �ֱ�")]
    [SerializeField] float hittingPeriod;
   // [Header("���� ġ�� �ֱ�")]
   // [SerializeField] float strikeSpeed;  // ���ǵ尡 �ʿ��Ѱ� ���� ����? �׳� ������ �״°ž�~
    [Header("���� ������")]
    [SerializeField] int lightningDamage;
    [Header("���� ����")]
    [SerializeField] bool isRandom;


    [SerializeField] PlayerModel.Nature _lightingNature;
    // �� - 0
    // �� - 1
    private GameObject _thisStrike;

    // OnTriggerEnter2D ���

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ObjectPool.SpawnObject(LightningB, hitSpot.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
        }

    }


    /*
     * Tag ���� != ��������, ȸ�ǰ���
     * Tag ���� == ��������, ���ع���
     */

    private void Lightning()
    {
        ObjectPool.SpawnObject(LightningB, hitSpot.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    private void SetActiveCollider(PlayerModel.Nature nature)
    {
        if (nature == _lightingNature)
        {
            
        }
    }
 //   private void OnCollisionEnter2D(Collision2D collision)
 //   {
 //       if (collision.gameObject.tag == "Player")
 //       {
 //           _player.playerModel.TakeDamageEvent(lightningDamage);
 //       }
 //   }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("OnTriggerEnter TEST");
            Lightning();
            _player.playerModel.TakeDamageEvent(lightningDamage);
        }
    }

    private void GenerateRandom()
    {
        int _chance = Random.Range(0, 1);
        if (_chance == 0)
            _thisStrike = LightningB;
            else _thisStrike = LightningR;
    }

}
