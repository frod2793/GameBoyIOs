using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfil_Manager : MonoBehaviour
{
 
    [Header("플레이어 프로필 생성 화면")]
    [SerializeField]
    private GameObject PlayerProfil_Create_PopUp;
    [SerializeField]
    private TMP_InputField NickName_InputField;
    [SerializeField]
    private Button CreateBtn;
    
    
    [SerializeField]
    private Button StartBtn;
    
    private PlayerData playerData;
    private string savePath;
    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        
        SearchPlayerData();
        StartBtn.onClick.AddListener(Func_StartBtn);
        CreateBtn.onClick.AddListener(Func_CreateBtn);
    }

    
    private void SearchPlayerData()
    {
        if (File.Exists(savePath))
        {
            LoadPlayerData();
        }
        else
        {
            PlayerProfil_Create_PopUp.SetActive(true);
            StartBtn.interactable = false;
        }
    }
    
    private void Func_CreateBtn()
    {
        if (NickName_InputField.text != "")
        {
            PlayerProfil_Create_PopUp.SetActive(false);
        }
        int Uid = Random.Range(100000, 999999);
        //생성한 닉네임뒤에 #을 붙인후 랜덤6자리 숫자를 붙여준다.
       
        CreateNewPlayerData(NickName_InputField.text, Uid);
    }

    private void CreateNewPlayerData(string playerName, int uid)
    {
        playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.InitializePlayerData(playerName, uid);
        StartBtn.interactable = true;
        SavePlayerData(); // 새로 생성한 데이터를 저장
    }
    public void SavePlayerData()
    {
        // ScriptableObject를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(playerData, true);
        
        // 파일에 저장
        File.WriteAllText(savePath, jsonData);
        Debug.Log("PlayerData saved to: " + savePath);
    }
    
    
    public void LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            // JSON 파일을 읽어와 ScriptableObject에 덮어씌움
            string jsonData = File.ReadAllText(savePath);
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            JsonUtility.FromJsonOverwrite(jsonData, playerData);
            Debug.Log("PlayerData loaded from: " + savePath);
        }
        else
        {
            Debug.LogWarning("No PlayerData file found at: " + savePath);
        }
    }
    
    private void Func_StartBtn()
    {
    
    }
    
}
