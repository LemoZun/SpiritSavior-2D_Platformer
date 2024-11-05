using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SoundData : ScriptableObject
{
    public partial struct Player
    {
        public AudioClip HealSound;
        // �ɷ��ر� ���� ����
        public AudioClip UnlockTagSound;
        public AudioClip UnlockWallJumpSound;
        public AudioClip UnlockDoubleJumpSound;
        public AudioClip UnlockDashSound;

        
    }
    public AudioClip HealSound { get { return _player.HealSound; } }
    // �ɷ��ر� ����
    public AudioClip UnlockTagSound { get { return _player.HealSound; } }
    public AudioClip UnlockWallJumpSound { get { return _player.HealSound; } }
    public AudioClip UnlockDoubleJumpSound { get { return _player.HealSound; } }
    public AudioClip UnlockDashSound { get { return _player.HealSound; } }


    // ��ȿ����
    public AudioClip ThunderSound { get { return _mapObject.ThunderSound; } }

    public AudioClip LeafSound { get { return _player.HealSound; } }
    public AudioClip PlatformOnSound { get { return _mapObject.PlatformOnSound; } }
    public AudioClip PlatformOffSound { get { return _mapObject.PlatformOffSound; } }

    public partial struct MapObject
    {
        public AudioClip ThunderSound;
        public AudioClip LeafSound;
        // �÷��� Ÿ������ �Ҹ�
        public AudioClip PlatformOnSound;
        public AudioClip PlatformOffSound;

    }

}
