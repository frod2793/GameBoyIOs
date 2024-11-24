using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Sound
{
    Bgm,
 Effect,
 MaxCount,
}


public class Soundmanager : MonoBehaviour
{
    AudioSource[] audioSources = new AudioSource[(int)Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private float effectsoundVolum;
    private float bgmSoundVolum;
    // Start is called before the first frame update
    
    public void Init()
    {
        GameObject root = new GameObject { name = "Sound" };
        Object.DontDestroyOnLoad(root);

        string[] SoundNames = System.Enum.GetNames(typeof(Sound));
        for(int i=0;i<SoundNames.Length -1;i++)
        {
            GameObject go = new GameObject { name = SoundNames[i] };
            audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }
        audioSources[(int)Sound.Bgm].loop = true;
    }



   public void Clear()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(AudioClip audioClip,Sound type = Sound.Effect,float Pitch = 1.0f)
    {
        if (audioClip == null)
        {
            return;
        }

        if (type == Sound.Bgm)
        {
            AudioSource audioSource = audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.pitch = Pitch;
            audioSource.volume = bgmSoundVolum;
            audioSource.clip = audioClip;
          
            audioSource.Play();
        }
        else//effect 효과음 재
        {
            AudioSource audioSource = audioSources[(int)Sound.Effect];
            audioSource.pitch = Pitch;
            audioSource.volume = effectsoundVolum;
            audioSource.PlayOneShot(audioClip);
        }

    }

   

    public void Play(string path , Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void VolumSet(Sound type = Sound.Effect,float volum = 1.0f)
    {
        if (type == Sound.Bgm)
        {
            bgmSoundVolum = volum;
        }
        else if (type == Sound.Effect)
        {
            effectsoundVolum = volum;
        }
    }

    AudioClip GetOrAddAudioClip(string path,Sound typer = Sound.Effect)
    {
        if (path.Contains("Sound/")==false)
        {
            path = $"Sound/{path}";
        }

        AudioClip audioClip = null;
        if (typer == Sound.Bgm)
        {
            Manager.Resource.Load<AudioClip>(path);

        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Manager.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }
        if (audioClip == null)
        {
            Debug.Log($"AudioClip missing !{path}");

        }
        return audioClip;
    }
}
