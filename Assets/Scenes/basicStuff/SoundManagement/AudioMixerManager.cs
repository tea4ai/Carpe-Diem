using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager instance;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVol", Mathf.Log10(volume) * 20f);
    }
    public void setSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVol", Mathf.Log10(volume) * 20f);
    }
    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log10(volume) * 20f);
    }
}
