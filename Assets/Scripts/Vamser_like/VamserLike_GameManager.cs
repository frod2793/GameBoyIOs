using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DogGuns_Games.vamsir
{
    public class VamserLike_GameManager : MonoBehaviour
    {
        private Player_Base[] _playerBases;
        private Weaphon_base[] _weaphonBases;
        [SerializeField] private GameObject inGameObjectParent;
        
        private Vector3 _spawnPosition = new Vector3(0, 0, 0);
        [SerializeField] 
        private OptionPopupManager optionPopupManager;
        public SettingsData_oBJ settingsData;
        
        private ObjectPool_Spawner _objectPoolSpawner;
        
        private void Awake()
        {
            Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex = 0; //임시 
            Player_Data_Manager_Dontdesytoy.Instance.SelectWeaponIndex = 0; //임시
            
            _objectPoolSpawner = FindFirstObjectByType<ObjectPool_Spawner>();
            
            Play_State.OnGameStart += GameStart;
            Play_State.OnGamePause += Pause;
            Play_State.OnGameResume += Resume;
           
        }
        
        
        private void GameStart()
        {
            Play_State.instance.isPlay = true;
            SpawnPlayer();
        }
        
        private void Pause()
        {
            Play_State.instance.isPlay = false;
        }
        
        private void Resume()
        {
            Play_State.instance.isPlay = true;
        }
        
        private void SpawnPlayer()
        {
            if (Play_State.instance.isPlay)
            {
                _weaphonBases = Resources.LoadAll<Weaphon_base>("Weaphon");
                for (int i = 0; i < _weaphonBases.Length; i++)
                {
                    if (Player_Data_Manager_Dontdesytoy.Instance.SelectWeaponIndex == _weaphonBases[i].weaphonIndex)
                    {
                        Instantiate(_weaphonBases[i].gameObject, _spawnPosition, Quaternion.identity,
                            inGameObjectParent.transform);
                    }
                }
                
                _playerBases = Resources.LoadAll<Player_Base>("Player_Character");

                for (int i = 0; i < _playerBases.Length; i++)
                {
                    if (Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex == _playerBases[i].characterIndex)
                    {
                        Instantiate(_playerBases[i].gameObject, _spawnPosition, Quaternion.identity,
                            inGameObjectParent.transform);
                    }
                }

                
            }
        }
        
        
        public void Open_OptionPopUp()
        {
            
            Instantiate(optionPopupManager);
            optionPopupManager.gameObject.SetActive(true);
            
            Play_State.instance.PlayState = Play_State.GameState.Pause;
        }
        public int Mobcount()
        {
            return _objectPoolSpawner.MobCount;
        }
        
        public int MobSpawnWave()
        {
            return _objectPoolSpawner.MobSpawnWave;
        }
        
        public float PlayerLevel()
        {
            return _playerBases[Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex].Level;
        }
        
        public int CoinCount()
        {
            return Player_Data_Manager_Dontdesytoy.Instance.scritpableobj_playerData.currency1;
        }
    }
}