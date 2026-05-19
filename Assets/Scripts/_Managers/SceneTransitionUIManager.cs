using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionUIManager : MonoBehaviour
{
    public static SceneTransitionUIManager Instance { get; private set; }

    [SerializeField] private CanvasGroup transitionCanvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() { SceneManager.sceneLoaded += StartEndTransition; }
    private void OnDisable() { SceneManager.sceneLoaded -= StartEndTransition; }

    public void StartTransition()
    {
        transitionCanvasGroup.DOKill();
        transitionCanvasGroup.alpha = 0;
        transitionCanvasGroup.DOFade(1, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void StartEndTransition(Scene scene, LoadSceneMode mode)
    {
        transitionCanvasGroup.DOKill();
        transitionCanvasGroup.alpha = 1;
        transitionCanvasGroup.DOFade(0, 0.5f).SetEase(Ease.InOutQuad);
    }
}
