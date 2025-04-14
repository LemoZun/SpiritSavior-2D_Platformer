using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.ParkJunMin.Scripts
{
    [System.Serializable]
    public class PlayerModel
    {
        public enum Nature {Red, Blue} // 플레이어 속성
        
        [Flags]
        public enum Ability 
        {
            // 능력 해금이 필요한 상태들
            None = 0,
            Tag = 1 << 0,
            Dash = 1 << 1,
            WallJump = 1 << 2,
            DoubleJump = 1 << 3
        }

        public event Action<Nature> OnPlayerTagged;
        public event Action OnPlayerDamageTaken;
        public event Action OnPlayerHealth;
        public event Action OnPlayerJumped;
        public event Action<Ability> OnAbilityUnlocked;
        public event Action OnPlayerRan;
        public event Action OnPlayerDoubleJumped;
        public event Action OnPlayerDashed;
        public event Action OnPlayerDied;
        public event Action OnPlayerSpawn;
        public event Action OnPlayerWallGrabbed;
        public event Action OnPlayerWallSlided;
        public event Action OnPlayerWallJumped;
        public event Action OnPlayerLanded;
        public event Action OnPlayerWakedUp;
        

        [Header("Player Data")]
        public float moveSpeed;        // 이동속도
        public float dashForce;         // 대시 힘
        public float dashCoolTime; // 대시 사용 후 쿨타임
        public float jumpForce;    // 높은점프 힘
        public float doubleJumpForce; // 더블 점프시 얼마나 위로 올라갈지 결정
        public float knockbackForce; // 피격시 얼마나 뒤로 밀려날 지 결정
        public float wallJumpPower; // 벽점프 힘
        public float maxAngle; // 이동 가능한 최대 각도
    
        public bool invincibility;
        public Nature curNature;
        public int hp;
        [FormerlySerializedAs("curMaxHP")] public int curMaxHp = 2; //�ӽð�
        private const int MaxHp = 3;

        public PlayerModel()
        {
            hp = curMaxHp;
        }

        public void UnlockAbilityEvent(Ability newAbility)
        {
            OnAbilityUnlocked?.Invoke(newAbility);
        }

        public void TagPlayerEvent()
        {
            curNature = curNature == Nature.Red ? Nature.Blue : Nature.Red;
            OnPlayerTagged?.Invoke(curNature);
        }

        public void TakeDamageEvent(int damage)
        {
            if (invincibility || hp <= 0) 
                return;
            hp -= damage;
            OnPlayerDamageTaken?.Invoke();
        }

        public void JumpPlayerEvent()
        {
            OnPlayerJumped?.Invoke();
        }

        public void DoubleJumpPlayerEvent()
        {
            OnPlayerDoubleJumped?.Invoke();
        }

        public void DashPlayerEvent()
        {
            OnPlayerDashed?.Invoke();
        }

        public void RunPlayerEvent()
        {
            OnPlayerRan?.Invoke();
        }

        public void GrabWallEvent()
        {
            OnPlayerWallGrabbed?.Invoke();
        }

        public void SlideWallEvent()
        {
            OnPlayerWallSlided?.Invoke();
        }

        public void JumpWallEvent()
        {
            OnPlayerWallJumped?.Invoke();
        }

        public void LandEvent()
        {
            OnPlayerLanded?.Invoke();
        }

        public void WakeUpEvent()
        {
            OnPlayerWakedUp?.Invoke();
        }

        public void HealPlayerEvent()
        {
            if (hp < curMaxHp)
                hp++;
            OnPlayerHealth?.Invoke();
        }
    
        public void DiePlayerEvent()
        {
            OnPlayerDied?.Invoke();
        }

        public void SpawnPlayerEvent()
        {
            OnPlayerSpawn?.Invoke();
        }
    }
}

