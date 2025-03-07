using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace DogGuns_Games
{
    public class Player_Data_Manager_Dontdesytoy : MonoBehaviour
    {
        public PlayerData scritpableobj_playerData;

        public int SelectWeaponIndex
        {
            get => scritpableobj_playerData.selelcWeaponIndex;
            set => scritpableobj_playerData.selelcWeaponIndex = value;
        }
        public int SelectCharacterIndex
        {
            get => scritpableobj_playerData.selectCharacterIndex;
            set => scritpableobj_playerData.selectCharacterIndex = value;
        }
        
        #region DontDestroyOnLoad

        //파괴되지않는 오브젝트
        private static Player_Data_Manager_Dontdesytoy instance;

        public static Player_Data_Manager_Dontdesytoy Instance
        {
            get
            {
                if (IsNullOrEmpty(instance))
                {
                    instance = FindAnyObjectByType<Player_Data_Manager_Dontdesytoy>();
                    if (IsNullOrEmpty(instance))
                    {
                        GameObject container = new GameObject("Player_Data");
                        instance = container.AddComponent<Player_Data_Manager_Dontdesytoy>();
                        DontDestroyOnLoad(container);
                    }
                }

                return instance;
            }
        }

        #endregion

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            
            if (scritpableobj_playerData == null)
            {
                scritpableobj_playerData = ScriptableObject.CreateInstance<PlayerData>();
            }
            
        }
        
        /// <summary>
        ///  플레이어 데이터 로컬 저장
        /// </summary>
        public void SavePlayerData()
        {
            string savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            string jsonData = JsonUtility.ToJson(scritpableobj_playerData, true);
            File.WriteAllText(savePath, jsonData);
        }

        /// <summary>
        ///  플레이어 데이터 업데이트
        /// </summary>
        /// <param name="playerData"></param>
        public void UpdatePlayerData(PlayerData playerData)
        {
            scritpableobj_playerData = playerData;
        }

        private static bool IsNullOrEmpty(Object value)
        {
            return ReferenceEquals(value, null);
        }
    }
}