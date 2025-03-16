using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DogGuns_Games.Lobby
{


    public class Quest_Index : MonoBehaviour
    {
       [SerializeField] private Toggle questToggle;
       [SerializeField] private TMP_Text questName;
       
         public void SetQuestIndex(string name)
         {
              if (questName != null)
              {
                questName.text = name;
              }
         }
    }
}