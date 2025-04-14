using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Project.ParkJunMin.Scripts.States
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class PlayerState : BaseState
    {
        protected readonly PlayerController player;
        protected PlayerModel.Nature prevNature; // 플레이어의 현재 속성
        // 플레이어 첫 생성시 보유 어빌리티 초기화
        public PlayerModel.Ability ability = PlayerModel.Ability.None;
        protected int animationIndex;

        protected PlayerState(PlayerController player)
        {
            this.player = player;
        }
        protected void PlayAnimationInUpdate()
        {
            AnimatorStateInfo curAnimationState = player.playerView.animator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = curAnimationState.normalizedTime % 1;
            if (prevNature == player.playerModel.curNature) 
                return;
            player.playerView.PlayAnimation(animationIndex, normalizedTime);
            prevNature = player.playerModel.curNature;
        }
    }
}
