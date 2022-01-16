using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EffectSoundType
{
    Max
}


public class UISoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioClip[] bgms;

    [SerializeField] AudioSource effectSoundAudio;
    [SerializeField] AudioSource bgmAudio;

    public bool IsBGMPlaying { get { return bgmAudio.isPlaying; } }

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
                if (SceneManager.GetActiveScene().name == "InGame" && obj.gameObject != this.gameObject) // destroy previous object
                    Destroy(obj.gameObject);
                else if (SceneManager.GetActiveScene().name != "InGame" && obj.gameObject == this.gameObject) // destroy new object
                    Destroy(obj.gameObject);
            }
            return;
        }

        if (instance == null) instance = this;
    }



    public void PlayUISound(EffectSoundType soundType)
    {
        AudioClip clip = SoundManager.Instance.GetClip(effectSounds[(int)soundType].name);

        if (clip == null) return;

        if (!effectSoundAudio.isPlaying)
            effectSoundAudio.PlayOneShot(clip);

        //for (int i =0; i < audios.Length; i++)
        //{
        //    if (!audios[i].isPlaying)
        //    {
        //        audios[i].PlayOneShot(clip);
        //        break;
        //    }
        //}

    }

    public void PlayBGM(GameState gameState, bool isLoop = true)
    {
        if (bgmAudio.isPlaying)
            bgmAudio.Stop();
        
        AudioClip clip = SoundManager.Instance.GetClip(bgms[(int)gameState].name);

        if (clip == null) return;

        bgmAudio.loop = isLoop;
        bgmAudio.clip = clip;
        bgmAudio.Play();
    }
}
