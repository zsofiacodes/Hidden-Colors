using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically start game music when entering the "Game" scene
        if (scene.name == "Game")
        {
            PlayMusic("Game", 2.0f);
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        MusicTrack track = musicLibrary.GetTrackFromName(trackName);
        if (track.clip != null)
        {
            StartCoroutine(AnimateMusicCrossfade(track, fadeDuration));
        }
    }

    IEnumerator AnimateMusicCrossfade(MusicTrack nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        float startVol = musicSource.volume;

        while (percent < 1 && fadeDuration > 0)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVol, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack.clip;
        musicSource.Play();

        percent = 0;
        while (percent < 1 && fadeDuration > 0)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            // Fades to the specific volume set for this track in the Library
            musicSource.volume = Mathf.Lerp(0, nextTrack.volume, percent);
            yield return null;
        }
        if (fadeDuration <= 0) musicSource.volume = nextTrack.volume;
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
        musicSource.Stop();
    }
}