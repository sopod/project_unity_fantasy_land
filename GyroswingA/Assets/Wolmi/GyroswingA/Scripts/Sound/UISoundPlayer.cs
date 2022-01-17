using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIEffectSoundType
{
    BtnClick,
    BtnBack,
    CreatureDash, 
    CreatureFire,
    CreatureJump,
    CreatureDead,
    ItemGet,
    Max
}


public class UISoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioClip[] bgms;

    [SerializeField] AudioSource[] effectSoundAudios;
    [SerializeField] AudioSource bgmAudio;

    public bool IsBGMPlaying
    {
        get { return bgmAudio.isPlaying; }
    }

    static UISoundPlayer instance;

    public static UISoundPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UISoundPlayer>();
            }

            return instance;
        }
    }

    void Awake()
    {
        var objs = FindObjectsOfType<UISoundPlayer>();

        if (objs.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            foreach (var obj in objs)
            {
                if (SceneManager.GetActiveScene().name == "InGame" &&
                    obj.gameObject != this.gameObject) // destroy previous object
                    Destroy(obj.gameObject);
                else if (SceneManager.GetActiveScene().name != "InGame" &&
                         obj.gameObject == this.gameObject) // destroy new object
                    Destroy(obj.gameObject);
            }

            return;
        }

        if (instance == null) instance = this;
    }



    public void PlayUISound(UIEffectSoundType soundType)
    {
        if ((int)soundType >= effectSounds.Length) return;

        AudioClip clip = SoundManager.Instance.GetClip(effectSounds[(int) soundType].name);

        if (clip == null) return;
        
        for (int i = 0; i < effectSoundAudios.Length; i++)
        {
            if (!effectSoundAudios[i].isPlaying)
            {
                effectSoundAudios[i].PlayOneShot(clip);
                break;
            }
        }

    }

    public void PlayBGM(GameState gameState, bool isLoop = true)
    {
        if ((int)gameState >= effectSounds.Length) return;

        if (bgmAudio.isPlaying)
            bgmAudio.Stop();

        AudioClip clip = SoundManager.Instance.GetClip(bgms[(int) gameState].name);

        if (clip == null) return;

        bgmAudio.loop = isLoop;
        bgmAudio.clip = clip;
        bgmAudio.Play();
    }

    public void PlayResultBGM(bool isWin)
    {
        if (bgmAudio.isPlaying)
            bgmAudio.Stop();

        AudioClip clip;
        if (isWin)
            clip = SoundManager.Instance.GetClip(bgms[2].name);
        else
            clip = SoundManager.Instance.GetClip(bgms[3].name);

        if (clip == null) return;

        bgmAudio.loop = false;
        bgmAudio.clip = clip;
        bgmAudio.Play();
    }

    public void StopPlayingBGM()
    {
        bgmAudio.Stop();
    }

}
