using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SoundData : ScriptableObject 
{
    [Header("BGM")]
    public AudioClip TitleBGM;
    public AudioClip GameBGM;

    [Header("UI ����")]
    public AudioClip HighlightSound;
    public AudioClip ClickSound;
    public AudioClip GameStartSound;
}
