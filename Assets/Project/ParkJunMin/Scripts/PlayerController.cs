using System.Collections;
using Project.ParkJunMin.Scripts.States;
using Project.ParkJunMin.Scripts.States.Basic;
using Project.ParkJunMin.Scripts.States.InAir;
using Project.ParkJunMin.Scripts.States.Wall;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.ParkJunMin.Scripts
{
    public partial class PlayerController : MonoBehaviour
    {
        public enum State 
        {
            Idle, 
            Run, 
            Dash, 
            Jump, 
            DoubleJump, 
            Fall, 
            Land, 
            WallGrab, 
            WallSliding,
            WallJump, 
            Damaged, 
            WakeUp, 
            Dead, 
            Spawn, 
            Size 
        }
        [FormerlySerializedAs("_curState")] [SerializeField] private State curState;
        private readonly PlayerState[] _states = new PlayerState[(int)State.Size];

        public PlayerModel.Ability unlockedAbilities = PlayerModel.Ability.None;
        public PlayerModel playerModel = new PlayerModel();
        [HideInInspector] public PlayerView playerView;

        private Collider2D _playerCollider;
        [FormerlySerializedAs("_groundLayerMask")] public int groundLayerMask;
        private int _wallLayerMask;
        private int _ignorePlayerLayerMask;

        public float speedAdjustmentOffsetInAir; // 공중에서의 속도 = 땅에서의 속도 * 해당 변수
        [HideInInspector] public float moveSpeedInAir;    // 공중에서 플레이어의 속도

        [Space(30)]
        [Header("Checking")]
        // 코요테 타임
        public float coyoteTime = 0.2f;
        public float coyoteTimeCounter;
        //점프 버퍼
        public float jumpBufferTime = 0.2f;
        public float jumpBufferCounter;
        public bool isGrounded;        // 캐릭터가 땅에 붙어있는지 체크
        
        [HideInInspector] public bool isDoubleJumpUsed; // 더블점프 사용 유무를 나타내는 변수
        [HideInInspector] public bool isDashUsed; // 대시를 사용했는지 유무를 나타내는 변수

        [HideInInspector] public Rigidbody2D rigid;
        [HideInInspector] public float dashDeltaTime;

        [Space(30)]
        [Header("Ground & Slope & Wall Checking")]
        public Transform bottomPivot;

        [SerializeField] private Transform _groundCheckPoint1;
        [SerializeField] private Transform _groundCheckPoint2;

        public Transform _wallCheckPoint;
        private readonly float _wallCheckDistance = 0.01f;
        [SerializeField] private float _wallCheckHeight; // 너무 길면 경사도 벽으로 인식함
        [SerializeField] private float _groundCheckDistance;
        public float groundAngle;
        public int isPlayerRight = 1;
        
        private bool _isStandable; // 두 레이 모두 땅에 붙어있는지 체크 // 현재 미사용
        [HideInInspector] public Vector2 perpAngle;
        [HideInInspector] public bool isSlope;
        private RaycastHit2D groundHit1;
        private RaycastHit2D groundHit2;
        private RaycastHit2D chosenHit;
        private RaycastHit2D wallHit;
        public bool isWall;                  // 캐릭터가 벽에 붙어있는지 체크
        private Vector2 _wallCheckBoxSize;
        Coroutine _wallCheckDisplayRoutine;

        [Header("Input")]
        [HideInInspector] public float moveInput;
        

        private void Awake()
        {
            if (playerModel != null)
                playerModel.curNature = PlayerModel.Nature.Red;
            else
                Debug.LogError("모델 생성 오류");

            rigid = GetComponent<Rigidbody2D>();
            if (rigid == null)
                Debug.LogError("rigidBody없음");

            _playerCollider = GetComponent<CapsuleCollider2D>();
            _states[(int)State.Idle] = new IdleState(this);
            _states[(int)State.Run] = new RunState(this);
            _states[(int)State.Dash] = new DashState(this);
            _states[(int)State.Jump] = new JumpState(this);
            _states[(int)State.DoubleJump] = new DoubleJumpState(this);
            _states[(int)State.Fall] = new FallState(this);
            _states[(int)State.Land] = new LandState(this);
            _states[(int)State.WallGrab] = new WallGrabState(this);
            _states[(int)State.WallSliding] = new WallSlidingState(this);
            _states[(int)State.WallJump] = new WallJumpState(this);
            _states[(int)State.Damaged] = new DamagedState(this);
            _states[(int)State.WakeUp] = new WakeupState(this);
            _states[(int)State.Dead] = new DeadState(this);
            _states[(int)State.Spawn] = new SpawnState(this);
            if (playerModel != null) 
                moveSpeedInAir = playerModel.moveSpeed * speedAdjustmentOffsetInAir;

            if (bottomPivot == null)
                bottomPivot = transform.Find("BottomPivot");

            if (_groundCheckPoint1 == null)
                _groundCheckPoint1 = transform.Find("GroundCheckPoint1");

            if (_groundCheckPoint2 == null)
                _groundCheckPoint2 = transform.Find("GroundCheckPoint2");

            if (_wallCheckPoint == null)
                _wallCheckPoint = transform.Find("WallCheckPoint");

            if (_wallCheckDisplayRoutine == null)
                _wallCheckDisplayRoutine = StartCoroutine(CheckWallDisplayRoutine());

            _wallCheckBoxSize = new Vector2(_wallCheckDistance, _wallCheckHeight);
        }
        private void Start()
        {
            playerView = GetComponent<PlayerView>();
            curState = State.Spawn;
            _states[(int)curState].Enter();
            SubscribeEvents();
            _wallLayerMask = LayerMask.GetMask("Wall");
            groundLayerMask = LayerMask.GetMask("Ground");
            _ignorePlayerLayerMask = LayerMask.GetMask("Ignore Player");
        }

        private void Update()
        {
            if (Time.timeScale == 0)
                return;

            _states[(int)curState].Update();
            TagePlayer();
            CheckDashCoolTime();
            CheckWall();
            ControlCoyoteTime();
            ControlJumpBuffer();

            //죽고 리셋
            if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeState(State.Dead);
                Debug.Log("죽음");
            }


            /* 미끄럼 방지 시행착오 및 임시피격트리거

            //// 미끄러짐 방지1
            //if (player.moveInput == 0)
            //{
            //    player.rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePosition;
            //}
            //else
            //{
            //    player.rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            //}
            // 미끄럼 방지2
            //if(moveInput == 0)
            //{
            //    rigid.velocity = new Vector2(0,rigid.velocity.y);
            //
            */
            ////임시 능력 해금 트리거
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UnlockAbility(PlayerModel.Ability.Tag);
                //Debug.Log("태그 해금");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UnlockAbility(PlayerModel.Ability.Dash);
                //Debug.Log("대시 해금");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                UnlockAbility(PlayerModel.Ability.WallJump);
                //Debug.Log("벽점프 해금");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                UnlockAbility(PlayerModel.Ability.DoubleJump);
                // Debug.Log("더블점프 해금");
            }
        }

        private void FixedUpdate()
        {
            if (Time.timeScale == 0)
                return;
            _states[(int)(curState)].FixedUpdate();
            CheckGroundRaycast();
        }

        private void CheckDashCoolTime()
        {
            if (!isDashUsed)
                return;

            // 대쉬를 쓰고 쿨타임만큼 지난경우
            if (dashDeltaTime >= playerModel.dashCoolTime)
            {
                isDashUsed = false;
                //dashDeltaTime을 0으로 초기화해주는건 대시진입시
            }
            else
            {
                dashDeltaTime += Time.deltaTime;
            }
        }

        public void ChangeState(State nextState)
        {
            // 벽타기 더블점프의 예외사항 처리
            if (curState == State.WallJump && nextState == State.DoubleJump)
            {
                _states[(int)curState].Exit();
                curState = nextState;
                _states[(int)curState].Enter();
            }

            //방안2. 중복 코드를 줄임
            // 어빌리티가 해금됐는지 확인하는 과정
            // 전환되려는 상태가 해금이 필요한 상태인지, 그렇다면 플레이어가 해금한 상태인지 확인함
            if (_states[(int)nextState].ability != PlayerModel.Ability.None &&
                !HasAbility(_states[(int)nextState].ability))
                return;
            _states[(int)curState].Exit();
            curState = nextState;
            _states[(int)curState].Enter();

        /* 방안1
        //if (_states[(int)nextState].ability != Ability.None)
        //{
        //    if (HasAbility(_states[(int)nextState].ability))
        //    {
        //        _states[(int)_curState].Exit();
        //        _curState = nextState;
        //        _states[(int)_curState].Enter();
        //    }
        //    else
        //    {
        //        Debug.Log("아직 해금하지 않은 능력");
        //    }
        //}
        //else
        //{
        //    _states[(int)_curState].Exit();
        //    _curState = nextState;
        //    _states[(int)_curState].Enter();
        //}
        */
        }

        private void CheckGroundRaycast()
        {
            // 땅 체크와 땅이 평지인지 경사면인지 체크하는 메서드
            groundHit1 = Physics2D.Raycast(_groundCheckPoint1.position, Vector2.down, _groundCheckDistance, groundLayerMask);
            groundHit2 = Physics2D.Raycast(_groundCheckPoint2.position, Vector2.down, _groundCheckDistance, groundLayerMask);
            Debug.DrawLine(_groundCheckPoint1.position, (Vector2)_groundCheckPoint1.position + Vector2.down * _groundCheckDistance, Color.cyan);
            Debug.DrawLine(_groundCheckPoint2.position, (Vector2)_groundCheckPoint2.position + Vector2.down * _groundCheckDistance, Color.yellow);


            if (groundHit1 || groundHit2)
            {
                isGrounded = true;
                if (groundHit1 && groundHit2)
                {
                    //_isStandable = true; // 현재 미사용
                    //둘 중 더 distance가 더 짧은 ray를 선택
                    chosenHit = groundHit1.distance <= groundHit2.distance ? groundHit1 : groundHit2;
                }
                else
                {
                    chosenHit = groundHit1 ? groundHit1 : groundHit2;
                }
            }
            else
            {
                isGrounded = false;
            }
            // Vector2.Perpendicular(Vector2 A) : A의 값에서 반시계 방향으로 90도 회전한 벡터값을 반환

            if (!isGrounded) 
                return;
            perpAngle = Vector2.Perpendicular(chosenHit.normal).normalized; // 
            groundAngle = Vector2.Angle(chosenHit.normal, Vector2.up);

            isSlope = groundAngle != 0;

            if (groundAngle > playerModel.maxAngle)
            {
                moveInput = 0;
            }

            //법선벡터, 지면에서 수직
            Debug.DrawLine(chosenHit.point, chosenHit.point + chosenHit.normal, Color.blue);

            // 법선벡터의 수직인 벡터, 경사면
            Debug.DrawLine(chosenHit.point, chosenHit.point + perpAngle, Color.red);
        }
        private void CheckWall()
        {
            if (curState is State.Damaged or State.Dead) 
                return;

            wallHit = Physics2D.BoxCast(_wallCheckPoint.position, _wallCheckBoxSize, 0, Vector2.right * isPlayerRight, _wallCheckDistance);
            isWall = wallHit;

            if (wallHit.collider == null)
                return;

            if (wallHit.collider.isTrigger)
                return;

            if ((_ignorePlayerLayerMask & (1 << wallHit.collider.gameObject.layer)) != 0)
                return;

            if (HasAbility(PlayerModel.Ability.WallJump) && (_wallLayerMask & (1 << wallHit.collider.gameObject.layer)) != 0)// 벽타기 가능한 벽일 경우
            {
                if (isGrounded || curState == State.WallJump || curState == State.WallGrab || curState == State.WallSliding)
                    return;

                if (Mathf.Approximately(moveInput, isPlayerRight) && moveInput != 0)
                    ChangeState(State.WallGrab);
            }
            else // 벽타기 불가능한 벽이었을 경우
            {
                //float wallAngle = Vector2.Angle(Vector2.up, wallHit.normal);
                {
                    Vector2 slideDirection = Vector2.Perpendicular(wallHit.normal).normalized;
                    rigid.velocity = new Vector2(slideDirection.x * rigid.velocity.x, rigid.velocity.y);
                }
            }
        }
        public void MoveInAir()
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rigid.velocity = new Vector2(moveInput * moveSpeedInAir, rigid.velocity.y);
            FlipPlayer(moveInput);
            CheckDashable();
        }

        private void ControlCoyoteTime()
        {
            if (isGrounded)
                coyoteTimeCounter = coyoteTime;
            else
                coyoteTimeCounter -= Time.deltaTime;
        }

        private void ControlJumpBuffer()
        {
            if (Input.GetKeyDown(KeyCode.C))
                jumpBufferCounter = jumpBufferTime;
            else
                jumpBufferCounter -= Time.deltaTime;
        }

        // 어빌리티 해금을 위한 메서드
        public void UnlockAbility(PlayerModel.Ability ability)
        {
            // 이미 해금한 능력이라면 종료
            if (HasAbility(ability))
                return;
            
            // 비트연산으로 어빌리티 해금 후 이벤트 전송
            unlockedAbilities |= ability; 
            playerModel.UnlockAbilityEvent(ability);
        }

        private bool HasAbility(PlayerModel.Ability ability)
        {
            // 해당 어빌리티를 가지고 있는지 판단함
            return (unlockedAbilities & ability) == ability;
        }

        public void FlipPlayer(float moveDirection)
        {
            playerView.FlipRender(moveDirection);
            AdjustColliderOffset();
            AdjustCheckPoint();
        }

        private void AdjustCheckPoint()
        {
            _groundCheckPoint1.localPosition = new Vector2(Mathf.Abs(_groundCheckPoint1.localPosition.x) * -isPlayerRight, _groundCheckPoint1.localPosition.y);
            _groundCheckPoint2.localPosition = new Vector2(Mathf.Abs(_groundCheckPoint2.localPosition.x) * isPlayerRight, _groundCheckPoint2.localPosition.y);
            _wallCheckPoint.localPosition = new Vector2(Mathf.Abs(_wallCheckPoint.localPosition.x) * isPlayerRight, _wallCheckPoint.localPosition.y);
        }
        private void AdjustColliderOffset()
        {
            _playerCollider.offset = new Vector2(Mathf.Abs(_playerCollider.offset.x) * isPlayerRight, _playerCollider.offset.y);
        }

        private void TagePlayer()
        {
            if (!Input.GetKeyDown(KeyCode.Z)) 
                return;
            if (HasAbility(PlayerModel.Ability.Tag))
            {
                // playerView.ChangeSprite(); // 상시 애니메이션 재생 상태라 없어도 무방
                playerModel.TagPlayerEvent(); // 속성 열거형 형식의 curNature를 바꿔줌 + 태그 이벤트 Invoke
            }
            else
            {
                Debug.Log("태그 능력 해금 안됨");
            }
        }

        public void CheckDashable()
        {
            //Dash 상태로 전환
            if (moveInput == 0)
                return;
            switch (isDashUsed)
            {
                case true when Input.GetKeyDown(KeyCode.X):
                    Debug.Log("대시 쿨타임중입니다.");
                    break;
                case false when Input.GetKeyDown(KeyCode.X):
                    ChangeState(State.Dash);
                    break;
            }
        }

        private void HandlePlayerDamaged()
        {
            ChangeState(State.Damaged);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();

            if (_wallCheckDisplayRoutine != null)
                StopCoroutine(_wallCheckDisplayRoutine);
        }

        private void SubscribeEvents()
        {
            playerModel.OnPlayerDamageTaken += HandlePlayerDamaged;
        }

        private void UnsubscribeEvents()
        {
            playerModel.OnPlayerDamageTaken -= HandlePlayerDamaged;
        }

        private IEnumerator CheckWallDisplayRoutine()
        {
            WaitForSeconds delay = new WaitForSeconds(0.01f);

            //BoxCast를 통해 벽을 체크한 범위를 보여줌
            while (true)
            {
                Vector2 origin = _wallCheckPoint.position;
                Vector2 direction = Vector2.right * isPlayerRight;
                Vector2 offset = direction * _wallCheckDistance;

                Vector2 topLeft = origin + (Vector2.up * _wallCheckBoxSize.y / 2) + (Vector2.left * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;
                Vector2 topRight = origin + (Vector2.up * _wallCheckBoxSize.y / 2) + (Vector2.right * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;
                Vector2 bottomLeft = origin + (Vector2.down * _wallCheckBoxSize.y / 2) + (Vector2.left * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;
                Vector2 bottomRight = origin + (Vector2.down * _wallCheckBoxSize.y / 2) + (Vector2.right * _wallCheckBoxSize.x / 2 * isPlayerRight) + offset;

                Debug.DrawLine(topLeft, topRight, Color.green);
                Debug.DrawLine(topRight, bottomRight, Color.green);
                Debug.DrawLine(bottomRight, bottomLeft, Color.green);
                Debug.DrawLine(bottomLeft, topLeft, Color.green);
                yield return delay;
            }

        }
        //public void AdjustDash() // 미완성
        //{
        //    float boundaryOffset = 1.0f; // y축 간격
        //    float colliderHeight = 2.6f; // 콜라이더의 높이
        //    float colliderOffsetY = 0.73f; // 콜라이더의 y 오프셋
        //    float colliderOffsetX = 0.06f; // 콜라이더의 x 오프셋

        //    boxHits = Physics2D.BoxCastAll(_wallCheckPoint.position, new Vector2(_wallCheckDistance, _wallCheckHeight + 0.35f), 0, Vector2.right * isPlayerRight, _wallCheckDistance);
        //    if (boxHits.Length > 0)
        //    {
        //        float closestDistance = float.MaxValue;
        //        RaycastHit2D closestHit = new RaycastHit2D();
        //        foreach (RaycastHit2D hit in boxHits)
        //        {
        //            if (hit.distance < closestDistance)
        //            {
        //                closestDistance = hit.distance;
        //                closestHit = hit;
        //            }
        //        }

        //        if (closestHit.collider != null)
        //        {
        //            if (!isGrounded)
        //            {
        //                Vector2 hitPosition = closestHit.point;
        //                Vector2 adjustedPosition = transform.position; // 기존 플레이어 위치

        //                //// 현재 콜라이더 중앙의 y 위치
        //                //float currentColliderY = adjustedPosition.y - colliderOffsetY;

        //                //// 플레이어가 조금 내려주거나 올려주면 매끄럽게 대쉬 할 수 있을때
        //                //if (Mathf.Abs(currentColliderY - hitPosition.y) < boundaryOffset)
        //                //{
        //                //    if (currentColliderY > hitPosition.y) // 천장에 붙여야 하는 경우
        //                //    {
        //                //        Debug.Log("천장에 붙어야함");
        //                //        //// 천장에 맞게 y 위치 보정
        //                //        //adjustedPosition.y = hitPosition.y + (colliderHeight - colliderOffsetY);
        //                //    }
        //                //    else if (currentColliderY < hitPosition.y) // 바닥에 붙여야 하는 경우
        //                //    {
        //                //        Debug.Log("바닥에 붙어야함");
        //                //        // 바닥에 맞게 y 위치 보정
        //                //        adjustedPosition.y = hitPosition.y - colliderOffsetY;
        //                //    }
        //                //}

        //                // x축 오프셋 고려해 보정
        //                adjustedPosition.x = hitPosition.x - colliderOffsetX;
        //                // 순간이동
        //                transform.position = adjustedPosition;
        //                //기존 속도 유지
        //                Vector2 newVelocity = rigid.velocity;
        //                newVelocity.y = 0;
        //                rigid.velocity = newVelocity;
        //                return;
        //            }
        //        }
        //    }
        //}
        private Vector2 GetCenterOfCollider()
        {
            return (Vector2)_playerCollider.bounds.center;
        }

        /* 미완성
    public void AdjustDash()
    {
        float boundaryOffset = 1.0f; // y축 간격
        float colliderHeight = 2.6f; // 콜라이더의 높이
        float colliderOffsetY = 0.73f; // 콜라이더의 y 오프셋
        float colliderOffsetX = 0.06f; // 콜라이더의 x 오프셋

        boxHits = Physics2D.BoxCastAll(_wallCheckPoint.position, new Vector2(_wallCheckDistance, _wallCheckHeight + 0.35f), 0, Vector2.right * isPlayerRight, _wallCheckDistance);
        if (boxHits.Length > 0)
        {
            float closestDistance = float.MaxValue;
            RaycastHit2D closestHit = new RaycastHit2D();
            foreach (RaycastHit2D hit in boxHits)
            {
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestHit = hit;
                }
            }

            if (closestHit.collider != null)
            {
                if (!isGrounded)
                {
                    Vector2 hitPosition = closestHit.point;
                    Vector2 adjustedPosition = transform.position; // 기존 플레이어 위치

                    //// 현재 콜라이더 중앙의 y 위치
                    //float currentColliderY = adjustedPosition.y - colliderOffsetY;

                    //// 플레이어가 조금 내려주거나 올려주면 매끄럽게 대쉬 할 수 있을때
                    //if (Mathf.Abs(currentColliderY - hitPosition.y) < boundaryOffset)
                    //{
                    //    if (currentColliderY > hitPosition.y) // 천장에 붙여야 하는 경우
                    //    {
                    //        Debug.Log("천장에 붙어야함");
                    //        //// 천장에 맞게 y 위치 보정
                    //        //adjustedPosition.y = hitPosition.y + (colliderHeight - colliderOffsetY);
                    //    }
                    //    else if (currentColliderY < hitPosition.y) // 바닥에 붙여야 하는 경우
                    //    {
                    //        Debug.Log("바닥에 붙어야함");
                    //        // 바닥에 맞게 y 위치 보정
                    //        adjustedPosition.y = hitPosition.y - colliderOffsetY;
                    //    }
                    //}

                    // x축 오프셋 고려해 보정
                    adjustedPosition.x = hitPosition.x - colliderOffsetX;
                    // 순간이동
                    transform.position = adjustedPosition;
                    //기존 속도 유지
                    Vector2 newVelocity = rigid.velocity;
                    newVelocity.y = 0;
                    rigid.velocity = newVelocity;
                    return;
                }
            }
        }
    }
    */

        //public void Freeze()
        //{
        //    Invoke("DelayWallJump", 0.3f);
        //}

        //public void DelayWallJump()
        //{
        //    isWallJumpUsed = false;
        //}

        // 레이어 땅 체크
        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //    {
        //        isGrounded = true;
        //    }
        //}

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //    {
        //        isGrounded = false;
        //    }
        //}

        //public void MoveInAir()
        //{
        //    float moveInput = Input.GetAxisRaw("Horizontal");

        //    Vector2 targetVelocity = rigid.velocity + new Vector2(moveInput * moveSpeed*Time.deltaTime, 0);
        //    targetVelocity = Vector2.ClampMagnitude(targetVelocity, maxMoveSpeedInAir); // 속도제한
        //    rigid.velocity = targetVelocity;

        //    FlipPlayer(moveInput);

        //    //추후 개선방안을 찾아야함
        //    isWall = Physics2D.BoxCast(_wallCheckPoint.position, _wallCheckBoxSize, 0, Vector2.right * isPlayerRight, _wallCheckDistance, wallLayerMask);

        //    if (isWall && _curState != State.WallJump)
        //    {
        //        if (moveInput == isPlayerRight && moveInput != 0)
        //        ChangeState(State.WallGrab);
        //    }
        //}

        //IEnumerator CheckGroundRayRoutine()
        //{
        //    WaitForSeconds delay = new WaitForSeconds(0.1f);
        //    while (true)
        //    {

        //        //Debug.DrawRay(_groundCheckPoint.position, Vector2.down * _groundCheckDistance, Color.green);
        //        //isGrounded = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckDistance,groundLayerMask); //_rayPoint.up * -1
        //        yield return delay;
        //    }
        //}
    }
}


