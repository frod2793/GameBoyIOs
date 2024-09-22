using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjEffectManager : MonoBehaviour
{
    [SerializeField]
    Transform player;
    private Vector2 originalscale;
    private float biggersize = 0.55f;
    private float smallersize = 0.24f;
    public float speed = 0.01f;

    private float time = 1f;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Playercontroll>().GetComponent<Transform>();
        originalscale = player.localScale;

    }

    public void Effect_Bigger()
    {
       StartCoroutine(Bigger());
    }

    IEnumerator Bigger()
    {


        while (player.localScale.x < biggersize)
        {
            player.localScale = originalscale * (1f + time * speed);
            Debug.Log("bigger11");
            time += Time.deltaTime;
            if (player.localScale.x >= biggersize)
            {
                time = 0;
                yield return new WaitForSeconds(5f);

               yield return StartCoroutine(smaller());
                break;
            }
            yield return null;
        }
    
  
        // StartCoroutine(smaller());
    }


    private IEnumerator smaller()
    {
        while (player.localScale.x > smallersize)
        {
            Debug.Log("bigger33");
            player.localScale = originalscale * (1f - time * speed);
            time += Time.deltaTime;
            if (player.localScale.x <= smallersize)
            {
                time = 0;
                break;
            }
            yield return null;
        }
    }
}
