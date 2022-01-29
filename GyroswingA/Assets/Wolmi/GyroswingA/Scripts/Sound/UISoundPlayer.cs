using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum UIEffectSoundType
{
    BtnClick,
    BtnBack,
    Max
}

public class UISoundPlayer : MonoBehaviour
{
    [SerializeField] AudioMixerGroup bgmMixerGroup;
    [SerializeField] AudioMixerGroup effectSoundMixerGroup;

    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioClip[] bgms;

    [SerializeField] AudioSource[] effectSoundAudios;
    [SerializeField] AudioSource bgmAudio;

    SoundLoader sound;
    

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
        DontDestroyOnLoad(this.gameObject);
        
        sound = FindObjectOfType<SoundLoader>();

        SetGroup();
    }

    void SetGroup()
    {
        for (int i = 0; i < effectSoundAudios.Length; i++)
        {
            effectSoundAudios[i].outputAudioMixerGroup = effectSoundMixerGroup;
        }

        bgmAudio.outputAudioMixerGroup = bgmMixerGroup;
    }


    public void PlayUISound(UIEffectSoundType soundType)
    {
        if ((int)soundType >= effectSounds.Length) return;

        AudioClip clip = sound.GetClip(effectSounds[(int) soundType].name);

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

    public void PlayBGM(SceneState state, bool isLoop = true)
    {
        int idx = (state == SceneState.Lobby) ? 0 : 1;

        if (idx >= effectSounds.Length) return;

        if (bgmAudio.isPlaying)
            bgmAudio.Stop();

        AudioClip clip = sound.GetClip(bgms[idx].name);

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
            clip = sound.GetClip(bgms[2].name);
        else
            clip = sound.GetClip(bgms[3].name);

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
