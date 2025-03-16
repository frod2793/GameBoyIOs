
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DogGuns_Games.Lobby
{
    public class Post_Manager : MonoBehaviour
    {
        struct postmessage
        {
            string message;
            string sender;
            string date;
        }

        [Header("<color=green> 우편함 관리")] [SerializeField]
        private GameObject postBoxPanel;

        [SerializeField] private GameObject postboxcontainer;
        [SerializeField] private Post_Index postboxPrefab;
        [SerializeField] private Button postBoxPanel_CloseBtn;

        [Header("<color=green> 확장 패널")] [SerializeField]
        GameObject postBoxPanel_Extension;

        [SerializeField] TMP_Text postBoxPanel_Extension_Text; // 상세 내용
        [SerializeField] TMP_Text postBoxSendername_Text;
        [SerializeField] TMP_Text rewardItemName_Text;
        [SerializeField] private Button postBoxPanel_Extension_CloseBtn;


        #region panelUI

        private void Start()
        {
            postBoxPanel_CloseBtn.onClick.AddListener(() => postBoxPanel.SetActive(false));
            postBoxPanel_Extension_CloseBtn.onClick.AddListener(() => postBoxPanel_Extension.SetActive(false));
            addPost_index();
        }

        public void OpenPostBoxPanel()
        {
            postBoxPanel.SetActive(true);
        }

        public void OpenPostBoxPanel_Extension(string message, string sender, string rewardItemName, int itemCode)
        {
            postBoxPanel_Extension.SetActive(true);
            postBoxPanel_Extension_Text.text = message;
            postBoxSendername_Text.text = sender;
            rewardItemName_Text.text = rewardItemName;
            postBoxPanel_Extension_CloseBtn.onClick.AddListener(() => ConfiromReword(itemCode));
        }

        public void ClosePostBoxPanel_Extension()
        {
            postBoxPanel_Extension.SetActive(false);
        }

        #endregion


        // todo: 프리펩에 메시지 와 물품 아이템 코드를 저장하고 받을수 있는 클래스를 생성하여 사용 
        //todo: 메시지를 받을시 메시지를 어떻게 처리 할것인가 

        private void addPost_index()
        {
            if (postboxPrefab == null || postboxcontainer == null)
            {
                Debug.LogError("Postbox prefab or container is null!");
                return;
            }

            Post_Index postIndex = Instantiate(postboxPrefab, postboxcontainer.transform);

            if (postIndex == null)
            {
                Debug.LogError("Failed to instantiate postbox prefab!");
                return;
            }

            // 샘플 데이터
            string message = "Sample message";
            string sender = "Sender name";
            string rewardItemName = "Gold";
            int itemCode = 1001;

            UnityEngine.Events.UnityEvent clickEvent = new UnityEngine.Events.UnityEvent();
            clickEvent.AddListener(() => OpenPostBoxPanel_Extension(message, sender, rewardItemName, itemCode));

            postIndex.SetPostIndex(sender, message, "date", clickEvent);
        }

        private void ConfiromReword(int itemCode)
        {
            Inventory_Data_Manager_Dontdestory.Instance.GetItemByItemCode(itemCode);
        }
    }
}