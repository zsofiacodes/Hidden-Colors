using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Automatically play ambient music if starting directly in the Game scene
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game")
            {
                PlayMusic("Game", 2.0f);
            }
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        AudioClip clip = musicLibrary.GetClipFromName(trackName);
        if (clip != null)
        {
            StartCoroutine(AnimateMusicCrossfade(clip, fadeDuration));
        }
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        float startVol = musicSource.volume;

        // Fade out current track
        while (percent < 1 && fadeDuration > 0)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVol, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        // Fade in new track - capped at 0.4f volume so SFX are audible
        percent = 0;
        while (percent < 1 && fadeDuration > 0)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 0.4f, percent);
            yield return null;
        }
        if (fadeDuration <= 0) musicSource.volume = 0.4f;
    }

    public void StopMusic(float fadeDuration = 1.0f)
    {
        StartCoroutine(AnimateMusicFadeOut(fadeDuration));
    }

    IEnumerator AnimateMusicFadeOut(float fadeDuration)
    {
        float percent = 0;
        float startVolume = musicSource.volume;
        while (percent < 1 && fadeDuration > 0)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0, percent);
            yield return null;
        }
        musicSource.volume = 0;
        musicSource.Stop();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the scene we just entered is your gameplay scene
        if (scene.name == "Game") // <--- Make sure this matches your scene name!
        {
            PlayMusic("Game", 2.0f); // <--- Make sure "Game" is in your MusicLibrary
        }
    }
}