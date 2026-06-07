using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip introCutsceneMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip cameraShutterSound;
    [SerializeField] private AudioClip kidLaughingSound;
    [SerializeField] private AudioClip kidFallSound;
    [SerializeField] private AudioClip uiClickSound;

    // Keep track of the active fade routine
    private Coroutine _musicFadeCoroutine;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip clipToPlay = scene.name switch
        {
            "MainMenu" => mainMenuMusic,
            "IntroCinematic" => introCutsceneMusic,
            "Game" => gameplayMusic,
            _ => null
        };

        if (clipToPlay != null) PlayBackgroundMusic(clipToPlay);
    }

    public void PlayBackgroundMusic(AudioClip musicClip, float duration = 1.0f)
    {
        // Cancel the previous transition to prevent coroutine overlap
        if (_musicFadeCoroutine != null) StopCoroutine(_musicFadeCoroutine);

        _musicFadeCoroutine = StartCoroutine(TransitionMusic(musicClip, duration));
    }

    private IEnumerator TransitionMusic(AudioClip newClip, float duration)
    {
        // 1. Fade Out existing
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0, elapsed / (duration / 2f));
            yield return null;
        }

        // 2. Switch Clip
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // 3. Fade In new
        elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, 0.5f, elapsed / (duration / 2f));
            yield return null;
        }
        musicSource.volume = 0.5f;
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        // Use PlayOneShot so SFX don't interrupt music or each other
        sfxSource.PlayOneShot(sfxClip);
    }

    public void PlayKidLaughingSFX()
    {
        PlaySFX(kidLaughingSound);
    }



    public void PlayKidFallSFX()
    {
        PlaySFX(kidFallSound);
    }

    public void PlayShutterSoundSFX()
    {
        PlaySFX(cameraShutterSound);
    }
}