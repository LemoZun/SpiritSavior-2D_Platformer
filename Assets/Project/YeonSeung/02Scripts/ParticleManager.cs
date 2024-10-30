using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Transform location;

    PlayerModel pModel;
    
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);


        this.location = transform;
    }


    #region �Լ�����Ʈ
    public void PlayRunFX()
    {
        Debug.Log("1. RUNFX_PM_Manager");
        ObjectPool.SpawnObject(runFX, this.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayJumpFX()
    {
        Debug.Log("2. JumpFX_PM_Manager");
        ObjectPool.SpawnObject(jumpFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayDoubleJumpFX()
    {
        Debug.Log("3. DoubleJump_PM_Manager");
        ObjectPool.SpawnObject(dJumpFX, this.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayHitFX()
    {
        Debug.Log("4. Hit_FX_PM_Manager");
        ObjectPool.SpawnObject(hitFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayGrassFX()
    {
        Debug.Log("5. GrassFX_PM_Manager");
        ObjectPool.SpawnObject(GrassFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }

    #endregion


    void Start()
    {
           SubscribeEvents();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SpaceBar EventTest");
        }
    }
    void SubscribeEvents()
    {
        Manager.Game.Player.playerModel.OnPlayerRan += PlayRunFX;
        Manager.Game.Player.playerModel.OnPlayerJumped += PlayRunFX;
        Manager.Game.Player.playerModel.OnPlayerDoubleJumped += PlayDoubleJumpFX;
        Manager.Game.Player.playerModel.OnPlayerDamageTaken += PlayHitFX;
    }
  //  private void PlayerModel_OnPlayerJumped()
  //  {
  //      Debug.Log("EventJUMP�ǳ�");
  //      PlayJumpFX();
  //      throw new NotImplementedException();
  //  }
}
