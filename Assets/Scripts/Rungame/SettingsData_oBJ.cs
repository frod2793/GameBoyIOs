using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[CreateAssetMenu(fileName = "SettingsData", menuName = "GameSettings/SettingsData")]

public class SettingsData_oBJ : ScriptableObject
{
    public float backgroundSoundVolume = 0.5f;  // 기본 배경음 볼륨
    public float effectSoundVolume = 0.5f;      // 기본 효과음 볼륨
    public bool ConSmall_toggle = true;         // 작은 버튼 기본값
    public bool ConNormal_Toggle = false;       // 중간 버튼 기본값
    public bool ConBigBtn_Toggle = false;       // 큰 버튼 기본값
    public int JoystickType = 0;       // 조이스틱 타입 기본값
    public float JoystickSize = 1;       // 조이스틱 크기 기본값
    public Vector2 JoystickPos = new Vector2(0.5f, 0.5f);       // 조이스틱 위치 기본값
  
    
    private static string filePath => Path.Combine(Application.persistentDataPath, "settingsData.json");

    // JSON으로 데이터 저장
    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(this, true); // ScriptableObject 데이터를 JSON으로 직렬화
        File.WriteAllText(filePath, jsonData);  // 파일에 저장
        Debug.Log("Settings saved to: " + filePath);
    }

    // JSON 파일에서 데이터 불러오기
    public void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            // JSON 데이터를 읽어와 ScriptableObject에 덮어쓰기
            string jsonData = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(jsonData, this);  // ScriptableObject에 불러온 JSON 데이터 덮어쓰기
            Debug.Log("Settings loaded from: " + filePath);
        }
        else
        {
            // 파일이 없으면 기본값으로 저장
            Debug.LogWarning("No settings file found. Saving default settings.");
            SaveSettings(); // 기본값을 사용하여 JSON 파일로 저장
        }
    }

}
