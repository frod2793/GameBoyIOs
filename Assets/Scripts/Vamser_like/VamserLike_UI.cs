using System;
using Cysharp.Threading.Tasks;
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

        [FormerlySerializedAs("SettingBtn")] [SerializeField]
        private Button settingBtn;

        [Header("<color=green>조이스틱</color>")] [SerializeField]
        private VariableJoystick variableJoystick;

        [SerializeField] private Transform joystickTransform;

        VamserLike_GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindFirstObjectByType<VamserLike_GameManager>();
        }

        private void Start()
        {
            Play_State.OnGameStart += GameStart;
            Play_State.OnGamePause += Pause;
            Play_State.OnGameResume += Resume;
        }

        private void GameStart()
        {
            BtnSetting();
            JoystickSetting();
            UpdateUI().Forget();
        }

        private void Pause()
        {
            joystickTransform.gameObject.SetActive(false);
        }

        private void Resume()
        {
            joystickTransform.gameObject.SetActive(true);
            JoystickSetting();

            settingBtn.enabled = true;
        }


        private void JoystickSetting()
        {
            if (variableJoystick == null)
            {
                variableJoystick = FindFirstObjectByType<VariableJoystick>();
            }

            Vector2 originJoystickPos = joystickTransform.position;
            joystickTransform.localScale = new Vector3(_gameManager.settingsData.joystickSize,
                _gameManager.settingsData.joystickSize, 1);
            variableJoystick.SetMode((JoystickType)_gameManager.settingsData.joystickType);
            //todo : 조이스틱 위치 조정 필요 
            joystickTransform.position = new Vector3(_gameManager.settingsData.joystickPos.x,
                _gameManager.settingsData.joystickPos.y, 0);
        }

        private void BtnSetting()
        {
            settingBtn.onClick.AddListener(() =>
            {
                _gameManager.Open_OptionPopUp();
                settingBtn.enabled = false;
            });
        }
        
        private async UniTask UpdateUI()
        {
            while (true)
            {
                mobWaveText.text = $"Wave {_gameManager.MobSpawnWave()}";
                coinText.text = $"{_gameManager.CoinCount()}";
                mobCountText.text = $"{_gameManager.Mobcount()}";
                playerLevelText.text = $"Lv. {_gameManager.PlayerLevel()}";
                await UniTask.Yield();
            }
        }
    }
}