using UnityEngine;

namespace DogGuns_Games.vamsir
{
    public class VamserLike_GameManager : MonoBehaviour
    {
        private Player_Base[] player;
        [SerializeField] private GameObject inGameObject_parent;
        
        private Vector3 SpawnPosition = new Vector3(0, 0, 0);
        
        private void Awake()
        {
            Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex = 0; //임시 

            SpawnPlayer();
        }


        private void SpawnPlayer()
        {
            player = Resources.LoadAll<Player_Base>("Player_Character");

            for (int i = 0; i < player.Length; i++)
            {
                if (Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex == player[i].CharacterIndex)
                {
                    Instantiate(player[i].gameObject, SpawnPosition, Quaternion.identity,
                        inGameObject_parent.transform);
                }
            }
        }
    }
}