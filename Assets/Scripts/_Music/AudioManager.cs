using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip introCustsceneMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip cameraShutterSound;
    [SerializeField] private AudioClip kidLaughingSound;
    [SerializeField] private AudioClip kidFallSound;
    [SerializeField] private AudioClip uiClickSound;


    private void Awake()
    {
        if (Instance != null) 
        {
            Destroy(gameObject); return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayBackgroundMusic(mainMenuMusic);
        }
        if (scene.name == "IntroCinematic")
        {
            PlayBackgroundMusic(introCustsceneMusic);
        }
        if (scene.name == "Game")
        {
            PlayBackgroundMusic(gameplayMusic);
        }
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        StopMusic();
        StartCoroutine(FadeInMusic(musicClip, 1f, 0.5f, true));
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        StartCoroutine(FadeInMusic(sfxClip, 0.1f, 1f, false));
    }

    public IEnumerator FadeInMusic(AudioClip targetClip, float fadeDuration, float targetVolume, bool shouldLoop)
    {
        audioSource.clip = targetClip;
        audioSource.volume = 0;
        audioSource.loop = true;
        audioSource.Play();

        float currentTime = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;

            // Linearly climb from 0 to our target volume over time
            audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / fadeDuration);

            // Pause here and wait for the next frame
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic(1.5f));
    }

    private IEnumerator FadeOutMusic(float fadeDuration)
    {
        float startVolume = audioSource.volume;
        float currentTime = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();

        audioSource.volume = startVolume;
    }

    public void PlayKidLaughingSFX()
    {
        PlaySFX(kidLaughingSound);
    }

    public void PlayKidFallSFX()
    {
        PlaySFX(kidFallSound);
    }
}
