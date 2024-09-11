using System.IO;
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

    [SerializeField] private Button jumpBtn;
    public float Btnsize;
    
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

        LoadSettings(); // 시작 시 설정 불러오기
    }

    private void OnEnable()
    {
        if (Soundmanager == null)
        {
            Soundmanager = FindObjectOfType<Soundmanager>();
        }
        LoadSettings(); // 활성화 시 설정 불러오기
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
        settingsData.SaveSettings();
        // ScriptableObject는 자동으로 저장되므로, 별도로 저장할 필요 없음
        Destroy(gameObject); // 창 종료
    }

    private void SetControllBtnSize(float size)
    {
        jumpBtn.transform.localScale = new Vector3(size, size, 1);
        Btnsize = size;
    }
    
    public void LoadSettings()
    {
        // ScriptableObject에서 설정 불러오기
        effectSoundVolum.value = settingsData.effectSoundVolume;
        bGMsoundVolum.value = settingsData.backgroundSoundVolume;
        ControllBtnSize_SmallToggle.isOn = settingsData.ConSmall_toggle;
        ControllBtnSize_NormalToggle.isOn = settingsData.ConNormal_Toggle;
        ControllBtnSize_BigToggle.isOn = settingsData.ConBigBtn_Toggle;
    }
}
