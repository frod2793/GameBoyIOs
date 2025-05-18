using UnityEngine;

namespace DogGuns_Games.Lobby
{
    /// <summary>
    /// 캐릭터 선택 UI 관리 클래스
    /// </summary>
    public class CharacterSelectUIManager : MonoBehaviour
    {
        #region 필드 및 변수

        [Header("<color=green>캐릭터 선택창")] 
        [SerializeField] private GameObject characterSelectPanel;
        
        [Header("<color=green>캐릭터 스킬뷰")] 
        [SerializeField] private GameObject characterSkillViewPanel;
        
        [Header("<color=green>캐릭터 리스트")] 
        [SerializeField] private GameObject characterListPanel;

        [Header("<color=green>캐릭터선택 관련")] 
        [SerializeField] private CharactorSelect_Index characterSelectIndexPrefab;
        [SerializeField] private Transform characterSelectIndexParent;

        #endregion

        #region Unity 라이프사이클

        private void Awake()
        {
           // InitializePanels();
        }

        #endregion

        #region 초기화 메서드

        /// <summary>
        /// 패널 초기 상태 설정
        /// </summary>
        private void InitializePanels()
        {
            if (characterSelectPanel != null)
                characterSelectPanel.SetActive(false);
            
            if (characterSkillViewPanel != null)
                characterSkillViewPanel.SetActive(false);
            
            if (characterListPanel != null)
                characterListPanel.SetActive(false);
        }

        #endregion

        #region UI 패널 제어 - 공개 메서드

        /// <summary>
        /// 캐릭터 선택 패널 열기
        /// </summary>
        public void OpenCharacterSelectPanel()
        {
            SetGameObjectActive(characterSelectPanel, true);
            LobbyUIManager.AddClosePopUpAction(CloseCharacterSelectPanel);
        }

        /// <summary>
        /// 캐릭터 목록 패널 열기
        /// </summary>
        public void OpenCharacterListPanel()
        {
            SetGameObjectActive(characterListPanel, true);
            SetGameObjectActive(characterSkillViewPanel, false);
            LobbyUIManager.AddClosePopUpAction(CloseCharacterListPanel);
        }

        /// <summary>
        /// 캐릭터 스킬 보기 패널 열기
        /// </summary>
        public void OpenCharacterSkillViewPanel()
        {
            SetGameObjectActive(characterSkillViewPanel, true);
            SetGameObjectActive(characterListPanel, false);
            LobbyUIManager.AddClosePopUpAction(CloseCharacterSkillViewPanel);
        }

        /// <summary>
        /// 캐릭터 선택 패널 닫기
        /// </summary>
        public void CloseCharacterSelectPanel()
        {
            SetGameObjectActive(characterSelectPanel, false);
        }

        /// <summary>
        /// 캐릭터 목록 패널 닫기
        /// </summary>
        private void CloseCharacterListPanel()
        {
            SetGameObjectActive(characterListPanel, false);
        }
        
        /// <summary>
        /// 캐릭터 스킬 보기 패널 닫기
        /// </summary>
        private void CloseCharacterSkillViewPanel()
        {
            SetGameObjectActive(characterSkillViewPanel, false);
        }

        #endregion

        #region 유틸리티 메서드

        /// <summary>
        /// 게임 오브젝트 활성화/비활성화 처리
        /// </summary>
        /// <param name="obj">대상 게임 오브젝트</param>
        /// <param name="isActive">활성화 여부</param>
        private static void SetGameObjectActive(GameObject obj, bool isActive = false)
        {
            if (obj != null)
                obj.SetActive(isActive);
            else
                Debug.LogWarning("활성화하려는 게임 오브젝트가 null입니다.");
        }

        #endregion
    }
}