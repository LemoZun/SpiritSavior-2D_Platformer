using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    // Map �κ�
    private PlayerController _playerController;
    private GameObject _redFace;
    private GameObject _blueFace;
    private GameObject _mapCam;

    // Map�ִϸ��̼� �κ�
    private RawImage _mapImage;
    public float transparencyTime = 1.0f;
    private bool _isTransparency = false;

    // �ڽ� �̵�
    private GameObject _player;

    private void Awake()
    {
        _redFace = GameObject.Find("RedFace");
        _blueFace = GameObject.Find("BlueFace");
        _mapImage = GameObject.Find("MapImage").GetComponent<RawImage>(); ;
        _mapCam = GameObject.Find("MapCamera");
        _player = GameObject.Find("Player");
    }
    void Start()
    {
        _mapImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 0); // ���� 0
        _redFace.SetActive(false);
        _blueFace.SetActive(false);

        _playerController = FindObjectOfType<PlayerController>();
        ChildSettings();
    }
    private void ChildSettings()
    {
        // FaceRed, FaceBlue, Camera�� Player �ڽ����� ����
        Transform redFaceTransform = _redFace.transform;
        Transform blueFaceTransform = _blueFace.transform;
        Transform cameraTransform = _mapCam.transform;

        // Player �ڽ����� ����
        redFaceTransform.SetParent(_player.transform);
        blueFaceTransform.SetParent(_player.transform);
        cameraTransform.SetParent(_player.transform);

        // Player ��ġ�� ����
        redFaceTransform.localPosition = new Vector3(0.23f, 4.28f, 0);
        blueFaceTransform.localPosition = new Vector3(0.23f, 4.28f, 0);
        cameraTransform.localPosition = new Vector3(0.15f, 4.11f, -86.49741f);
    }
    public void FaceMap()
    {
        if (_playerController != null)
        {
            // ���� Ȯ��
            bool isRed = _playerController.playerModel.curNature == PlayerModel.Nature.Red;
            _redFace.SetActive(isRed);
            _blueFace.SetActive(!isRed);
        }
    }
    private IEnumerator TransparencyImage(float start, float end)
    {
        _isTransparency = true;
        float time = 0;
        Color color = _mapImage.color;

        while (time < transparencyTime)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(start, end, time / transparencyTime);
            _mapImage.color = color;
            yield return null;
        }

        _isTransparency = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !_isTransparency)
        {
            if (_mapImage.color.a > 0)
            {
                StartCoroutine(TransparencyImage(1, 0));
                _redFace.SetActive(false);
                _blueFace.SetActive(false);
            }
            else
            {
                StartCoroutine(TransparencyImage(0, 1));
                FaceMap();
            }
        }
        if (_mapImage.color.a > 0)
        {
            FaceMap();
        }
    }
}