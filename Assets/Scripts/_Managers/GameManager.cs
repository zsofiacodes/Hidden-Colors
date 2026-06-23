using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, IntroCinematic, Tutorial, Free, TakingPicture, Outro, FinalReality }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    [Header("References")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private ObjectiveManager objectiveManager;

    public event Action<GameState> onStateChange;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetState(GameState.MainMenu);
        ResetPlayerPrefs();
    }

    private void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Tutorial:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case GameState.Free:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                FindReferences();
                break;

            case GameState.TakingPicture:
                break;

            case GameState.Outro:
                Debug.Log("Switching to Reality Mode... Starting transition.");
                StartCoroutine(TransitionAndLoadOutro("OutroCinematic"));
                break;

            case GameState.FinalReality:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                StartCoroutine(TransitionAndLoadOutro("Game"));
                break;
        }

        onStateChange?.Invoke(currentState);
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    private IEnumerator TransitionAndLoadOutro(string sceneName)
    {
        if (SceneTransitionUIManager.Instance != null)
        {
            SceneTransitionUIManager.Instance.StartTransition();
            yield return new WaitForSeconds(0.6f);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void FindReferences()
    {
        // Don't look for game-specific managers in the Outro scene
        if (SceneManager.GetActiveScene().name == "OutroCinematic") return;

        if (cameraManager == null)
        {
            cameraManager = FindFirstObjectByType<CameraManager>();
        }

        if (objectiveManager == null)
        {
            objectiveManager = FindFirstObjectByType<ObjectiveManager>();
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("OutroCinematic");
    }
}