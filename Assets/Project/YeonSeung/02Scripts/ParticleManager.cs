using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    PlayerController _player;

    // float x;
    // float y;
    // float z;

    Transform _pBottom;


    // Transform _collisionPos;

    Vector3 _collisionPos;

    [Header("�޸��� FX ")]
    [SerializeField] GameObject runFX;
    [Header("���� FX ")]
    [SerializeField] GameObject jumpFX;
    [Header("�̴� ���� FX ")]
    [SerializeField] GameObject dJumpFX;
    [Header("�ǰ� FX ")]
    [SerializeField] GameObject hitFX;
    [Header("ü��ȸ�� FX ")]
    [SerializeField] GameObject healFX;
    [Header("�ܵ� ��� FX ")]
    [SerializeField] GameObject GrassFX;

    // public Transform location;



    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);



        //this.location = transform;
    }


    #region �Լ�����Ʈ
    public void PlayRunFX()
    {
        Debug.Log("1. RUNFX_PM_Manager");
        ObjectPool.SpawnObject(runFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayJumpFX()
    {
        Debug.Log("2. JumpFX_PM_Manager");
        ObjectPool.SpawnObject(jumpFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayDoubleJumpFX()
    {
        Debug.Log("3. DoubleJump_PM_Manager");
        ObjectPool.SpawnObject(dJumpFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayHitFX()
    {
        Debug.Log("4. Hit_FX_PM_Manager");
        ObjectPool.SpawnObject(hitFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayHealFX()
    {
        Debug.Log("4. Hit_FX_PM_Manager");
        ObjectPool.SpawnObject(healFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayGrassFX()
    {
        Debug.Log("5. GrassFX_PM_Manager");
        ObjectPool.SpawnObject(GrassFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }

    #endregion


    void Start()
    {
        SubscribeEvents();
        _player = Manager.Game.Player;
        // _pBottom.y = _player.transform.position.y;
        _pBottom = _player.bottomPivot;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SpaceBar EventTest");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _collisionPos = collision.transform.position;
        }
    }



    void SubscribeEvents()
    {
        Manager.Game.Player.playerModel.OnPlayerRan += PlayRunFX;
        Manager.Game.Player.playerModel.OnPlayerJumped += PlayJumpFX;
        Manager.Game.Player.playerModel.OnPlayerDoubleJumped += PlayDoubleJumpFX;
        Manager.Game.Player.playerModel.OnPlayerDamageTaken += PlayHitFX;
        Manager.Game.Player.playerModel.OnPlayerHealth += PlayHealFX;
    }
    //  private void PlayerModel_OnPlayerJumped()
    //  {
    //      Debug.Log("EventJUMP�ǳ�");
    //      PlayJumpFX();
    //      throw new NotImplementedException();
    //  }
}
