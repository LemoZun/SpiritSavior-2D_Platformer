using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �Ŵ���
/// Manager.Sound �� ���ؼ� ���
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _bgm;
    [SerializeField] private AudioSource _sfx;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// BGM ����
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBGM(AudioClip clip)
    {
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
    /// SFX ����
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip) 
    {
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
}
