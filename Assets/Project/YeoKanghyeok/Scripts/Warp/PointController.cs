using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject buttonCanvas;
    [SerializeField] Transform buttonLayout;
    [SerializeField] Material unActiveMaterial;
    [SerializeField] Material activeMaterial;
    [SerializeField] bool _inPoint; // point ���ٿ���
    [SerializeField] bool _pointActive; // point Ȱ��ȭ ����
    [SerializeField] bool _uiActive; // ui canvas Ȱ��ȭ ����
    private GameObject _buttonObject;
    private SpriteRenderer _spriteRenderer;
    public ButtonController _button;
    private Transform _transform;
    void Start()
    {
        _inPoint = false;
        _pointActive = false;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = unActiveMaterial;

        buttonCanvas.SetActive(false);

        _transform = GetComponent<Transform>();


    }

    #region point ���ٿ���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _inPoint = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _inPoint = false;
        }
    }
    #endregion


    void Update()
    {
        if (_inPoint)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // pointȰ��ȭ �� button ����
                if (!_pointActive)
                {
                    ActivePoint();
                    _pointActive = true;
                }

                // ui canvas Ȱ��ȭ
                if (!_uiActive)
                {
                    buttonCanvas.SetActive(true);
                    _uiActive = true;
                }
                else if (_uiActive)
                {
                    buttonCanvas.SetActive(false);
                    _uiActive = false;
                }
            }
        }
        else
        {
            buttonCanvas.SetActive(false);
            _uiActive = false;
        }
    }

    private void ActivePoint()
    {
        _spriteRenderer.material = activeMaterial;

        _buttonObject = Instantiate(buttonPrefab, buttonLayout) as GameObject; // button ����
        _button = _buttonObject.GetComponent<ButtonController>();
        _button.Destinasion = _transform;
        _button.Destinasion.position = _transform.position; // point ��ġ ��ư �������� ����
    }
}
