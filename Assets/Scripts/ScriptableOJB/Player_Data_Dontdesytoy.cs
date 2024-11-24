using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class Player_Data_Dontdesytoy : MonoBehaviour
{
    [FormerlySerializedAs("playerData")] public PlayerData scritpableobj_playerData;
    
    //파괴되지않는 오브젝트
    private static Player_Data_Dontdesytoy instance;
    public static Player_Data_Dontdesytoy Instance
    {
        get
        {
            if (IsNullOrEmpty(instance) )
            {
                instance = FindAnyObjectByType<Player_Data_Dontdesytoy>();
                if (IsNullOrEmpty(instance))
                {
                    GameObject container = new GameObject("Player_Data");
                    instance = container.AddComponent<Player_Data_Dontdesytoy>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        if (scritpableobj_playerData == null)
        {
            scritpableobj_playerData =   ScriptableObject.CreateInstance<PlayerData>();
        }
    
    }
     
    public void SavePlayerData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        string jsonData = JsonUtility.ToJson(scritpableobj_playerData, true);
        File.WriteAllText(savePath, jsonData);
    }
    
   
    public void UpdatePlayerData(PlayerData playerData)
    {
        scritpableobj_playerData = playerData;
    }
    
    private static bool IsNullOrEmpty(Object value)
    {
        return ReferenceEquals(value, null);
    }
}
