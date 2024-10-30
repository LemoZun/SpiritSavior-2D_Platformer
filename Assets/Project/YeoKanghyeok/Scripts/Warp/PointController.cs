using UnityEngine;

public class PointController : Warp
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonCanvas;
    [SerializeField] Material unActiveMaterial;
    [SerializeField] Material activeMaterial;
    [SerializeField] bool _inPoint; // point ���ٿ���
    [SerializeField] bool _pointActive; // point Ȱ��ȭ ����
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
                if (!_pointActive) // pointȰ��ȭ �� button ����
                {
                    ActivePoint();
                    _pointActive = true;
                }

                if (!uiActive)
                {
                    uiActive = true;
                    OnUI();
                }
                else if (uiActive)
                {
                    uiActive = false;
                    OffUI();
                }
            }
        }
        else
        {
            uiActive = false;
            OffUI() ;
        }
    }

    private void ActivePoint()
    {
        _buttonObject = Instantiate(buttonPrefab,buttonCanvas) as GameObject; // button ����
        _button = _buttonObject.GetComponent<ButtonController>();
        _button.Destinasion.position = _transform.position; // point ��ġ ��ư �������� ����
        _button.ActiveButton = true;
    }
}
