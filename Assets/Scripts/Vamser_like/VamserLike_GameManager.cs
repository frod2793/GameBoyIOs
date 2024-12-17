using UnityEngine;
using UnityEngine.Serialization;

namespace DogGuns_Games.vamsir
{
    public class VamserLike_GameManager : MonoBehaviour
    {
        private Player_Base[] _playerBases;
     [SerializeField] private GameObject inGameObjectParent;
        
        private Vector3 _spawnPosition = new Vector3(0, 0, 0);
        [SerializeField] 
        private OptionPopupManager optionPopupManager;
        public SettingsData_oBJ settingsData;
        private void Awake()
        {
            Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex = 0; //임시 

            SpawnPlayer();
        }


        private void SpawnPlayer()
        {
            _playerBases = Resources.LoadAll<Player_Base>("Player_Character");

            for (int i = 0; i < _playerBases.Length; i++)
            {
                if (Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex == _playerBases[i].CharacterIndex)
                {
                    Instantiate(_playerBases[i].gameObject, _spawnPosition, Quaternion.identity,
                        inGameObjectParent.transform);
                }
            }
        }
        
        
        public void Open_OptionPopUp()
        {
            Instantiate(optionPopupManager);
            optionPopupManager.gameObject.SetActive(true);
        }
        
    }
}