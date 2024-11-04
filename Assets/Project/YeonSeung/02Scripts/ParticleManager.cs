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


    [Header("�÷��̾� ���� ��ƼŬ")]
    [Space(5)]
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
    [Header("���ȿ�� FX ")]
    [SerializeField] GameObject dashFX;
    [Space(20)]
    [Header("��ɾ�� ȿ�� FX")]
    [Space(5)]
    [Header("�±� ���FX")]
    [SerializeField] GameObject unlockTagFX;
    [Header("������ ���FX ")]
    [SerializeField] GameObject unlockWallJumpFX;
    [Header("�������� ���FX ")]
    [SerializeField] GameObject unlockDoubleJumpFX;
    [Header("��� ���FX ")]
    [SerializeField] GameObject unlockDashFX;

    [Header("������ FX ")]
    [SerializeField] GameObject spawnFX;
    [Space(20)]
    [Header("�ٸ���ƼŬ��")]
    [Space(5)]



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
        ObjectPool.SpawnObject(runFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayJumpFX()
    {
        ObjectPool.SpawnObject(jumpFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayDoubleJumpFX()
    {
        ObjectPool.SpawnObject(dJumpFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayHitFX()
    {
        ObjectPool.SpawnObject(hitFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayHealFX()
    {
        ObjectPool.SpawnObject(healFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayDashFX()
    {
        ObjectPool.SpawnObject(dashFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlaySpawnFX()
    {
        ObjectPool.SpawnObject(spawnFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }

    // �ر� FX
    public void PlayUnlockTagFX()
    {
        ObjectPool.SpawnObject(unlockTagFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayUnlockWallJumpFX()
    {
        ObjectPool.SpawnObject(unlockWallJumpFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayUnlockDoubleJumpFX()
    {
        ObjectPool.SpawnObject(unlockDoubleJumpFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }
    public void PlayUnlockDashFX()
    {
        ObjectPool.SpawnObject(unlockDashFX, _pBottom.transform.position, transform.rotation, ObjectPool.PoolType.ParticleSystem);
    }



    // �ٸ� ��ƼŬ��
    public void PlayGrassFX()
    {
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
        Manager.Game.Player.playerModel.OnPlayerDashed += PlayDashFX;
        Manager.Game.Player.playerModel.OnAbilityUnlocked += UpdateSkillFX;
        Manager.Game.Player.playerModel.OnPlayerSpawn += PlaySpawnFX;
    }

    void UpdateSkillFX(PlayerModel.Ability ability)
    {
        switch (ability)
        {
            case PlayerModel.Ability.Tag:
                PlayUnlockTagFX();
                break;
            case PlayerModel.Ability.WallJump:
                PlayUnlockWallJumpFX();
                break;
            case PlayerModel.Ability.DoubleJump:
                PlayUnlockDoubleJumpFX();
                break;
            case PlayerModel.Ability.Dash:
                PlayUnlockDashFX();
                break;
            default:
                break;
        }
    }
}
