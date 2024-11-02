using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{

    public static TestSceneChanger Instance;

    [Header("�÷��̾� ���� ��")]
    public SceneField _playerScene;
    [Header("���� ��������")]
    [SerializeField] SceneField _firstStage;

    Dictionary<Vector2, bool> _disPosableTrapDic = new Dictionary<Vector2, bool>(40);

    private void Awake()
    {
        if (Instance == null && SceneChanger.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitGameScene();
    }


    /// <summary>
    /// ��ȸ�� Ʈ���� ������� üũ
    /// </summary>
    /// <returns></returns>
    public bool CheckKeepingTrap(Vector2 key)
    {
        if (_disPosableTrapDic.ContainsKey(key) == false)
        {
            _disPosableTrapDic.Add(key, true);
        }
        return _disPosableTrapDic[key];
    }

    /// <summary>
    /// ��ȸ�� Ʈ���� ������ ����
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetKeepingTrap(Vector2 key, bool value)
    {
        if (_disPosableTrapDic.ContainsKey(key))
        {
            _disPosableTrapDic[key] = value;
        }
    }

    /// <summary>
    /// �ʱ� ���� �� �ε�
    /// </summary>
    public void InitGameScene()
    {
        AsyncOperation playerSceneOper = SceneManager.LoadSceneAsync(_playerScene);
        playerSceneOper.allowSceneActivation = true;
        AsyncOperation firstSceneOper = SceneManager.LoadSceneAsync(_firstStage, LoadSceneMode.Additive);
        firstSceneOper.allowSceneActivation = true;
        //StartCoroutine(LoadSceneRoutine());
    }
}
