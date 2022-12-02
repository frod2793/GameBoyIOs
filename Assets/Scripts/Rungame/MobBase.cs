using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBase : MonoBehaviour
{
    private float mobSpeed;
    [SerializeField]
    RunGameUiManager UImanager;

  
   
    // Start is called before the first frame update
    void Awake()
    {
        UImanager = GameObject.FindObjectOfType<RunGameUiManager>();

        mobSpeed = UImanager.MobSpeed;
    }

    private void OnEnable()
    {
        Vector2 vector = new Vector2(UImanager.startpoint.x, transform.position.y);
        transform.position = vector;

    }


    // Update is called once per frame
    void Update()
    {
        if (UImanager.isPlay)
        {
          


            transform.Translate(Vector2.left * Time.deltaTime * mobSpeed);

            if (transform.position.x < -6)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
