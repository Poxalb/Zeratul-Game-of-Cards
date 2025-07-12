using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider GVSlider;

    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float masterVolume = PlayerPrefs.GetFloat("globalVolume", 1f);

        musicSlider.value = musicVolume;
        SFXSlider.value = sfxVolume;
        GVSlider.value = masterVolume;

        audioMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        audioMixer.SetFloat("master", Mathf.Log10(masterVolume) * 20);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value; 
        audioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value; 
        audioMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetGlobalVolume()
    {
        float volume = GVSlider.value; 
        audioMixer.SetFloat("master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("globalVolume", volume);
    }
}
