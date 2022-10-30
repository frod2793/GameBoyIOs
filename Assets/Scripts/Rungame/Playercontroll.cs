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
    bool isJump;
    bool isTop;


    Vector2 startPosition;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rungameUimanager.JumpBtn.onClick.AddListener(Func_jump);
        jumpHight = rungameUimanager.JumpHight;
        jumpSpeed = rungameUimanager.JumpSpeed;

        startPosition = transform.position;
    }

     void Update()
    {
        if (rungameUimanager.isPlay)
        {


            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
        jump();
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
            isJump = false;
            isTop = false;
            transform.position = startPosition;
        }




    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            rungameUimanager.Gameover();
            Debug.Log("colider");
        }
    }

}
