using TMPro;
using UnityEngine;

namespace DogGuns_Games.vamsir
{


    public class VamserLike_UI : MonoBehaviour
    {
        [SerializeField] private TMP_Text MobWaveText;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text MobCountText;
        [SerializeField] private TMP_Text playerLevelText;

        VamserLike_GameManager gameManager;

        
        private void Awake()
        {
            gameManager = FindFirstObjectByType<VamserLike_GameManager>();
        }
        
        
    }
}