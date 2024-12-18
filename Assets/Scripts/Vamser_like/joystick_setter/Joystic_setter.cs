using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Joystic_setter : MonoBehaviour
{
    [Header("<color=green>조이스틱 데이터")] [SerializeField]
    private SettingsData_oBJ settingsData; // ScriptableObject 참조

    [Header("<color=green>조이스틱 설정UI")] [SerializeField]
    private Slider joystickSizeSlider;

    [SerializeField] private TMP_Dropdown joystickTypeDropdown;
    [SerializeField] private Transform joystickTransform;
    [SerializeField] JoyStick_Pos_dragandDrop joyStickPosDragandDrop;
    [SerializeField] private Button saveandExitBtn;

    private void OnEnable()
    {
        joystickSizeSlider.onValueChanged.AddListener(delegate { JoystickSizeSliderChanged(); });
        joystickTypeDropdown.onValueChanged.AddListener(delegate { JoystickTypeDropdownChanged(); });
        saveandExitBtn.onClick.AddListener(SaveAndExit);
        LoadSettings();
    }


    private void JoystickSizeSliderChanged()
    {
        settingsData.joystickSize = joystickSizeSlider.value;
        joystickTransform.localScale = new Vector3(settingsData.joystickSize, settingsData.joystickSize, 1);
    }

    private void JoystickTypeDropdownChanged()
    {
        settingsData.joystickType = joystickTypeDropdown.value;
    }

    private void LoadSettings()
    {
        joystickSizeSlider.value = settingsData.joystickSize;
        joystickTypeDropdown.value = settingsData.joystickType;
        joystickTransform.localScale = new Vector3(settingsData.joystickSize, settingsData.joystickSize, 1);
        joystickTransform.position = settingsData.joystickPos;
    }


    private void SetJoystickPos()
    {
        settingsData.joystickPos = joystickTransform.position;
    }


    private void SaveAndExit()
    {
        settingsData.joystickType = joystickTypeDropdown.value;
        settingsData.joystickPos = joystickTransform.position;
        settingsData.joystickSize = joystickSizeSlider.value;

        settingsData.SaveSettings();
        SetJoystickPos();
        Destroy(gameObject);
    }
}