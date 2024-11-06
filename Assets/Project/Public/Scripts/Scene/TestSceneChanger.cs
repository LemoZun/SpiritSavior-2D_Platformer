using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{

    public static TestSceneChanger Instance;

    [Header("�÷��̾� ���� ��")]
    public SceneField _playerScene;
    [Header("���� ��������")]
    [SerializeField] SceneField _firstStage;

    [SerializeField] public SceneLoadTrigger CurSceneTrigger;
    public event UnityAction OnChangeCurSceneTrigger;

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
        if (Manager.Game.DisPosableTrapDic.ContainsKey(key) == false)
        {
            Manager.Game.DisPosableTrapDic.Add(key, true);
        }
        return Manager.Game.DisPosableTrapDic[key];
    }

    /// <summary>
    /// ��ȸ�� Ʈ���� ������ ����
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetKeepingTrap(Vector2 key, bool value)
    {
        if (Manager.Game.DisPosableTrapDic.ContainsKey(key))
        {
            Manager.Game.DisPosableTrapDic[key] = value;
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

    public void SetCurSceneTrigger(SceneLoadTrigger sceneTrigger)
    {
        CurSceneTrigger = sceneTrigger;
        OnChangeCurSceneTrigger?.Invoke();
    }
}
