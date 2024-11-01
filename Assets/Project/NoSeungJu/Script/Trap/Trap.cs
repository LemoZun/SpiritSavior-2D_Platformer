using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������ ProcessActive���� ������ �ۼ��� ��, Active �Լ��� �����ų ��
/// </summary>
public abstract class Trap : MonoBehaviour
{

    [Header("���ӿ��� ��ȸ���ΰ�?")]
    [SerializeField] protected bool _isDisposable;

    protected virtual void Start()
    {
        if (_isDisposable)
        {
            bool keeping = false;
            if (SceneChanger.Instance != null)
            {
                keeping = SceneChanger.Instance.CheckKeepingTrap(transform.position);
            }
            else if (TestSceneChanger.Instance != null)
            {
                keeping = SceneChanger.Instance.CheckKeepingTrap(transform.position);
            }
            if (!keeping)
            {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���⼭ ������ �ۼ��ϰ� Active�� ȣ���� ��
    /// </summary>
    protected abstract void ProcessActive();

    protected void Active()
    {
        ProcessActive();
        if (_isDisposable)
            UnActiveFromGame();
    }

    protected void UnActive()
    {
        ProcessUnActive();
    }

    protected virtual void ProcessUnActive() { }
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
    public void ActiveFromGame()
    {
        if (SceneChanger.Instance == null) return;
        SceneChanger.Instance.SetKeepingTrap(transform.position, true);
    }

    /// <summary>
    /// ���ӿ��� ����
    /// </summary>
    public void UnActiveFromGame()
    {
        if (SceneChanger.Instance == null) return;
        SceneChanger.Instance.SetKeepingTrap(transform.position, false);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision) { }
    protected virtual void OnTriggerEnter2D(Collider2D collision) { }

}