using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerObj : MonoBehaviour
{
    [SerializeField]
    Transform player;
    private float biggersize = 0.74f;
    private float smallersize = 0.24f;
    public float speed = 1f;

    private float movespeed;
    private Rigidbody2D rigidbody;
    [SerializeField]
    RunGameUiManager Uimanager;

    private float time=1;
    private Vector2 originalscale;
    // Update is called once per frame
    private void Awake()
    {
        player = GameObject.FindObjectOfType<Playercontroll>().GetComponent<Transform>();
        originalscale = player.localScale;


        Uimanager = GameObject.FindObjectOfType<RunGameUiManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        movespeed = Uimanager.CoinSpeed;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Q))
        {
            StartCoroutine(Bigger());
            Debug.Log("bigger");
        }
#endif
        if (Uimanager.isPlay)
        {

            transform.Translate(Vector2.left * Time.deltaTime * movespeed);
            Vector2 vector = new Vector2(transform.position.x, transform.position.y);
            rigidbody.MovePosition(vector);

            if (transform.position.x < -6)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mob") || other.CompareTag("Ground"))
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        }

        if (other.CompareTag("Player"))
        {
            StartCoroutine(Bigger());
        }

        if (other.CompareTag("Heal"))
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Bigger()
    {
        while (player.localScale.x < biggersize)
        {
            player.localScale = originalscale * (1f + time * speed);
            Debug.Log("bigger11");
            if (player.localScale.x>=biggersize)
            {
                time = 0;
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        Debug.Log("bigger22");
        StartCoroutine(smaller());
    }

    private IEnumerator smaller()
    {
        while (player.localScale.x > smallersize)
        {
            Debug.Log("bigger33");
            player.localScale = originalscale * (0.1f + time * speed);
            if (player.localScale.x <= smallersize)
            {
                time = 0;
                break;
            }
            yield return null;
        }
    }


}
