using System;
using UnityEngine;

namespace Project.ParkJunMin.Scripts
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerModel _playerModel;
        private PlayerController _player;
        public Animator animator;
        public SpriteRenderer renderer;
        public Sprite[] sprites;
        private int _curSpriteIndex = 0;
        public int animationIntervalNumber = 4;
        private AnimatorStateInfo stateInfo;

        //[SerializeField] AudioClip[] clips;

        private int[] _animationHash = new int[]
        {
            //Red
            Animator.StringToHash("Idle_Red"),
            Animator.StringToHash("Run_Red"),
            Animator.StringToHash("Dash_Red"),
            Animator.StringToHash("Jump_Red"),
            Animator.StringToHash("DoubleJump_Red"),
            Animator.StringToHash("Fall_Red"),
            Animator.StringToHash("Land_Red"),
            Animator.StringToHash("WallGrabStart_Red"),
            Animator.StringToHash("WallSliding_Red"),
            Animator.StringToHash("WallJump_Red"),
            Animator.StringToHash("DamageStart_Red"),
            Animator.StringToHash("WakeUp_Red"),
            Animator.StringToHash("Die_Red"),
            Animator.StringToHash("Resurrection_Red"),

            //Blue
            Animator.StringToHash("Idle_Blue"),
            Animator.StringToHash("Run_Blue"),
            Animator.StringToHash("Dash_Blue"),
            Animator.StringToHash("Jump_Blue"),
            Animator.StringToHash("DoubleJump_Blue"),
            Animator.StringToHash("Fall_Blue"),
            Animator.StringToHash("Land_Blue"),
            Animator.StringToHash("WallGrabStart_Blue"),
            Animator.StringToHash("WallSliding_Blue"),
            Animator.StringToHash("WallJump_Blue"),
            Animator.StringToHash("DamageStart_Blue"),
            Animator.StringToHash("WakeUp_Blue"),
            Animator.StringToHash("Die_Blue"),
            Animator.StringToHash("Resurrection_Blue")
        };

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            renderer = GetComponent<SpriteRenderer>();
            if(sprites.Length > 0 )
                renderer.sprite = sprites[_curSpriteIndex];
            _playerModel = _player.playerModel;
            animator = GetComponent<Animator>();
        }



        public void FlipRender(float moveDirection)
        {
            if (moveDirection < 0)
            {
                renderer.flipX = true;
                _player.isPlayerRight = -1;
            }

            if (moveDirection > 0)
            {
                renderer.flipX = false;
                _player.isPlayerRight = 1;
            }
        }

        /// <summary>
        /// 애니메이션 재생을 위한 메서드
        /// </summary>
        /// <param name="animationIndex"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void PlayAnimation(int animationIndex)
        {
            if (animationIndex >= 0 && animationIndex < _animationHash.Length) // 해싱된 애니메이션 파라미터
            {
                switch (_playerModel.curNature) // 플레이어의 현재 속성에 맞는 애니메이션 재생
                {
                    case PlayerModel.Nature.Red:
                        animator.Play(_animationHash[animationIndex],0,0);
                        break;
                    case PlayerModel.Nature.Blue:
                        animator.Play(_animationHash[animationIndex + (int)PlayerController.State.Size],0,0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                Debug.LogError("애니메이션 인덱스 에러");
            }
        }

        public void PlayAnimation(int animationIndex, float normalizedTime)
        {
            if (animationIndex < 0 || animationIndex >= _animationHash.Length) 
                return;
            switch (_playerModel.curNature)
            {
                case PlayerModel.Nature.Red:
                    animator.Play(_animationHash[animationIndex], 0, normalizedTime);
                    break;
                case PlayerModel.Nature.Blue:
                    animator.Play(_animationHash[animationIndex + (int)PlayerController.State.Size], 0, normalizedTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_playerModel.curNature), "플레이어 속성 에러");
            }
        }

        public bool IsAnimationFinished()
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 1.0f;
        }
        public void ChangeSprite()
        {
            //renderer.sprite = sprites[(_curSpriteIndex++)%sprites.Length];
        }
    }
}
