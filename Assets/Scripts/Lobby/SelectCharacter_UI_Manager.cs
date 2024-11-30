using System.Drawing.Design;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectCharacter_UI_Manager : MonoBehaviour
{
  
    [Header("캐릭터 선택창 열기 버튼")]
    [SerializeField]private Button openCharacterSelectButton;
    [Header("캐릭터 선택창 닫기 버튼")]
    [SerializeField] private Button closeCharacterSelectButton;
    [Header("캐릭터 선택창")]
    [SerializeField] private GameObject characterSelectPanel;
    [Header("캐릭터 리스트 열기 버튼")]
    [SerializeField] private Button openCharacterListPanel;
    [Header("캐릭터 스킬뷰 열기 버튼")]
    [SerializeField] private Button openCharacterSkillViewPanel;
    [Header("캐릭터 스킬뷰")]
    [SerializeField] private GameObject characterSkillViewPanel;
    [Header("캐릭터 리스트")]
    [SerializeField] private GameObject characterListPanel;
    
    
    void Start()
    {
        openCharacterSelectButton.onClick.AddListener(() => Func_ActiveGameObject(characterSelectPanel,true));
        openCharacterListPanel.onClick.AddListener(() =>
        {
            Func_ActiveGameObject(characterListPanel, true);
            Func_ActiveGameObject(characterSkillViewPanel, false);
        });
        openCharacterSkillViewPanel.onClick.AddListener(() =>
        {
            Func_ActiveGameObject(characterSkillViewPanel, true);
            Func_ActiveGameObject(characterListPanel, false);
        });
        closeCharacterSelectButton.onClick.AddListener(() => Func_ActiveGameObject(characterSelectPanel));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private static void Func_ActiveGameObject(GameObject obj,bool IsActive = false)
    {
        obj.SetActive(IsActive);
    }

}
