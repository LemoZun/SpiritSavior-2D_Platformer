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

    public Transform location;



    // PlayerModel playerModel;
    
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // runFX = GameObject.Find("SmallDustFX");

        this.location = transform;
    }

   // public void TestTest()
   // {
   //     playerModel.OnPlayerJumped += (PlayJumpFX);
   // }

    public void PlayRunFX()
    {
        Debug.Log("PM Test");
        ObjectPool.SpawnObject(runFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayJumpFX()
    {
        Debug.Log("PM Test");
        ObjectPool.SpawnObject(jumpFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayDoubleJumpFX(Transform trans)
    {
        Debug.Log("PM Test");
        ObjectPool.SpawnObject(dJumpFX, this.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayHitFX()
    {
        Debug.Log("PM Test HIt");
        ObjectPool.SpawnObject(hitFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayGrassFX()
    {
        Debug.Log("PM Test");
        ObjectPool.SpawnObject(GrassFX, transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }

    void Start()
    {

    }


    void Update()
    {

    }
}
