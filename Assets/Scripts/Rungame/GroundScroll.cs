using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    [SerializeField]
    private RunGameUiManager gameUiManager;
    [SerializeField]
    private Sprite[] groundimag;
    private SpriteRenderer[] tiles;
    SpriteRenderer temp;
    private float groundSpeed;
    // Start is called before the first frame update
    private void Awake()
    {
       
        tiles = gameUiManager.tiles;
        groundimag = gameUiManager.GroundImage;
        groundSpeed = gameUiManager.Groundspeed;
    }

    private void Start()
    {
        temp =tiles[0];
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameUiManager.isPlay)
        {



            for (int i = 0; i < tiles.Length; i++)
            {
                if (-6 >= tiles[i].transform.position.x)
                {
                    for (int q = 0; q < tiles.Length; q++)
                    {
                        if (temp.transform.position.x < tiles[q].transform.position.x)
                        {
                            temp = tiles[q];
                        }
                        tiles[i].transform.position = new Vector2(5 + 1, -0.3f);
                        tiles[i].sprite = groundimag[Random.Range(0, groundimag.Length)];
                    }
                }
            }
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * groundSpeed);
            }
        }
    }
}
