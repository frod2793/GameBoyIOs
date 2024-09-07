using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroll : MonoBehaviour
{
    [SerializeField]
    private RunGameUiManager rungameUimanager;

    [SerializeField]
    private float jumpHight;
    [SerializeField]
    private float jumpSpeed;
 
    float hit;
    bool isJumping;
    bool isTop;

    Vector2 startPosition;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rungameUimanager.JumpBtn.onClick.AddListener(Func_jump);
        rungameUimanager.PlayBtn.onClick.AddListener(Init);
        jumpHight = rungameUimanager.JumpHight;
        jumpSpeed = rungameUimanager.JumpSpeed;

        startPosition = transform.position;
    }

    private void Init()//restart init
    {
        rungameUimanager.currenthp = 100;
        animator.SetBool("isDead", false);
        rungameUimanager.JumpBtn.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isTop)
        {
            if (rungameUimanager.isPlay)
            {
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
            }
        }

        //jump();


        rungameUimanager.Hpbar.value = rungameUimanager.currenthp / rungameUimanager.maxHp;

        if (rungameUimanager.isPlay)
        {
            rungameUimanager.currenthp = rungameUimanager.currenthp - rungameUimanager.discountHp;

            if (rungameUimanager.currenthp < 0)
            {
                animator.SetBool("isDead", true);
                rungameUimanager.JumpBtn.gameObject.SetActive(false);
                rungameUimanager.Gameover();
            }
        }

    }
    // Update is called once per frame
    private void Func_jump()
    {
        if (!isJumping) // 점프 중이 아닐 때만 점프 실행
        {
            StartCoroutine(JumpRoutine()); // 코루틴 실행
            Debug.Log("player jump");
        }
    }

    private IEnumerator JumpRoutine()
    {
        isJumping = true; // 점프 상태 설정

        // 애니메이터 설정
        animator.SetBool("isJump", true);

        // 상승 (점프 최고점까지 이동)
        while (transform.position.y < jumpHight - 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position,
                new Vector2(transform.position.x, jumpHight), jumpSpeed * Time.deltaTime);

            yield return null; // 한 프레임 대기
        }

        // 최고점 도달 후 하강
        isTop = true; // 최고점 도달 플래그 설정

        while (transform.position.y > startPosition.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
            yield return null; // 한 프레임 대기
        }

        // 점프 종료 후 초기화
        transform.position = startPosition; // 정확한 위치로 설정
        animator.SetBool("isJump", false);  // 애니메이션 해제
        isTop = false;   // 최고점 상태 해제
        isJumping = false; // 점프 완료 상태 설정

        Debug.Log("player landing");
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            hit = collision.GetComponent<MobBase>().mobDamage;

            rungameUimanager.currenthp = rungameUimanager.currenthp - hit ;

            if (rungameUimanager.currenthp < 0)
            {
                animator.SetBool("isDead", true);
            
            rungameUimanager.Gameover();
            }
            else
            {
                animator.SetTrigger("isHit");
            }

            Debug.Log("colider");
        }

        if (collision.CompareTag("Heal"))
        {
            rungameUimanager.currenthp = rungameUimanager.currenthp + 50f;
        }

    }

}
