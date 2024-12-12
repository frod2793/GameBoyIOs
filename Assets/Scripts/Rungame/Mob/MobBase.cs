using UnityEngine;

namespace DogGuns_Games.Run
{
    public class MobBase : MonoBehaviour
    {
        private float mobSpeed;
        [SerializeField] RunGameUiManager uImanager;

        public float mobDamage;
        public string mobName;

        // Start is called before the first frame update
        void Awake()
        {
            uImanager = FindAnyObjectByType<RunGameUiManager>();
            mobSpeed = uImanager.MobSpeed;
        }

        private void OnEnable()
        {
            Vector2 vector = new Vector2(uImanager.startpoint.x, transform.position.y);
            transform.position = vector;
        }


        // Update is called once per frame
        void Update()
        {
            if (uImanager.isPlay)
            {
                transform.Translate(Vector2.left * Time.deltaTime * mobSpeed);

                if (transform.localPosition.x < -10)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}