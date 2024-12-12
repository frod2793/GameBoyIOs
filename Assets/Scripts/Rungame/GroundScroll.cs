using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogGuns_Games.Run
{


    public class GroundScroll : MonoBehaviour
    {
        [SerializeField] private RunGameUiManager gameUiManager;
        [SerializeField] private Sprite[] groundimag;
        private SpriteRenderer[] tiles;
        private float groundSpeed;

        float leftPosX = 0f;
        float rightPosX = 0f;
        float xScreenHalfSize;
        //  float yScreenHalfSize;


        // Start is called before the first frame update
        private void Awake()
        {

            tiles = gameUiManager.tiles;
            groundimag = gameUiManager.GroundImage;
            groundSpeed = gameUiManager.Groundspeed;
        }

        private void Start()
        {
            xScreenHalfSize = -12.64f;
            leftPosX = (xScreenHalfSize * 1f);
            rightPosX = 36.36f; //-xScreenHalfSize * 1f * tiles.Length;
        }

        void Update()
        {
            if (gameUiManager.isPlay)
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    tiles[i].transform.position += new Vector3(-groundSpeed, 0, 0) * Time.deltaTime;

                    if (tiles[i].transform.position.x < leftPosX)
                    {
                        Vector3 nextPos = tiles[i].transform.position;
                        nextPos = new Vector3(nextPos.x + rightPosX, nextPos.y, nextPos.z);
                        tiles[i].transform.position = nextPos;
                    }
                }
            }
        }
    }
}