using UnityEngine;

namespace DogGuns_Games.Run
{
    public class CoinObj : Obect_base
    {
        private void OnEnable()
        {
            Vector2 vector2 = new Vector2(uimanager.C_startpoint.x, 1);
            transform.position = vector2;
            objRigidbody.constraints = RigidbodyConstraints2D.None;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Mob") || other.CompareTag("Ground"))
            {
                objRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            }


            if (other.CompareTag("Player"))
            {
                uimanager.score++;
                gameObject.SetActive(false);
            }


            if (other.CompareTag("Heal"))
            {
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (uimanager.isPlay)
            {
                transform.Translate(Vector2.left * Time.deltaTime * Speed);
                Vector2 vector = new Vector2(transform.position.x, transform.position.y);
                objRigidbody.MovePosition(vector);

                if (transform.position.x < -6)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}