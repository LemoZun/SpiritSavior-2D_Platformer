using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private SceneField[] _sceneToLoad;

    SceneField _DontUnLoadScene;
    GameObject _player;

    bool _canUnload = true;
   
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        if (SceneChanger.Instance != null) 
        {
            _DontUnLoadScene= SceneChanger.Instance._playerScene;
        }
        else if(TestSceneChanger.Instance != null)
        {
            _DontUnLoadScene = TestSceneChanger.Instance._playerScene;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _player.tag)
        {
            LoadScene();
            UnloadScene();
        }
    }


    public void AddSceneToLoad(SceneField[] sceneToLoad)
    {
        _sceneToLoad = sceneToLoad;
        _canUnload = false;
    }

    public void Delete()
    {
        StartCoroutine(DeleteRoutine());
    }

    WaitForSeconds SceneTriggerDeleteDelay = new WaitForSeconds(1f);
    IEnumerator DeleteRoutine()
    {
        yield return SceneTriggerDeleteDelay;
        Destroy(gameObject);
    }

    void LoadScene()
    {
        for (int i = 0; i < _sceneToLoad.Length; i++) 
        {
            bool isSceneLoaded = false;
            for (int j = 0; j < SceneManager.sceneCount; j++) 
            {
                Scene loadedScneen = SceneManager.GetSceneAt(j);
                if(loadedScneen.name == _sceneToLoad[i].SceneName)
                {
                    isSceneLoaded = true;
                    break;
                }
            }
            if (!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_sceneToLoad[i], LoadSceneMode.Additive);
            }
        }
    }

    void UnloadScene()
    {
        if (_canUnload == false) return;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            bool isLoadScene = false;
            Scene loadedScene = SceneManager.GetSceneAt(i);
            // ���� ���� ��ε�Ǽ� �ȵǴ� ���̶� ����?
            if (loadedScene.name == _DontUnLoadScene.SceneName)
            {
                isLoadScene = true;
            }
            else
            {
                // ���� �ε��ؾ��ϴ� ���̶� ����?
                for (int j = 0; j < _sceneToLoad.Length; j++)
                {
                    if (loadedScene.name == _sceneToLoad[j].SceneName)
                    {
                        isLoadScene = true;
                    }
                }
            }

            // �ε��ؾ��� ���� �ƴϸ� ��ε�
            if(isLoadScene == false)
            {
                SceneManager.UnloadSceneAsync(loadedScene);
            }
        }
    }


}
