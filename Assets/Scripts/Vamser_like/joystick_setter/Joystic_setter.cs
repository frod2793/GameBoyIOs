using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Joystic_setter : MonoBehaviour
{
    public SettingsData_oBJ settingsData; // ScriptableObject 참조
    [SerializeField] private Slider JoystickSizeSlider;
    [SerializeField] private TMP_Dropdown JoystickTypeDropdown;
    [SerializeField] private Transform joystickTransform;
    [SerializeField]  JoyStick_Pos_dragandDrop _joyStickPosDragandDrop;
[SerializeField] private Button saveandExitBtn;
    private void OnEnable()
    {
        JoystickSizeSlider.onValueChanged.AddListener(delegate { JoystickSizeSliderChanged(); });
        JoystickTypeDropdown.onValueChanged.AddListener(delegate { JoystickTypeDropdownChanged(); });
        saveandExitBtn.onClick.AddListener(SaveAndExit);
        LoadSettings();
    }
    
    
    
    private void JoystickSizeSliderChanged()
    {
        settingsData.joystickSize = JoystickSizeSlider.value;
        joystickTransform.localScale = new Vector3(settingsData.joystickSize, settingsData.joystickSize, 1);
    }
    
    private void JoystickTypeDropdownChanged()
    {
        settingsData.joystickType = JoystickTypeDropdown.value;
    }
    
    private void LoadSettings()
    {
        JoystickSizeSlider.value = settingsData.joystickSize;
        JoystickTypeDropdown.value = settingsData.joystickType;
    }
    
    private void OnDisable()
    {
        settingsData.SaveSettings();
        SetJoystickPos();
    }
    
    private void SetJoystickPos()
    {
        settingsData.joystickPos = joystickTransform.position;
    }
    
    
    private void SaveAndExit()
    {
        settingsData.SaveSettings();
        SetJoystickPos();
         Destroy(gameObject);
    }
    
    
    
    
    
}
