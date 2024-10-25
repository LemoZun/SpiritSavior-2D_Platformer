using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerModel _playerModel;
    private PlayerController _player;
    private Animator _animator;
    public SpriteRenderer renderer;
    public Sprite[] sprites;
    private int _curSpriteIndex = 0;
    

    //[SerializeField] AudioClip[] clips;

    private int[] _animationHash = new int[]
    {
        Animator.StringToHash("Idle_Red"),
        Animator.StringToHash("Run_Red"),
        Animator.StringToHash("Jump_Red"),
        Animator.StringToHash("Fall_Red"),

        Animator.StringToHash("Idle_Blue"),
        Animator.StringToHash("Run_Blue"),
        Animator.StringToHash("Jump_Blue"),
        Animator.StringToHash("Fall_Blue")
    };



    private void Start()
    {
        _player = GetComponent<PlayerController>();
        renderer = GetComponent<SpriteRenderer>();
        if(sprites.Length > 0 )
            renderer.sprite = sprites[_curSpriteIndex];
        _playerModel = _player.playerModel;
        _animator = GetComponent<Animator>();
    }

    public void ChangeSprite()
    {
        //renderer.sprite = sprites[(_curSpriteIndex++)%sprites.Length];
    }

    public void FlipRender(float _moveDirection)
    {
        if (_moveDirection < 0)
            renderer.flipX = true;
        if (_moveDirection > 0)
            renderer.flipX = false;
    }

    public void PlayAnimation(int animationIndex)
    {
        if(animationIndex >= 0 &&  animationIndex < _animationHash.Length)
        {
            _animator.Play(_animationHash[animationIndex],0,0);
        }
        else
        {
            Debug.LogError("애니메이션 인덱스 에러");
        }
    }

}
