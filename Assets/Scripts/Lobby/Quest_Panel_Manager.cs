using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DogGuns_Games.Lobby
{

    public class Quest_Panel_Manager : MonoBehaviour
    {
        #region 변수

        [Header("<color=green> 퀘스트 패널")] [SerializeField]
        private GameObject questPanel;

        [SerializeField] private GameObject questContainer;

        [SerializeField]private Quest_Index questPrefab;
        [SerializeField]private Button all_comfirmBtn;
        [SerializeField] private Button questPanel_CloseBtn;

        [Header("<color=green> 확장 패널")] [SerializeField]
        private GameObject questPanel_Extension;

        [SerializeField] private TMP_Text questPanel_Extension_Text; // 상세 내용
        [SerializeField] private TMP_Text rewardItemName_Text;
        [SerializeField] private Button questPanel_Extension_CloseBtn;

        #endregion


        private void Start()
        {
            questPanel_CloseBtn.onClick.AddListener(() => questPanel.SetActive(false));
            questPanel_Extension_CloseBtn.onClick.AddListener(() => questPanel_Extension.SetActive(false));
            addQuest_index();
        }

        #region panelUI

        public void OpenQuestPanel()
        {
            questPanel.SetActive(true);
        }

        public void OpenQuestPanel_Extension(string message, string questName, string rewardItemName, int itemCode)
        {
            questPanel_Extension.SetActive(true);
            questPanel_Extension_Text.text = message;
          
            rewardItemName_Text.text = rewardItemName;
            questPanel_Extension_CloseBtn.onClick.AddListener(() => ConfiromReword(itemCode));
        }

        public void CloseQuestPanel_Extension()
        {
            questPanel_Extension.SetActive(false);
        }

        private void ConfiromReword(int itemCode)
        {
            Inventory_Data_Manager_Dontdestory.Instance.GetItemByItemCode(itemCode);
        }

        private void addQuest_index()
        {
            
            for (int i = 0; i < 10; i++)
            {
                Quest_Index questIndex = Instantiate(questPrefab, questContainer.transform);
                questIndex.SetQuestIndex("퀘스트" + i);
         
            }
        }

        #endregion



    }
}