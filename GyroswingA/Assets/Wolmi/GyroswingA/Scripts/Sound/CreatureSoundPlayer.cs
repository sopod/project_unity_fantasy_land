using UnityEngine;
using UnityEngine.Audio;

public enum CreatureEffectSoundType
{
    Hit,
    Dead,
    Dash,
    Fire,
    Jump,
    ItemGet,
    Max
}

public class CreatureSoundPlayer : MonoBehaviour
{
    const float TRIM_RATIO = 0.3f;

    SoundLoader soundFiles;

    [SerializeField] AudioMixerGroup effectSoundMixerGroup;

    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioSource effectSoundAudio;

    void Awake()
    {
        soundFiles = UISoundPlayer.Instance.soundFiles;
        SetGroup();
    }

    void SetGroup()
    {
        effectSoundAudio.outputAudioMixerGroup = effectSoundMixerGroup;
    }

    public void PlaySound(CreatureEffectSoundType soundType, bool trim)
    {
        if ((int) soundType >= effectSounds.Length) return;

        AudioClip clip = soundFiles.GetClip(effectSounds[(int)soundType].name);

        if (clip == null) return;

        if (trim) effectSoundAudio.time = TRIM_RATIO;

        effectSoundAudio.PlayOneShot(clip);
    }
}
