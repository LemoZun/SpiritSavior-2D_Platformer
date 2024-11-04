using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : BaseUI
{
    List<GameObject> _highlightList = new List<GameObject>(5);

    List<GameObject> optionBoxs = new List<GameObject>(2);

    int _menuButtonInHash;
    int _menuButtonOutHash;

    private void Start()
    {
        Init();
        SubsCribeEvents();
        Time.timeScale = 1.0f;
    }


    private void Update()
    {

    }

    /// <summary>
    /// UI �̺�Ʈ ����
    /// </summary>
    void SubsCribeEvents()
    {
        // �� ���� ���� �̺�Ʈ ����
        GetUI<Button>("NewGameButton").onClick.AddListener(StartNewGame);

        // ũ���� ���� �̺�Ʈ ����
        GetUI<Button>("CreditButton").onClick.AddListener(ShowCredit);

        // ũ���� ���� �̺�Ʈ ����
        GetUI<Button>("CreditExitButton").onClick.AddListener(HideCredit);

        // ���� ���� �̺�Ʈ ����
        GetUI<Button>("GameOffButton").onClick.AddListener(ExitGame);

        // ����â On/Off �̺�Ʈ ����
        GetUI<Button>("BackButton").onClick.AddListener(ToggleOptionUI);
        GetUI<Button>("OptionButton").onClick.AddListener(ToggleOptionUI);

        // ����Ű ���� �̺�Ʈ ����
        GetUI<Button>("KeyButton").onClick.AddListener(ToggleKeyOption);

        // ����� ������ư �̺�Ʈ ����
        GetUI<Button>("AudioButton").onClick.AddListener(ToggleAudioOption);

        // Audio �����̴� �̺�Ʈ ����
        GetUI<Slider>("MasterVolume").onValueChanged.AddListener(SetVolumeMaster);
        GetUI<Slider>("BGMVolume").onValueChanged.AddListener(SetVolumeBGM);
        GetUI<Slider>("SFXVolume").onValueChanged.AddListener(SetVolumeSFX);
    }


    void StartNewGame()
    {
        SceneChanger.Instance.InitGameScene();
    }

    void ShowCredit()
    {
        GetUI("BlackFillter").gameObject.SetActive(false);
        GetUI("CreditFillter").gameObject.SetActive(true);
        CloseHighlight();
    }

    void HideCredit()
    {
        GetUI("BlackFillter").gameObject.SetActive(true);
        GetUI("CreditFillter").gameObject.SetActive(false);      
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region �ɼ�
    /// <summary>
    /// ���� �ɼ� UI On/Off
    /// </summary>
    private void ToggleOptionUI()
    {
        if (GetUI("OptionUI").activeSelf)
        {
            GetUI("TitleUI").SetActive(true);
            GetUI("OptionUI").SetActive(false);
            CloseHighlight();
        }
        else
        {
            GetUI("TitleUI").SetActive(false);
            GetUI("OptionUI").SetActive(true);
            CloseOptionBox();
        }
    }


    /// <summary>
    /// ���۹� ON/OFF
    /// </summary>
    private void ToggleKeyOption()
    {
        GameObject keyOption = GetUI("KeyOption");
        for (int i = 0; i < optionBoxs.Count; i++)
        {
            if (optionBoxs[i] == keyOption)
                continue;
            optionBoxs[i].SetActive(false);
        }
        keyOption.SetActive(!keyOption.activeSelf);
    }

    /// <summary>
    /// ����� ���� ON/OFF
    /// </summary>
    private void ToggleAudioOption()
    {
        GameObject audioOption = GetUI("AudioOption");
        for (int i = 0; i < optionBoxs.Count; i++)
        {
            if (optionBoxs[i] == audioOption)
                continue;
            optionBoxs[i].SetActive(false);
        }
        audioOption.SetActive(!audioOption.activeSelf);
    }

    private void SetVolumeMaster(float volume)
    {
        if (Manager.Sound.GetVolumeBGM() > volume)
        {
            SetVolumeBGM(volume);
            GetUI<Slider>("BGMVolume").value = Manager.Sound.GetVolumeBGM();
        }
        if (Manager.Sound.GetVolumeSFX() > volume)
        {
            SetVolumeSFX(volume);
            GetUI<Slider>("SFXVolume").value = Manager.Sound.GetVolumeSFX();
        }
    }
    private void SetVolumeBGM(float volume)
    {
        if (GetUI<Slider>("BGMVolume").value > GetUI<Slider>("MasterVolume").value)
        {
            volume = GetUI<Slider>("MasterVolume").value;
            GetUI<Slider>("BGMVolume").value = volume;
        }
        Manager.Sound.SetVolumeBGM(volume);
    }

    private void SetVolumeSFX(float volume)
    {
        if (GetUI<Slider>("SFXVolume").value > GetUI<Slider>("MasterVolume").value)
        {
            volume = GetUI<Slider>("MasterVolume").value;
            GetUI<Slider>("SFXVolume").value = volume;
        }
        Manager.Sound.SetVolumeSFX(volume);
    }

    /// <summary>
    /// ���� �ʱ⼼��
    /// </summary>
    private void Init()
    {
        InitVolume();
        InitButton();
        InitOptionBox();
    }

    /// <summary>
    /// ������ �ʱ⼼��
    /// </summary>
    private void InitVolume()
    {
        float masterVolume = GetUI<Slider>("MasterVolume").value;
        if (Manager.Sound.GetVolumeBGM() > masterVolume)
        {
            SetVolumeBGM(masterVolume);
        }
        GetUI<Slider>("BGMVolume").value = Manager.Sound.GetVolumeBGM();

        if (Manager.Sound.GetVolumeSFX() > masterVolume)
        {
            SetVolumeSFX(masterVolume);
        }
        GetUI<Slider>("SFXVolume").value = Manager.Sound.GetVolumeSFX();
    }

    private void InitButton()
    {
        if (GetUI("OptionUI").activeSelf) // �ɼ�UI �� On�϶��� ���α�
            ToggleOptionUI();
    }

    private void InitOptionBox()
    {
        optionBoxs.Add(GetUI("KeyOption"));
        optionBoxs.Add(GetUI("AudioOption"));

        CloseOptionBox();
    }
    private void CloseOptionBox()
    {
        foreach (GameObject optionBox in optionBoxs)
        {
            optionBox.SetActive(false);
        }
    }

    void CloseHighlight()
    {
        foreach (GameObject highlight in _highlightList)
        {
            highlight.SetActive(false);
        }
    }
    #endregion




    /// <summary>
    /// ���̶���Ʈ ����Ʈ�� �߰�
    /// ���̶���Ʈ�鿡 ���� �����ϰ� ���ϰ��ϱ� ����
    /// </summary>
    /// <param name="highlight"></param>
    public void AddHighlightList(GameObject highlight)
    {
        _highlightList.Add(highlight);
    }
}
