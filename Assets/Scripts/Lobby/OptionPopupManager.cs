using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopupManager : MonoBehaviour
{
    public SettingsData_oBJ settingsData; // ScriptableObject 참조

    [Header("사운드 조절")] [SerializeField] private Slider effectSoundVolum;
    [SerializeField] private Slider bgMsoundVolum;
    [SerializeField] private Button exitBtn;
    Soundmanager _soundmanager;

    [Header("게임 컨트롤 버튼 조절")] [SerializeField]
    private Toggle controllBtnSizeSmallToggle;

    [SerializeField] private Toggle controllBtnSizeNormalToggle;
    [SerializeField] private Toggle controllBtnSizeBigToggle;

    [Header("조이스틱 사이즈및 타입 조절")] public VariableJoystick variableJoystick;
    [SerializeField] private Slider joystickSizeSlider;
    [SerializeField] private TMP_Dropdown joystickTypeDropdown;
    [SerializeField] private Button joyStickPossettingBtn;
    [SerializeField] private Button jumpBtn;

    private void Start()
    {
        settingsData.LoadSettings();
        // 슬라이더와 토글에 대한 리스너 등록
        effectSoundVolum.onValueChanged.AddListener(delegate { EffectsoundVolumSlider(); });
        bgMsoundVolum.onValueChanged.AddListener(delegate { BgmSoundVolumSlider(); });

        exitBtn.onClick.AddListener(SaveAndExit);

        controllBtnSizeSmallToggle.onValueChanged.AddListener(delegate { SetControllBtnSize(0.5f); });
        controllBtnSizeNormalToggle.onValueChanged.AddListener(delegate { SetControllBtnSize(1f); });
        controllBtnSizeBigToggle.onValueChanged.AddListener(delegate { SetControllBtnSize(1.5f); });


        joystickSizeSlider.onValueChanged.AddListener(delegate { SetJoystickSize(); });

        DropDown_Init();

        LoadSettings(); // 시작 시 설정 불러오기
    }

    private void OnEnable()
    {
        if (_soundmanager == null)
        {
            _soundmanager = FindAnyObjectByType<Soundmanager>();
        }

        LoadSettings(); // 활성화 시 설정 불러오기
    }

    private void SetJoystickSize()
    {
        variableJoystick.gameObject.transform.localScale =
            new Vector3(joystickSizeSlider.value, joystickSizeSlider.value, 1);
    }

    private void EffectsoundVolumSlider()
    {
        if (_soundmanager != null)
        {
            _soundmanager.VolumSet(Sound.Effect, effectSoundVolum.value);
        }
    }

    private void BgmSoundVolumSlider()
    {
        if (_soundmanager != null)
        {
            _soundmanager.VolumSet(Sound.Bgm, bgMsoundVolum.value);
        }
    }

    private void SaveAndExit()
    {
        // ScriptableObject에 직접 저장
        settingsData.effectSoundVolume = effectSoundVolum.value;
        settingsData.backgroundSoundVolume = bgMsoundVolum.value;
        settingsData.conSmallToggle = controllBtnSizeSmallToggle.isOn;
        settingsData.conNormalToggle = controllBtnSizeNormalToggle.isOn;
        settingsData.conBigBtnToggle = controllBtnSizeBigToggle.isOn;
        settingsData.joystickType = joystickTypeDropdown.value;
        settingsData.joystickSize = joystickSizeSlider.value;
        settingsData.joystickPos = variableJoystick.gameObject.transform.position;


        settingsData.SaveSettings();
        // ScriptableObject는 자동으로 저장되므로, 별도로 저장할 필요 없음
        Destroy(gameObject); // 창 종료
    }

    private void SetControllBtnSize(float size)
    {
        jumpBtn.transform.localScale = new Vector3(size, size, 1);
    }

    private void LoadSettings()
    {
        // ScriptableObject에서 설정 불러오기
        effectSoundVolum.value = settingsData.effectSoundVolume;
        bgMsoundVolum.value = settingsData.backgroundSoundVolume;
        controllBtnSizeSmallToggle.isOn = settingsData.conSmallToggle;
        controllBtnSizeNormalToggle.isOn = settingsData.conNormalToggle;
        controllBtnSizeBigToggle.isOn = settingsData.conBigBtnToggle;
        joystickTypeDropdown.value = settingsData.joystickType;
    }

    private void DropDown_Init()
    {
        // joystickTypeDropdown 옵션 생성
        joystickTypeDropdown.ClearOptions();
        List<string> options = new List<string>();
        options.Add("Fixed");
        options.Add("Floating");
        options.Add("Dynamic");
        joystickTypeDropdown.AddOptions(options);

        joystickTypeDropdown.onValueChanged.AddListener(delegate { ModeChanged(joystickTypeDropdown.value); });
    }

    private void ModeChanged(int index)
    {
        switch (index)
        {
            case 0:
                variableJoystick.SetMode(JoystickType.Fixed);
                break;
            case 1:
                variableJoystick.SetMode(JoystickType.Floating);
                break;
            case 2:
                variableJoystick.SetMode(JoystickType.Dynamic);
                break;
            default:
                break;
        }
    }
}