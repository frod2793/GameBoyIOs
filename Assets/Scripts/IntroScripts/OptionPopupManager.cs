using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopupManager : MonoBehaviour
{

    [SerializeField]
    private Slider effectSoundVolum;
    [SerializeField]
    private Slider bGMsoundVolum;
    [SerializeField]
    private Button ExitBtn;
    Soundmanager Soundmanager;

    private void Start()
    {
        effectSoundVolum.onValueChanged.AddListener(delegate {
            EffectsoundVolumSlider();
        });

        bGMsoundVolum.onValueChanged.AddListener(delegate
        {
            BgmSoundVolumSlider();
        });

        ExitBtn.onClick.AddListener(SaveAndExite);
    }
    private void OnEnable()
    {
        if (Soundmanager == null)
        {
        Soundmanager = FindObjectOfType<Soundmanager>();
        }

        effectSoundVolum.value = PlayerPrefs.GetFloat("effectVolum");
        bGMsoundVolum.value = PlayerPrefs.GetFloat("bgmVolum");



    }

    public void EffectsoundVolumSlider()
    {
        if (Soundmanager == null)
        {
            return;
        }
        Soundmanager.VolumSet(Sound.Effect,effectSoundVolum.value);
    }

    public void BgmSoundVolumSlider()
    {
        if (Soundmanager==null)
        {
            return;
        }
        Soundmanager.VolumSet(Sound.Bgm, bGMsoundVolum.value);

    }
    public void SaveAndExite()
    {
        PlayerPrefs.SetFloat("effectVolum", effectSoundVolum.value);
        PlayerPrefs.SetFloat("bgmVolum", bGMsoundVolum.value);
        Destroy(gameObject);
      
    }
    

}
