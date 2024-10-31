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
    [Header("���� ġ�� �ֱ�:  __�� ���� �Ɽ")]  // �ٵ� 
    [SerializeField] float hittingPeriod;

    [Header("���� ������")]
    [SerializeField] int lightningDamage;
    [Header("���� ����")]
    [SerializeField] bool isRandom;


    [SerializeField] PlayerModel.Nature _lightingNature;
    
    // �� - 0 : �� - 1
    private GameObject _thisStrike;

    // OnTriggerEnter2D ���

    PlayerController _player;


    private int _defaultLayer;
    private int _ignorePlayerLayer;

    private bool _canAttack = true;

    private Coroutine _delayTime;


 // Tag ���� != ��������, ȸ�ǰ���
 // Tag ���� == ��������, ���ع���



    IEnumerator DelayStrike()
    {
        yield return new WaitForSeconds(hittingPeriod);
    }

    void Awake()
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
        SetActiveCollider(_player.playerModel.curNature);
    }
    private void OnEnable()
    {
        if (Manager.Game == null) return;
        _player = Manager.Game.Player;
        _player.playerModel.OnPlayerTagged += SetActiveCollider;
        SetActiveCollider(_player.playerModel.curNature);
    }
    private void SetActiveCollider(PlayerModel.Nature nature)
    {
        if (nature == _lightingNature)
        {
            _canAttack = true;
        }
        else if (nature != _lightingNature)
        {
            _canAttack = false;
        }
    }
    private void Lightning()
    {
        ObjectPool.SpawnObject(LightningB, hitSpot.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(_canAttack)
            {
                Debug.Log("OnTriggerEnter TEST");
                Lightning();
                _player.playerModel.TakeDamageEvent(lightningDamage);
            }
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
