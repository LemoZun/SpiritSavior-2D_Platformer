using UnityEngine;

namespace Project.ParkJunMin.Scripts.States.InAir
{
    public class LandState : PlayerState
    {
        public LandState(PlayerController player) : base(player)
        {
            animationIndex = (int)PlayerController.State.Land;
        }

        public override void Enter()
        {
            player.playerView.PlayAnimation(animationIndex);
            player.playerModel.LandEvent();
            player.isDoubleJumpUsed = false;
        }

        public override void Update()
        {
            if (player.jumpBufferCounter > 0f)
            {
                player.ChangeState(PlayerController.State.Jump);
                return;
            }

            if (Input.anyKey)
            {
                player.ChangeState(PlayerController.State.Idle);
                return;
            }

            if (player.playerView.IsAnimationFinished())
            {
                player.ChangeState(PlayerController.State.Idle);
            }
        }

        public override void Exit()
        {
        
        }

        public override void FixedUpdate()
        {
        
        }

    }
}
