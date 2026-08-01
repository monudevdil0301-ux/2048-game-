using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip swipeClip;
    public AudioClip mergeClip;
    public AudioClip popClip;
    public AudioClip gameOverClip;
    public AudioClip buttonClip;
    public AudioClip victoryClip;

    bool musicEnabled;
    bool sfxEnabled;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Keys now match what MenuManager saves ("Music" and "Sound")
        musicEnabled = PlayerPrefs.GetInt("Music", 1) == 1;
        sfxEnabled   = PlayerPrefs.GetInt("Sound", 1) == 1;

        ApplyMusic();
        ApplySFX();
    }

    // Called by MenuManager.ToggleMusic()
    public void SetMusicEnabled(bool enabled)
    {
        musicEnabled = enabled;
        ApplyMusic();
    }

    // Called by MenuManager.ToggleSound()
    public void SetSoundEnabled(bool enabled)
    {
        sfxEnabled = enabled;
        ApplySFX();
    }

    void ApplyMusic()
    {
        if (musicSource == null) return;
        musicSource.mute = !musicEnabled;
        if (!musicSource.isPlaying && musicSource.clip != null)
            musicSource.Play();
    }

    void ApplySFX()
    {
        if (sfxSource == null) return;
        sfxSource.mute = !sfxEnabled;
    }

    public bool IsMusicEnabled() => musicEnabled;
    public bool IsSFXEnabled()   => sfxEnabled;

   public void PlayMerge()
{
    if (!sfxEnabled || sfxSource == null || mergeClip == null)
        return;

    sfxSource.PlayOneShot(mergeClip);
}
    public void PlayPop()
    {
        if (!sfxEnabled || sfxSource == null || popClip == null) return;
        sfxSource.PlayOneShot(popClip);
    }

    public void PlaySwipe()
    {
        if (!sfxEnabled || sfxSource == null || swipeClip == null) return;
        sfxSource.PlayOneShot(swipeClip, 1f);
    }

    public void PlayGameOver()
    {
        if (!sfxEnabled || sfxSource == null || gameOverClip == null) return;
        sfxSource.PlayOneShot(gameOverClip, 1f);
    }
    public void PlayButton()
{
    if (!sfxEnabled || sfxSource == null || buttonClip == null)
        return;

    sfxSource.PlayOneShot(buttonClip);
}
public void PlayVictory()
{
    if (!sfxEnabled || sfxSource == null || victoryClip == null)
        return;

    sfxSource.PlayOneShot(victoryClip);
}

}