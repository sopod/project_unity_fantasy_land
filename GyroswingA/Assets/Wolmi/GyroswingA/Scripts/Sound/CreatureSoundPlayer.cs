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

        if (trim) effectSoundAudio.time = trimRatio;
        effectSoundAudio.PlayOneShot(clip);
    }
}
