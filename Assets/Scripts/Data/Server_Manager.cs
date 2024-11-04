using BackEnd;
using UnityEngine;
// using UnityGoogleDrive;
public class Server_Manager : MonoBehaviour
{
    //파괴되지않는 오브젝트
    private static Server_Manager instance;
    public static Server_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Server_Manager>();
                if (instance == null)
                {
                    GameObject container = new GameObject("Server_Manager");
                    instance = container.AddComponent<Server_Manager>();
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
        BackEndInIt();
    }
    void BackEndInIt() {
        var bro = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if(bro.IsSuccess()) {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        } else {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }
    }

 
}
