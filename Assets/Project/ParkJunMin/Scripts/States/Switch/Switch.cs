using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Switch : Trap
{
    [Header("������ ����ġ �Է� ����?")]
    [SerializeField] bool canManyInput;

    [Header("����ġ�� ������ ���")]
    [SerializeField] SwichInteractable _swichInteractable;


    Vector2 posUI = new Vector2(2, 3);
    SwitchUI _switchUI;
    Coroutine _enterTriggerRoutine;

    private void Awake()
    {
        _switchUI = GetComponent<SwitchUI>();
        UnTrackingUIToPlayer();
    }

    protected override void Start()
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
                keeping = TestSceneChanger.Instance.CheckKeepingTrap(transform.position);
            }
            if (!keeping)
            {
                StartCoroutine(StartRoutine());
            }
        }
    }

    WaitForSeconds _startRoutine = new WaitForSeconds(0.1f);
    IEnumerator StartRoutine()
    {
        yield return _startRoutine;
        Active();
        Destroy(gameObject);
    }

    // Ʈ���ſ� ���� �� ĳ���Ϳ� ��ȣ�ۿ� ����
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _enterTriggerRoutine = _enterTriggerRoutine == null ? StartCoroutine(EnterTriggerRoutine()) : _enterTriggerRoutine;
            TrackingUIToPlayer(collision);
        }
    }

    // Ʈ���ſ��� ���� �� ĳ���Ϳ� ��ȣ�ۿ��� ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_enterTriggerRoutine != null)
        {
            StopCoroutine(_enterTriggerRoutine);
            _enterTriggerRoutine = null;
            UnTrackingUIToPlayer();
        }
    }

    protected override void ProcessActive()
    {
        _swichInteractable.Interact();
    }

    /// <summary>
    /// FŰ �Է��� ���޴� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterTriggerRoutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Active();
                if(canManyInput == false)
                {
                    Delete();
                    yield break;
                }            
            }
            yield return null;
        }
    }

    void TrackingUIToPlayer(Collider2D collider)
    {
        GameObject switchUI = _switchUI.GetUI("SwitchUI");
        switchUI.SetActive(true);
        switchUI.transform.SetParent(collider.transform);
        switchUI.transform.position = new Vector2(
            collider.transform.position.x + posUI.x,
            collider.transform.position.y + posUI.y);
    }

    void UnTrackingUIToPlayer()
    {
        GameObject switchUI = _switchUI.GetUI("SwitchUI");
        if (switchUI != null)
        {
            switchUI.SetActive(false);
            switchUI.transform.SetParent(transform);
        }
    }

    void Delete()
    {
        UnTrackingUIToPlayer();
        gameObject.SetActive(false);
    }
}
