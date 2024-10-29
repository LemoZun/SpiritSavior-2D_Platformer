using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disposable : MonoBehaviour
{
    bool _isDisposable;
    
    /// <summary>
    /// ��ȸ�� bool �� ����
    /// </summary>
    /// <param name="value"></param>
    public void SetIsDisposable(bool value)
    {
        _isDisposable = value;
    }

    /// <summary>
    /// ���ӿ��� ���� X
    /// </summary>
    public void ActiveTrap()
    {
        if (SceneChanger.Instance == null) return;
        SceneChanger.Instance.SetKeepingTrap(transform.position, true);
    }

    /// <summary>
    /// ���ӿ��� ����
    /// </summary>
    public void UnActiveTrap()
    {
        if (SceneChanger.Instance == null) return;
        SceneChanger.Instance.SetKeepingTrap(transform.position, false);
    }
}
