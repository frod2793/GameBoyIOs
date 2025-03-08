using DogGuns_Games.Lobby;
using TMPro;
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
    
    [Header("<color=green> 우편함 관리")]
    [FormerlySerializedAs("MessingerPanel")] 
    [SerializeField] private GameObject postBoxPanel;
    [SerializeField] private GameObject postboxcontainer;
    [SerializeField] private Post_Index postboxPrefab;
    [Header("<color=green> 확장 패널")]
   [SerializeField] GameObject postBoxPanel_Extension;
    [SerializeField] TMP_Text postBoxPanel_Extension_Text;// 상세 내용
    
    // todo: 프리펩에 메시지 와 물품 아이템 코드를 저장하고 받을수 있는 클래스를 생성하여 사용 
//todo: 메시지를 받을시 메시지를 어떻게 처리 할것인가 

    private void addPost_index()
    {
        Post_Index postIndex = Instantiate(postboxPrefab, postboxcontainer.transform);
        postIndex.SetPostIndex("name", "title", "date", null);
        
    }
    
    public void OpenPostBoxPanel()
    {
        
        postBoxPanel.SetActive(true);
    }


    private void ConfiromReword(int itemCode)
    {
        Inventory_Data_Manager_Dontdestory.Instance.GetItemByItemCode(itemCode);
    }
}

