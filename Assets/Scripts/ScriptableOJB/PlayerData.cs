using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string nickname;  // 플레이어 닉네임
    public string UID;          // 6자리의 랜덤 숫자
    public int currency1;    // 재화1 (예: 뼈)
    public int currency2;    // 재화2 (예: 보석)
    public float experience; // 경험치량
    public int level;        // 플레이어 레벨

    // 기본값으로 초기화
    public void InitializePlayerData(string playerName , string uid)
    {
        nickname = playerName;
        Debug.Log("nickname: "+nickname);
        UID = uid;
        currency1 = 0;  // 초기 재화1
        currency2 = 0;  // 초기 재화2
        experience = 0f; // 초기 경험치
        level = 1;       // 초기 레벨
    }
}
