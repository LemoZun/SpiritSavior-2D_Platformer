using UnityEngine;

public class TriggerTrap : Trap
{
    [Header("�������� ����Ʈ")]
    [SerializeField] Transform _fallingPoint;

    [Header("�������� ��ü")]
    [SerializeField] FallingTrapObject _fallingTrapObject;

    [Header("��ü ������ Ÿ��")]
    [SerializeField] float _lifeTime;

    protected override void Start()
    {
        base.Start();

        _fallingTrapObject.SetFallingPoint(_fallingPoint);
        _fallingTrapObject.SetLifeTimeDelay(new WaitForSeconds(_lifeTime));
    }

    
}