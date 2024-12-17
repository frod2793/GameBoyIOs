using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DogGuns_Games.vamsir
{
    public class VamserLike_UI : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text mobWaveText;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text mobCountText;
        [SerializeField] private TMP_Text playerLevelText;

        [FormerlySerializedAs("SettingBtn")]
        [SerializeField] private Button settingBtn;
        
        [Header("<color=green>조이스틱</color>")]
        [SerializeField] private VariableJoystick variableJoystick;
 
        
        VamserLike_GameManager _gameManager;
        
        private void Awake()
        {
            _gameManager = FindFirstObjectByType<VamserLike_GameManager>();
        }

        private void Start()
        {
            BtnSetting();
            JoystickSetting();
        }

        private void JoystickSetting()
        {
            if (variableJoystick == null)
            {
                variableJoystick = FindFirstObjectByType<VariableJoystick>();
            }
                
            variableJoystick.gameObject.transform.localScale = new Vector3(_gameManager.settingsData.joystickSize, _gameManager.settingsData.joystickSize, 1);  
            variableJoystick.SetMode((JoystickType)_gameManager.settingsData.joystickType);
        }
        private void BtnSetting()
        {
            settingBtn.onClick.AddListener(_gameManager.Open_OptionPopUp);
        }
        
        
    }
}