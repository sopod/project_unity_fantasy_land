using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CreatureEffectSoundType
{
    Dash,
    Fire,
    Jump,
    Dead,
    ItemGet,
    HittingSound,
    Max
}

public class CreatureSoundPlayer : MonoBehaviour
{
    const float trimRatio = 0.3f;

    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioSource[] effectSoundAudios;
    

    public void PlaySound(CreatureEffectSoundType soundType, bool trim)
    {
        if ((int) soundType >= effectSounds.Length) return;

        AudioClip clip = SoundManager.Instance.GetClip(effectSounds[(int)soundType].name);

        if (clip == null) return;


        for (int i = 0; i < effectSoundAudios.Length; i++)
        {
            if (!effectSoundAudios[i].isPlaying)
            {
                if (trim) effectSoundAudios[i].time = trimRatio;

                effectSoundAudios[i].PlayOneShot(clip);
                return;
            }
        }

    }
}
