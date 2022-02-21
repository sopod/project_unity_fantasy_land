using UnityEngine;
using UnityEngine.Audio;

public enum UIEffectSoundType
{
    BtnClick,
    BtnBack,
    KilledEnemy,
    Max
}

public enum BgmSoundType
{
    Lobby,
    InGame, 
    Win, 
    GameOver,
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
            if (instance == null) instance = FindObjectOfType<UISoundPlayer>();
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

    public void PlayBGM(BgmSoundType soundType, bool isLoop)
    {
        int idx = (int)soundType;
        if (idx >= bgms.Length) return;

        if (bgmAudio.isPlaying) bgmAudio.Stop();

        AudioClip clip = soundFiles.GetClip(bgms[idx].name);

        if (clip == null) return;

        bgmAudio.loop = isLoop;
        bgmAudio.clip = clip;
        bgmAudio.Play();
    }

    public void StopPlayingBGM()
    {
        bgmAudio.Stop();
    }

}
