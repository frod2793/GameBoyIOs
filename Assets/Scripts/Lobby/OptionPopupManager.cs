using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopupManager : MonoBehaviour
{
    public SettingsData_oBJ settingsData; // ScriptableObject 참조
    
    [Header("사운드 조절")] 
    [SerializeField] private Slider effectSoundVolum;
    [SerializeField] private Slider bGMsoundVolum;
    [SerializeField] private Button ExitBtn;
    Soundmanager Soundmanager;

    [Header("게임 컨트롤 버튼 조절")] 
    [SerializeField] private Toggle ControllBtnSize_SmallToggle;
    [SerializeField] private Toggle ControllBtnSize_NormalToggle;
    [SerializeField] private Toggle ControllBtnSize_BigToggle;

    [Header("조이스틱 사이즈및 타입 조절")]
    public VariableJoystick variableJoystick;
    [SerializeField] private Slider joystickSizeSlider;
    [SerializeField] private TMP_Dropdown joystickTypeDropdown;
    [SerializeField] private Button JoyStickPossettingBtn;
    [SerializeField] private Button jumpBtn;

    private void Start()
    {
        settingsData.LoadSettings();
        // 슬라이더와 토글에 대한 리스너 등록
        effectSoundVolum.onValueChanged.AddListener(delegate { EffectsoundVolumSlider(); });
        bGMsoundVolum.onValueChanged.AddListener(delegate { BgmSoundVolumSlider(); });

        ExitBtn.onClick.AddListener(SaveAndExit);
        
        ControllBtnSize_SmallToggle.onValueChanged.AddListener(delegate { SetControllBtnSize(0.5f); });
        ControllBtnSize_NormalToggle.onValueChanged.AddListener(delegate { SetControllBtnSize(1f); });
        ControllBtnSize_BigToggle.onValueChanged.AddListener(delegate { SetControllBtnSize(1.5f); });
        
        
        joystickSizeSlider.onValueChanged.AddListener(delegate {SetJoystickSize(); });
        
        DropDown_Init();
        
        LoadSettings(); // 시작 시 설정 불러오기
    }

    private void OnEnable()
    {
        if (Soundmanager == null)
        {
            Soundmanager = FindAnyObjectByType<Soundmanager>();
        }
        
        LoadSettings(); // 활성화 시 설정 불러오기
    }

    public void SetJoystickSize()
    {
        variableJoystick.gameObject.transform.localScale = new Vector3(joystickSizeSlider.value, joystickSizeSlider.value, 1);
    }
    
    public void EffectsoundVolumSlider()
    {
        if (Soundmanager != null)
        {
            Soundmanager.VolumSet(Sound.Effect, effectSoundVolum.value);
        }
    }

    public void BgmSoundVolumSlider()
    {
        if (Soundmanager != null)
        {
            Soundmanager.VolumSet(Sound.Bgm, bGMsoundVolum.value);
        }
    }

    public void SaveAndExit()
    {
        // ScriptableObject에 직접 저장
        settingsData.effectSoundVolume = effectSoundVolum.value;
        settingsData.backgroundSoundVolume = bGMsoundVolum.value;
        settingsData.ConSmall_toggle = ControllBtnSize_SmallToggle.isOn;
        settingsData.ConNormal_Toggle = ControllBtnSize_NormalToggle.isOn;
        settingsData.ConBigBtn_Toggle = ControllBtnSize_BigToggle.isOn;
        settingsData.JoystickType = joystickTypeDropdown.value;
        settingsData.JoystickSize = joystickSizeSlider.value;
        settingsData.JoystickPos = variableJoystick.gameObject.transform.position;
        
        
        settingsData.SaveSettings();
        // ScriptableObject는 자동으로 저장되므로, 별도로 저장할 필요 없음
        Destroy(gameObject); // 창 종료
    }

    private void SetControllBtnSize(float size)
    {
        jumpBtn.transform.localScale = new Vector3(size, size, 1);
    }
    
    public void LoadSettings()
    {
        // ScriptableObject에서 설정 불러오기
        effectSoundVolum.value = settingsData.effectSoundVolume;
        bGMsoundVolum.value = settingsData.backgroundSoundVolume;
        ControllBtnSize_SmallToggle.isOn = settingsData.ConSmall_toggle;
        ControllBtnSize_NormalToggle.isOn = settingsData.ConNormal_Toggle;
        ControllBtnSize_BigToggle.isOn = settingsData.ConBigBtn_Toggle;
        joystickTypeDropdown.value = settingsData.JoystickType;
        
    }

    public void DropDown_Init()
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
    
    public void ModeChanged(int index)
    {
        switch(index)
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
