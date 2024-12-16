using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DogGuns_Games.vamsir
{
    public class VamserLike_UI : MonoBehaviour
    {
        [SerializeField] private TMP_Text MobWaveText;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text MobCountText;
        [SerializeField] private TMP_Text playerLevelText;

        [SerializeField] private Button SettingBtn;
     
        VamserLike_GameManager gameManager;

        [Header("Puse 팝업")] public Image PusePopup;
        public Button PopUp_playBtn;
        public Button PopUp_ResetBtn;
        public Button PopUp_OPtionBtn;
    
       

        private void Awake()
        {
            gameManager = FindFirstObjectByType<VamserLike_GameManager>();
        }

        private void Start()
        {
            BtnSetting();
        }

        private void BtnSetting()
        {
            SettingBtn.onClick.AddListener(gameManager.Open_OptionPopUp);
        }
        
        
    }
}