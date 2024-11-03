using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ���� �Ŵ���
/// Manager.Sound �� ���ؼ� ���
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] public SoundData Data;
    [SerializeField] private AudioSource _bgm;
    [SerializeField] private AudioSource _sfx;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        Debug.Log(Data.TestBGM);
    }

    /// <summary>
    /// BGM ����
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        _bgm.clip = clip;
        _bgm.Play();
    }

    /// <summary>
    /// BGM ����
    /// </summary>
    public void StopBGM() 
    {
        if (_bgm.isPlaying)
        {
            _bgm.Stop();
        }
    }

    /// <summary>
    /// BGM �Ͻ�����
    /// </summary>
    public void PauseBGM()
    {
        if (_bgm.isPlaying)
        {
            _bgm.Pause();
        }
    }

    /// <summary>
    /// BGM ���� ����
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeBGM(float volume)
    {
        _bgm.volume = volume;
    }
    
    /// <summary>
    /// BGM ������ ���
    /// </summary>
    /// <returns></returns>
    public float GetVolumeBGM()
    {
        return _bgm.volume;
    }

    /// <summary>
    /// SFX ����
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip) 
    {
        if (clip == null) return;
        _sfx.PlayOneShot(clip);
    }

    /// <summary>
    /// SFX ���� ����
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeSFX(float volume)
    {
        _sfx.volume = volume;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetVolumeSFX()
    {
        return _sfx.volume;
    }
}
