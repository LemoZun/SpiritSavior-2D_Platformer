using System.Collections;
using UnityEngine;

public class TriggerTrap : Trap
{
    [Header("�������� ����Ʈ")]
    [SerializeField] Transform _fallingPoint;

    [Header("�������� ��ü")]
    [SerializeField] FallingTrapObject _fallingTrapObject;
    [SerializeField] int _damage = 1;

    [Header("��ü ������ Ÿ��")]
    [SerializeField] float _lifeTime;

    [Header("��ߵ� �ð�")]
    [SerializeField] float _restartTime;

    WaitForSeconds _lifeTimeDelay;
    WaitForSeconds _restartTimeDelay;
    bool _canActive = true;

    private void Awake()
    {
        _lifeTimeDelay = new WaitForSeconds(_lifeTime);
        _restartTimeDelay = new WaitForSeconds(_restartTime);
    }

    protected override void Start()
    {
        base.Start();      
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(collision.gameObject.tag == "Player")
        {
            ActiveTrap();
        }
    }

    void ActiveTrap()
    {
        if (_canActive)
        {
            FallingTrapObject fallingTrapObject = Instantiate(_fallingTrapObject, _fallingPoint.position, _fallingPoint.rotation);
            fallingTrapObject.SetLifeTimeDelay(_lifeTimeDelay);
            fallingTrapObject.SetDamage(_damage);
            StartCoroutine(RestartRoutine());
        }
    }

    IEnumerator RestartRoutine()
    {
        _canActive = false;
        yield return _restartTimeDelay;      
        _canActive = true;
    }
}