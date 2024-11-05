using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SoundData : ScriptableObject
{
    public partial struct Player
    {
        public AudioClip HealSound;
        // �ɷ��ر� ���� ����
        public AudioClip unlockTagFX;
        public AudioClip unlockWallJumpFX;
        public AudioClip unlockDoubleJumpFX;
        public AudioClip unlockDashFX;

    }
    public AudioClip HealSound { get { return _player.HealSound; } }
    // �ɷ��ر� ����
    public AudioClip UnlockTagSound { get { return _player.HealSound; } }
    public AudioClip UnlockWallJumpSound { get { return _player.HealSound; } }
    public AudioClip UnlockDoubleJumpSound { get { return _player.HealSound; } }
    public AudioClip UnlockDashSound { get { return _player.HealSound; } }

}
