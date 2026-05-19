using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    public Image fadeOverlay;
    public float fadeDuration = 1.0f;

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayGame);
        settingButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        StartCoroutine(FadeAndLoad());
        GameManager.Instance.SetState(GameState.IntroCinematic);
    }

    IEnumerator FadeAndLoad()
    {
        float timer = 0f;
        fadeOverlay.gameObject.SetActive(true);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, progress);
            yield return null;
        }

        SceneTransitionUIManager.Instance.StartTransition();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("IntroCinematic");
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}