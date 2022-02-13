using UnityEngine;
using UnityEngine.Audio;

public enum CreatureEffectSoundType
{
    Dash,
    Dead,
    Fire,
    Jump,
    ItemGet,
    Max
}

public class CreatureSoundPlayer : MonoBehaviour
{
    const float trimRatio = 0.3f;

    SoundLoader soundFiles;

    [SerializeField] AudioMixerGroup effectSoundMixerGroup;

    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioSource[] effectSoundAudios;

    void Awake()
    {
        soundFiles = UISoundPlayer.Instance.soundFiles;
        SetGroup();
    }

    void SetGroup()
    {
        for (int i = 0; i < effectSoundAudios.Length; i++)
        {
            effectSoundAudios[i].outputAudioMixerGroup = effectSoundMixerGroup;
        }
    }

    public void PlaySound(CreatureEffectSoundType soundType, bool trim)
    {
        if ((int) soundType >= effectSounds.Length) return;

        AudioClip clip = soundFiles.GetClip(effectSounds[(int)soundType].name);

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
