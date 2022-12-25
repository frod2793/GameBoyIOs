using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerObj : MonoBehaviour
{
    private float movespeed;
    private Rigidbody2D rigidbody;
    [SerializeField]
    RunGameUiManager Uimanager;

    ObjEffectManager ObjEffect;
    // Update is called once per frame
    private void Awake()
    {
        ObjEffect = GameObject.FindObjectOfType<ObjEffectManager>();
        Uimanager = GameObject.FindObjectOfType<RunGameUiManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        movespeed = Uimanager.CoinSpeed;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Q))
        {
            //ObjEffect.Effect_Bigger();
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
          ObjEffect.Effect_Bigger();
        }

        if (other.CompareTag("Heal"))
        {
            gameObject.SetActive(false);
        }
    }




}
