using UnityEditor;
using UnityEngine;


namespace DogGuns_Games.vamsir
{
    [CustomEditor(typeof(VamserLike_GameManager))]
    public class VamSetLike_Test : Editor
    {

        [Header("<color=green> 인게임내에 캐릭터 및 무기 변경")]
        [SerializeField] private int Character_Index;
        [SerializeField] private int Weapon_Index;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            VamserLike_GameManager vamserLikeGameManager = (VamserLike_GameManager) target;

            Character_Index = EditorGUILayout.IntField("Character_Index", Character_Index);
            Weapon_Index = EditorGUILayout.IntField("Weapon_Index", Weapon_Index);

            if (GUILayout.Button("Change Character And Weapon"))
            {
                Player_Data_Manager_Dontdesytoy.Instance.SelectCharacterIndex = Character_Index;
                Player_Data_Manager_Dontdesytoy.Instance.SelectWeaponIndex = Weapon_Index;
                vamserLikeGameManager.ChangeCharacterAndWeaponandSpawn();
            }
        }
      
        
        
    }
}
