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
    bool isJump;
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

        
          jump();


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


        isJump = true;

      
        Debug.Log("player jump");

    }

    void jump()
    {
       
        if (isJump)
        {
            animator.SetBool("isJump", true);
            if (transform.position.y <= jumpHight - 0.1f && !isTop)
            {
              
               transform.position = Vector2.Lerp(transform.position,
                  new Vector2(transform.position.x, jumpHight), jumpSpeed * Time.deltaTime);
              
            }
            else
            {
                isTop = true;
            }
            if (transform.position.y > startPosition.y && isTop)
            {
                transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
            }

        }
        

        if (isJump && transform.position.y <= startPosition.y)
        {
            isJump = false;//jump
            isTop = false;
            animator.SetBool("isJump", isTop);
            transform.position = startPosition;
        }




    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            hit = collision.GetComponent<MobBase>().Damage;

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
