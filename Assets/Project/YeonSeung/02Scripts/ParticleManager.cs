using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;


    [Header("�޸��� FX ")]
    [SerializeField] GameObject runFX;
    [Header("���� FX ")]
    [SerializeField] GameObject jumpFX;
    [Header("�̴� ���� FX ")]
    [SerializeField] GameObject dJumpFX;
    [Header("�ǰ� FX ")]
    [SerializeField] GameObject hitFX;
    [Header("�ܵ� ��� FX ")]
    [SerializeField] GameObject GrassFX;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    public void PlayRunFX()
    {
        ObjectPool.SpawnObject(runFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayJumpFX()
    {
        ObjectPool.SpawnObject(runFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayDoubleJumpFX()
    {
        ObjectPool.SpawnObject(runFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayHitFX()
    {
        ObjectPool.SpawnObject(runFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
