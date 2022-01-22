using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SoundGroupType
{
    BGM, 
    EffectSound,
    Max
}


public class VolumeSlider : MonoBehaviour
{
    [SerializeField] SoundGroupType type;
    [SerializeField] AudioMixerGroup mixerGroup;
    string bgmVolumeName = "BGMVolume";
    string effectSoundVolume = "EffectSoundVolume";

    [SerializeField] Slider slider;

   
    public void InitSlider()
    {
        slider.minValue = 0.1f;

        if (type == SoundGroupType.BGM && !PlayerPrefs.HasKey(bgmVolumeName)) // if not exists, save and load
        {
            PlayerPrefs.SetFloat(bgmVolumeName, 1);
            Load();
        }
        else if (type == SoundGroupType.EffectSound && !PlayerPrefs.HasKey(effectSoundVolume))
        {
            PlayerPrefs.SetFloat(effectSoundVolume, 1);
            Load();
        }
        else // if exists, load
        {
            Load();
        }
    }

    public void OnVolumeChanged()
    {
        float volume = Mathf.Log10(slider.value) * 20;
        if (type == SoundGroupType.BGM)
        {
            mixerGroup.audioMixer.SetFloat(bgmVolumeName, volume);
        }
        else
        {
            mixerGroup.audioMixer.SetFloat(effectSoundVolume, volume);
        }
            
        Save();
    }

    void Load()
    {
        if (type == SoundGroupType.BGM)
        {
            slider.value = PlayerPrefs.GetFloat(bgmVolumeName);
        }
        else
        {
            slider.value = PlayerPrefs.GetFloat(effectSoundVolume);
        }
    }

    void Save()
    {
        if (type == SoundGroupType.BGM)
        {
            PlayerPrefs.SetFloat(bgmVolumeName, slider.value);
        }
        else
        {
            PlayerPrefs.SetFloat(effectSoundVolume, slider.value);
        }
    }
}
