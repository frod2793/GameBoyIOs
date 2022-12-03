using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObj : MonoBehaviour
{
    private float speed;
    private Rigidbody2D rigidbody;
    [SerializeField]
    RunGameUiManager Uimanager;

    private void Awake()
    {
        Uimanager = GameObject.FindObjectOfType<RunGameUiManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        speed = Uimanager.CoinSpeed;
    }

    private void OnEnable()
    {
        Vector2 vector2 = new Vector2(Uimanager.C_startpoint.x, 1);
        transform.position = vector2;
        rigidbody.constraints = RigidbodyConstraints2D.None;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mob")|| other.CompareTag("Ground"))
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        }



        if (other.CompareTag("Player"))
        {
            Uimanager.score++;
            gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Uimanager.isPlay)
        { 

            transform.Translate(Vector2.left * Time.deltaTime * speed);
            Vector2 vector = new Vector2(transform.position.x,transform.position.y);
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
}
