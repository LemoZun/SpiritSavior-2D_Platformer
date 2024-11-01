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
    [Space(10)]

    [Header("���� ġ�� �ֱ�:  __�� ���� �Ɽ")]  // �ٵ� 
    [SerializeField] float hittingPeriod;
   // [SerializeField] bool _canRespawn;

    [Header("���� ������")]
    [SerializeField] int lightningDamage;
  // 
  //
  //  [SerializeField] Collider2D _hitBox;



    [SerializeField] PlayerModel.Nature _lightingNature;

    // �� - 0 : �� - 1  / ���߿� ����� �ٸ���Ÿ�� ����ϸ�
    [Header("���� ����")]
    [SerializeField] bool isRandom;
    private GameObject _thisStrike;

    // OnTriggerEnter2D ���

    PlayerController _player;


    private int _defaultLayer;
    private int _ignorePlayerLayer;

    private bool _canAttack = true;

    private Coroutine _PeriodicStrike;

    WaitForSeconds _respawnDelay;
    WaitForSeconds _poolDelay;
    float _poolDelayTime = 0.5f;

 // Tag ���� != ��������, ȸ�ǰ���
 // Tag ���� == ��������, ���ع���



    

    void Awake()
    {
        // Layer �߰�
        _defaultLayer = gameObject.layer;
        _ignorePlayerLayer = LayerMask.NameToLayer("Ignore Player");
        _respawnDelay = new WaitForSeconds(hittingPeriod);
        _poolDelay = new WaitForSeconds(_poolDelayTime);
      //  _hitBox = GetComponent<Collider2D>();
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
      //  _hitBox.enabled = false;
    }
    private void OnEnable()
    {
        if (Manager.Game == null) return;
        _player = Manager.Game.Player;
        _player.playerModel.OnPlayerTagged += SetActiveCollider;
        SetActiveCollider(_player.playerModel.curNature);
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Q))
        {
            Lightning();
        }

        if (_PeriodicStrike == null)
        {
            _PeriodicStrike = StartCoroutine(RespawnRoutine());
            Debug.Log("�ڷ�ƾ��");
         //   Lightning();
        }
       // else if (_PeriodicStrike != null)
       // {
       //     StopCoroutine(_PeriodicStrike);
       //     
       // }
       // StopCoroutine �����ؾߵ�
        
    }
    IEnumerator RespawnRoutine()
    {
       while(true)
        {
            Debug.Log("Coroutine����");
            Lightning();
            yield return _respawnDelay;

        }
       
    }
    private void BoxOn()
    {
      //  _hitBox.enabled = true;
    }
    private void BoxOff()
    {
       // _hitBox.enabled = false;
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
                // Lightning();
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
