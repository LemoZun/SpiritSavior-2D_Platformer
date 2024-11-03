using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOptionUI : BaseUI
{
    List<GameObject> optionBoxs = new List<GameObject>(2);
   
    int _menuButtonInHash;
    int _menuButtonOutHash;

    private bool _iInputKey = false;
    protected override void Awake()
    {
        base.Awake();

        _menuButtonInHash = Animator.StringToHash("In");
        _menuButtonInHash = Animator.StringToHash("Out");      
    }

    private void Start()
    {
        SubscribeEvent();
        Init();     
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            ToggleGameOptionUI();
            
        }
    }
    
    /// <summary>
    /// ���� �ɼ� UI On/Off
    /// </summary>
    private void ToggleGameOptionUI()
    {
        if (GetUI("GameOptionUI").activeSelf)
        {
            Time.timeScale = 1f;
            
            GetUI("GameOptionUI").SetActive(false);
            GetUI<Animator>("MenuButton").Play("Out");
            
            Manager.Game.Player.GetComponent<Animator>().enabled = true;
        }
        else
        {
            Time.timeScale = 0f;

            GetUI("GameOptionUI").SetActive(true);
            GetUI<Animator>("MenuButton").Play("In");

            Manager.Game.Player.GetComponent<Animator>().enabled = false;
            CloseOptionBox();
        }
    }


    /// <summary>
    /// ���۹� ON/OFF
    /// </summary>
    private void ToggleKeyOption()
    {
        Manager.Sound.PlaySFX(Manager.Sound.Data.ClickSound);

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



    private void BackTitle()
    {
        SceneManager.LoadSceneAsync("TitleScene");
    }

    /// <summary>
    /// ���� �ʱ⼼��
    /// </summary>
    private void Init()
    {
        InitVolume();
        InitButton();
        InitOptionBox();

        if (GetUI("GameOptionUI").activeSelf)
        {
            ToggleGameOptionUI();
        }   
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
        if (GetUI("GameOptionUI").activeSelf) // �ɼ�UI �� On�϶��� ���α�
            ToggleGameOptionUI();
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


    /// <summary>
    /// UI �̺�Ʈ ����
    /// </summary>
    private void SubscribeEvent()
    {
        GetUI<Button>("KeyButton").onClick.AddListener(ToggleKeyOption);

        // ����� ������ư �̺�Ʈ ����
        GetUI<Button>("AudioButton").onClick.AddListener(ToggleAudioOption);

        // Audio �����̴� �̺�Ʈ ����
        GetUI<Slider>("MasterVolume").onValueChanged.AddListener(SetVolumeMaster);
        GetUI<Slider>("BGMVolume").onValueChanged.AddListener(SetVolumeBGM);
        GetUI<Slider>("SFXVolume").onValueChanged.AddListener(SetVolumeSFX);

        // �޴� On/Off �̺�Ʈ ����
        GetUI<Button>("BackButton").onClick.AddListener(ToggleGameOptionUI);
        GetUI<Button>("MenuButton").onClick.AddListener(ToggleGameOptionUI);

        // ����ȭ�� ��ư �̺�Ʈ ����
        GetUI<Button>("TitleButton").onClick.AddListener(BackTitle);
    }


}
