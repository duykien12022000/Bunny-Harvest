using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : Singleton<AudioManager>
{
    public static int MusicSetting
    {
        get { return PlayerPrefs.GetInt("musicEnabled", 1); }
        private set { PlayerPrefs.SetInt("musicEnabled", value); }
    }
    public static int SoundSetting
    {
        get { return PlayerPrefs.GetInt("soundEnabled", 1); }
        private set { PlayerPrefs.SetInt("soundEnabled", value); }
    }
    public static int VibraSetting
    {
        get { return PlayerPrefs.GetInt("vibraEnabled", 1); }
        private set { PlayerPrefs.SetInt("vibraEnabled", value); }
    }
    public static float soundVolume
    {
        get { return PlayerPrefs.GetFloat("sound_vol", 1); }
        set { PlayerPrefs.SetFloat("sound_vol", value); }
    }
    public static float musicVolume
    {
        get { return PlayerPrefs.GetFloat("music_vol", 1); }
        set
        {
            if (value <= 0)
            {
                Instance.EnableMusic(false);
            }
            if (musicVolume <= 0 && value > 0)
            {
                Instance.EnableMusic(true);
            }
            PlayerPrefs.SetFloat("music_vol", value);
        }
    }
    [SerializeField] AudioContainerSO soundContainer;
    [SerializeField] AudioContainerSO musicContainer;
    [SerializeField] AudioSource soundPlayer;
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource soundLoop;
    [field: SerializeField] private List<AudioSource> activeAudioSources = new List<AudioSource>();
    [field: SerializeField] private List<AudioSource> inActiveAudioSources = new List<AudioSource>();
    protected override void Awake()
    {
        for (int i = 0; i < 2; i++)
        {
            AudioSource audioSource = Instantiate(musicPlayer, transform);
            inActiveAudioSources.Add(audioSource);
        }
        //if (_instance == null)
        //{
        //    _instance = this as AudioManager;
        //    DontDestroyOnLoad(gameObject); // nếu muốn giữ lại giữa các scene
        //}
        //else if (_instance != this)
        //{
        //    Destroy(gameObject); // hủy bản mới sinh ra
        //}
    }
    public void PlayOneShot(AudioClip audioClip, float volume, float pitch = 1, float delay = 0)
    {
        if (SoundSetting != 1) return;
        if (audioClip == null) return;
        StartCoroutine(IEDeplayPlayOneShot(audioClip, volume, pitch, delay));
    }
    public void PlayOneShot(string clipName, float volume, float pitch = 1, float delay = 0)
    {
        if (SoundSetting != 1) return;
        AudioClip clip = soundContainer.GetClip(clipName);
        PlayOneShot(clip, volume, pitch, delay);
    }
    IEnumerator IEDeplayPlayOneShot(AudioClip audioClip, float volume, float pitch, float delay = 0)
    {
        float timer = delay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        float newVolume = volume;
        soundPlayer.pitch = pitch;
        soundPlayer.PlayOneShot(audioClip, newVolume);
    }

    private AudioClip currentLoop;

    public void PlayLoop(AudioClip audioClip, float volume, float delay = 0)
    {
        if (SoundSetting != 1) return;
        if (audioClip == null) return;
        if (currentLoop != null)
        {
            StopLoop();
        }
        currentLoop = audioClip;
        StartCoroutine(IEDelayPlayLoop(currentLoop, volume, delay));
    }
    public void PlayLoop(string clipName, float volume, float delay = 0)
    {
        if (SoundSetting != 1) return;
        AudioClip clip = soundContainer.GetClip(clipName);
        PlayLoop(clip, volume, delay);
    }
    IEnumerator IEDelayPlayLoop(AudioClip audioClip, float volume, float delay = 0)
    {
        float timer = delay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        float newVolume = volume;
        soundLoop.clip = audioClip;
        soundLoop.volume = newVolume;
        soundLoop.Play();
    }
    public void StopLoop()
    {
        //if (soundLoopPlayer.clip.length > 0) return;
        soundLoop.Stop();
    }

    private string currentBM;
    public void StopMusic(string musicName = null, float currentVolume = 1)
    {
        for (int i = 0; i < activeAudioSources.Count; i++)
        {
            var source = activeAudioSources[i];
            if (source.clip.name == musicName)
            {
                FadeMusic(source, currentVolume, 0, 1.5f, () =>
                {
                    source.Stop();
                    inActiveAudioSources.Add(source);
                    activeAudioSources.Remove(source);
                });
            }
        }
    }
    public void ChangeVolumeMusic(float currentVolume = 1, float endValue = 0)
    {
        if (MusicSetting != 1)
        {
            currentVolume = 0;
        }
        for (int i = 0; i < activeAudioSources.Count; i++)
        {
            var source = activeAudioSources[i];
            if (source.clip.name == currentBM)
            {
                FadeMusic(source, currentVolume, endValue, 1.5f, () =>
                {
                    if (endValue != 0) return;
                    source.Stop();
                    inActiveAudioSources.Add(source);
                    activeAudioSources.Remove(source);
                });
            }
        }
    }
    public void PlayMusic(string clipName, float volume, bool isLoop)
    {
        if (MusicSetting != 1)
        {
            volume = 0;
        }
        if (currentBM != null)
        {
            StopMusic(currentBM, volume);
        }
        AudioClip clip = musicContainer.GetClip(clipName);
        if (clip == null) return;
        if (IsPlaying(clip.name) != null) return;
        AudioSource source = GetAudioSource();
        source.clip = clip;
        source.loop = isLoop;
        source.Play();
        currentBM = clipName;
        FadeMusic(source, 0, volume, 1.5f, () => { });
    }
    public void PlayMusic(AudioClip clip, float volume, bool isLoop)
    {
        if (MusicSetting != 1)
        {
            volume = 0;
        }
        if (clip == null) return;
        if (IsPlaying(clip.name) != null) return;
        AudioSource source = GetAudioSource();
        source.clip = clip;
        source.loop = isLoop;
        //source.volume = volume;
        source.Play();

    }
    private AudioSource IsPlaying(string clipName)
    {
        for (int i = 0; i < activeAudioSources.Count; i++)
        {
            if (activeAudioSources[i].clip.name == clipName)
            {
                return activeAudioSources[i];
            }
        }
        return null;
    }
    private AudioSource GetAudioSource()
    {
        AudioSource audioSource = null;
        if (inActiveAudioSources.Count > 0)
        {
            audioSource = inActiveAudioSources[0];
            inActiveAudioSources.RemoveAt(0);
        }
        else
        {
            audioSource = musicPlayer.gameObject.AddComponent<AudioSource>();
        }
        activeAudioSources.Add(audioSource);
        return audioSource;
    }

    public void StopSound()
    {
        soundPlayer.Stop();
    }

    public void StopMusic(string musicName)
    {
        var source = IsPlaying(musicName);
        if (source)
        {
            source.Stop();
            activeAudioSources.Remove(source);
            inActiveAudioSources.Add(source);
        }
    }
    public void StopAllMusic()
    {
        for (int i = 0; i < activeAudioSources.Count; i++)
        {
            activeAudioSources[i].Stop();
        }
        inActiveAudioSources.AddRange(activeAudioSources);
        activeAudioSources.Clear();
    }
    public void ResumeMusic()
    {
        musicPlayer.Play();
    }
    public void EnableMusic(bool status)
    {
        MusicSetting = status ? 1 : 0;
        if (MusicSetting != 1)
        {
            for (int i = 0; i < activeAudioSources.Count; i++)
            {
                activeAudioSources[i].volume = 0;
            }
        }
        else
        {
            for (int i = 0; i < activeAudioSources.Count; i++)
            {
                activeAudioSources[i].volume = 1;
            }
        }
    }
    public void EnableSound(bool status)
    {
        SoundSetting = status ? 1 : 0;
    }
    public void FadeMusic(AudioSource audio, float value, float endValue, float duration, System.Action onComplete)
    {
        if (MusicSetting != 1)
        {
            value = 0;
            endValue = 0;
        }
        Tween x = null;
        x = DOTween.To(() => value, x => value = x, endValue, duration).SetEase(Ease.InOutQuart).OnUpdate(() =>
        {
            audio.volume = value;
        }).OnComplete(() =>
        {
            x.Kill();
            onComplete?.Invoke();
        });
    }
    public void EnableVibra(bool status)
    {
        VibraSetting = status ? 1 : 0;
    }
    public float GetSFXLength(string clipName)
    {
        AudioClip clip = soundContainer.GetClip(clipName);
        if (clip == null) return 0;
        return clip.length;
    }

}
public static class SFXStr
{
    public static string CLICK = "click";
    public static string CLACK = "clack";
    public static string STEP = "step";
    public static string HOLD = "hold";
    public static string LOSE = "lose";
    public static string LOSE_VOICE = "aww_dissappointment";
    public static string TIME_UP = "time up";
    public static string WIN_LONG = "win 1";
    public static string WIN_SHORT = "win 2";
    public static string FIREWORK = "firework";
    public static string NEWTIPS = "new_tips";
    public static string NEWBOOSTER = "new_booster";
    public static string COIN = "coin";

}