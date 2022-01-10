using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum UISoundType
{
    Max
}


public class UISoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioClip[] bgms;

    [SerializeField] AudioSource effectSoundAudio;
    [SerializeField] AudioSource bgmAudio;

    public void PlayUISound(UISoundType soundType)
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
