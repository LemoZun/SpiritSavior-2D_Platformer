using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;

    Dictionary<Vector2, bool> _disPosableTrapDic = new Dictionary<Vector2, bool>(40);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
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
}