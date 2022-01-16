using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("bgmVolume"))
        {
            PlayerPrefs.SetFloat("bgmVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = bgmSlider.value;
        Save();
    }

    void Load()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume");
    }

    void Save()
    {
        PlayerPrefs.SetFloat("bgmVolume", bgmSlider.value);
    }
}
