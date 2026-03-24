using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [Header("Transition Settings")]
    public Image fadeOverlay;
    public string nextSceneName = "IntroCinematic";
    public float fadeDuration = 1.0f;

    public void PlayGame()
    {
        // Stop menu music immediately
        MusicManager.Instance.StopMusic(0f);
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        float timer = 0f;
        fadeOverlay.gameObject.SetActive(true);
        float startVolume = AudioListener.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, progress);
            AudioListener.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);

        // Reset Listener volume
        AudioListener.volume = 1f;

        // Start Cinematic music instantly upon scene load
        MusicManager.Instance.PlayMusic("Intro", 0f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}