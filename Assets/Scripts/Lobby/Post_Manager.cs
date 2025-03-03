using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Post_Manager : MonoBehaviour
{

    struct postmessage
    {
        string message;
        string sender;
        string date;
    }
    
    [FormerlySerializedAs("MessingerPanel")] 
    [SerializeField] private GameObject postBoxPanel;
    [SerializeField] private GameObject postboxcontainer;
    [SerializeField] private GameObject postboxPrefab;
    
    
    // todo: 프리펩에 메시지 와 물품 아이템 코드를 저장하고 받을수 있는 클래스를 생성하여 사용 
    
    
    
    
}

