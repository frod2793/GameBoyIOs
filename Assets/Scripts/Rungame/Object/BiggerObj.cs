using UnityEngine;

namespace DogGuns_Games.Run
{
    public class BiggerObj : Obect_base
    {
        ObjEffectManager objEffect;

        // Update is called once per frame
        protected override void BaseInit()
        {
            base.BaseInit();
            objEffect = FindAnyObjectByType<ObjEffectManager>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.Q))
            {
                //objEffect.Effect_Bigger();
                Debug.Log("bigger");
            }
#endif
            if (uimanager.isPlay)
            {
                float movement = Speed * Time.deltaTime;
                transform.Translate(Vector2.left * movement);
                Vector2 vector = new Vector2(transform.position.x, transform.position.y);
                objRigidbody.MovePosition(vector);
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
                objRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            }

            if (other.CompareTag("Player"))
            {
                objEffect.Effect_Bigger();
                gameObject.SetActive(false);
            }

            if (other.CompareTag("Heal"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}