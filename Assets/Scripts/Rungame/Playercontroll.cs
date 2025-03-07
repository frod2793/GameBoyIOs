using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DogGuns_Games.Run
{
    public class Playercontroll : MonoBehaviour
    {
        [SerializeField] private RunGameUiManager rungameUimanager;

        private Camera mainCamera; // 카메라 참조

        //private float jumpHight;
        private float fallMultiplier = 2.5f; // 하강 속도 조절 변수 (기본값 2.5)

        private DOTweenAnimation doTweenAnimation;
        private float jumpSpeed;
        private SpriteRenderer playerSprite;

        float hit;
        bool IsJumping;
        bool isTop;

        private bool ishit;

        // Vector2 startPosition;
        Animator animator;

        private Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
            animator = GetComponent<Animator>();
            //jumpHight = rungameUimanager.JumpHight;
            jumpSpeed = rungameUimanager.JumpSpeed;
            playerSprite = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            //startPosition = transform.position;
            rungameUimanager.jumpBtn.onClick.AddListener(Func_jump);
            rungameUimanager.PlayBtn.onClick.AddListener(Init);
            rungameUimanager.contineBtn.onClick.AddListener(Init);
        }

        private void Init() //restart init
        {
            rungameUimanager.currenthp = 100;
            PlayAnimation("IsDead", false);
            rungameUimanager.jumpBtn.gameObject.SetActive(true);
        }

        void Update()
        {
            if (!isTop)
            {
                if (rungameUimanager.isPlay)
                {
                    PlayAnimation("IsWalk", true);
                }
                else
                {
                    PlayAnimation("IsWalk", false);
                }
            }


            rungameUimanager.Hpbar.value = rungameUimanager.currenthp / rungameUimanager.maxHp;

            if (rungameUimanager.isPlay)
            {
                rungameUimanager.currenthp -= rungameUimanager.discountHp;

                if (rungameUimanager.currenthp < 0)
                {
                    PlayAnimation("IsDead", true);
                    rungameUimanager.jumpBtn.gameObject.SetActive(false);
                    rungameUimanager.Gameover();
                }

                if (rb.linearVelocity.y < 0)
                {
                    // 하강 속도 조절 (fallMultiplier 사용)
                    float gravityAdjustment = (fallMultiplier - 1) * Physics2D.gravity.y * Time.deltaTime;
                    rb.linearVelocity += Vector2.up * gravityAdjustment;
                }
            }
        }

        private void Func_jump()
        {
            if (!IsJumping && Mathf.Abs(rb.linearVelocity.y) < 0.01f) // 점프 중이 아니고, 바닥에 있을 때만 점프
            {
                StartCoroutine(JumpRoutine()); // 코루틴 실행
                Debug.Log("player jump");
            }
        }

        private IEnumerator JumpRoutine()
        {
            IsJumping = true; // 점프 상태 설정
            isTop = false;

            // 애니메이터 설정
            PlayAnimation("IsJump", true);

            // 점프 시작 - Rigidbody2D의 velocity를 설정하여 위로 점프
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);

            // 최고점에 도달할 때까지 대기
            while (rb.linearVelocity.y > 0)
            {
                yield return null; // 한 프레임 대기
            }

            isTop = true; // 최고점 도달

            // 하강 및 착지 대기
            while (!isGrounded())
            {
                yield return null; // 한 프레임 대기
            }

            // 착지 후 상태 초기화
            PlayAnimation("IsJump", false);
            isTop = false; // 최고점 상태 해제
            IsJumping = false; // 점프 완료 상태 설정

            Debug.Log("player landing");
        }

        private bool isGrounded()
        {
            // 바닥에 닿았는지 체크 (충돌 감지)
            return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mob"))
            {
                hit = collision.GetComponent<MobBase>().mobDamage;
                Debug.Log(" ishit: " + ishit);
                if (!ishit)
                {
                    rungameUimanager.currenthp -= hit;
                }

                ShakeCamera();


                Debug.Log("hit: " + hit);
                if (rungameUimanager.currenthp < 0)
                {
                    PlayAnimation("IsDead", true);
                    rungameUimanager.Gameover();
                }
                else
                {
                    Debug.Log(" doTweenAnimation.DOPlay();: " + hit);
                    //dotween 을 사용하여 0.2속도로 playerSprite가  10번 깜빡이는 효과
                    FlashEffect(5, 0.2f); // 깜빡임 효과 호출
                    ishit = true;
                }

                Debug.Log("colider");
            }

            if (collision.CompareTag("Heal"))
            {
                rungameUimanager.currenthp += 50f;
            }
        }

        private void FlashEffect(int flashes, float flashSpeed)
        {
            // 투명도를 깜빡이기 위해 1(보이기)와 0(투명)을 반복하는 트위닝
            playerSprite.DOFade(0, flashSpeed).SetLoops(flashes * 2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    // 깜빡임이 끝나면 ishit을 false로 초기화
                    playerSprite.color = Color.white;
                    ishit = false;
                });
        }

        private void ShakeCamera()
        {
            // DOTween을 사용하여 카메라를 흔들림 효과를 줍니다.
            mainCamera.transform
                .DOShakePosition(0.5f, strength: new Vector3(0.5f, 0.5f, 0), vibrato: 10, randomness: 90,
                    snapping: false, fadeOut: true).OnComplete(() =>
                {
                    mainCamera.transform.position = new Vector3(0, 0.19f, -10);
                });

            // 흔들림 매개변수 설명:
            // - 지속 시간: 0.5초 동안 흔들림
            // - 힘 (strength): X, Y축으로 0.5의 강도로 흔들림
            // - vibrato: 흔들림의 진동 횟수 (10번)
            // - randomness: 랜덤하게 흔들리는 정도 (90도)
            // - fadeOut: 흔들림이 점점 감소하며 종료
        }

        private void PlayAnimation(string animationName, bool isTrue)
        {
            animator.SetBool(animationName, isTrue);
        }
    }
}