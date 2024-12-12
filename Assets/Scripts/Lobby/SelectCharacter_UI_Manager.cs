using UnityEngine;

namespace DogGuns_Games.Lobby
{
    public class SelectCharacter_UI_Manager : MonoBehaviour
    {
        [Header("캐릭터 선택창")] [SerializeField] private GameObject characterSelectPanel;
        [Header("캐릭터 스킬뷰")] [SerializeField] private GameObject characterSkillViewPanel;
        [Header("캐릭터 리스트")] [SerializeField] private GameObject characterListPanel;


        public void OpenCharacterSelectPanel()
        {
            Func_ActiveGameObject(characterSelectPanel, true);
        }

        public void OpenCharacterListPanel()
        {
            Func_ActiveGameObject(characterListPanel, true);
            Func_ActiveGameObject(characterSkillViewPanel, false);
        }

        public void OpenCharacterSkillViewPanel()
        {
            Func_ActiveGameObject(characterSkillViewPanel, true);
            Func_ActiveGameObject(characterListPanel, false);
        }

        public void CloseCharacterSelectPanel()
        {
            Func_ActiveGameObject(characterSelectPanel, false);
        }


        private static void Func_ActiveGameObject(GameObject obj, bool IsActive = false)
        {
            obj.SetActive(IsActive);
        }
    }
}