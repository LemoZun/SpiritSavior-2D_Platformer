using System.Collections;
using UnityEngine;

public class PointController : Trap
{
    [Header("���������")]
    [SerializeField] string pointName;
    [SerializeField] ButtonController buttonPrefab;
    [SerializeField] SceneField[] _sceneToLoad;

    [Space(10f)]
    [SerializeField] GameObject buttonCanvas;
    [SerializeField] Transform buttonLayout;
    [SerializeField] ParticleSystem unActiveParticle;
    [SerializeField] ParticleSystem activeParticle;
    private bool _pointActive; // point Ȱ��ȭ ����
    private bool _uiActive; // ui canvas Ȱ��ȭ ����
    private ButtonController _button;

    protected override void Start()
    {
        if (_isDisposable)
        {
            bool isUnActive = false;
            if (SceneChanger.Instance != null)
            {
                isUnActive = SceneChanger.Instance.CheckKeepingTrap(transform.position);
            }
            else if (TestSceneChanger.Instance != null)
            {
                isUnActive = TestSceneChanger.Instance.CheckKeepingTrap(transform.position);
            }
            if (isUnActive == true)
            {
                _pointActive = false;
                unActiveParticle.gameObject.SetActive(true);
                activeParticle.gameObject.SetActive(false);
            }
            else
            {
                _pointActive = true;
                unActiveParticle.gameObject.SetActive(false);
                activeParticle.gameObject.SetActive(true);
            }
        }

        StartCoroutine(StartRoutine());


    }

    IEnumerator StartRoutine()
    {
        yield return null;
        buttonCanvas = Manager.UI.WarpUI;
        buttonCanvas.SetActive(false);
    }

    #region point ���ٿ���
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _inputRoutine = _inputRoutine == null ? StartCoroutine(InputRoutine()) : _inputRoutine;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_inputRoutine != null)
            {
                StopCoroutine(_inputRoutine);
                _inputRoutine = null;
            }

            buttonCanvas.SetActive(false);
            _uiActive = false;
        }
    }
    #endregion

    Coroutine _inputRoutine;
    IEnumerator InputRoutine()
    {
        while (true)
        {
            if (_pointActive)
            {
                Manager.Sound.PlaySFX(Manager.Sound.Data.WarpAfterOpenSound); // 2.3 ���� ���� �Ҹ�
            }
            else
            {
                Manager.Sound.PlaySFX(Manager.Sound.Data.WarpBeforeOpenSound); // 2.1 ������ �� �Ҹ�
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Manager.Sound.PlaySFX(Manager.Sound.Data.WarpOpeningSound); // 2.2 ������ �Ҹ�
                if (!_pointActive)
                {
                    
                    Active();
                    _pointActive = true;
                }

                if (!_uiActive)
                {
                    Manager.Sound.PlaySFX(Manager.Sound.Data.InteractionSound); // 2.4 F ��ȣ�ۿ� �� �Ҹ�
                    buttonCanvas.SetActive(true);
                    _uiActive = true;
                }
                else if (_uiActive)
                {
                    Manager.Sound.PlaySFX(Manager.Sound.Data.InteractionSound); // 2.4 F ��ȣ�ۿ� �� �Ҹ�
                    buttonCanvas.SetActive(false);
                    _uiActive = false;
                }
            }
            yield return null;
        }
    }
    protected override void ProcessActive()
    {
        ActivePoint();
    }

    private void ActivePoint()
    {
        unActiveParticle.gameObject.SetActive(false);
        activeParticle.gameObject.SetActive(true);

        _button = Instantiate(buttonPrefab, buttonCanvas.transform);
        _button._buttonText.text = pointName;
        _button.destinationPos = transform.position;
        _button.SceneToLoad = _sceneToLoad;
    }


}
