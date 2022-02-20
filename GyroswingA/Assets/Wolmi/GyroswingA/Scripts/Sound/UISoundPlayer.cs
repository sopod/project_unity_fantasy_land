using UnityEngine;
using UnityEngine.Audio;

public enum UIEffectSoundType
{
    BtnClick,
    BtnBack,
    KilledEnemy,
    Max
}

public class UISoundPlayer : MonoBehaviour
{
    public SoundLoader soundFiles;

    [SerializeField] AudioMixerGroup bgmMixerGroup;
    [SerializeField] AudioMixerGroup effectSoundMixerGroup;

    [SerializeField] AudioClip[] effectSounds;
    [SerializeField] AudioClip[] bgms;

    [SerializeField] AudioSource effectSoundAudio;
    [SerializeField] AudioSource bgmAudio;

    

    static UISoundPlayer instance;

    public static UISoundPlayer Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UISoundPlayer>();

            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        SetGroup();
    }

    void SetGroup()
    {
        effectSoundAudio.outputAudioMixerGroup = effectSoundMixerGroup;

        bgmAudio.outputAudioMixerGroup = bgmMixerGroup;
    }


    public void PlayUISound(UIEffectSoundType soundType)
    {
        if ((int)soundType >= effectSounds.Length) return;

        AudioClip clip = soundFiles.GetClip(effectSounds[(int) soundType].name);

        if (clip == null) return;

        effectSoundAudio.PlayOneShot(clip);

    }

    public void PlayBGM(SceneState state, bool isLoop = true)
    {
        int idx = (state == SceneState.Lobby) ? 0 : 1;

        if (idx >= effectSounds.Length) return;

        if (bgmAudio.isPlaying)
            bgmAudio.Stop();

        AudioClip clip = soundFiles.GetClip(bgms[idx].name);

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
            clip = soundFiles.GetClip(bgms[2].name);
        else
            clip = soundFiles.GetClip(bgms[3].name);

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
