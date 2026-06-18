using System;
using System.Collections; // Required for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, IntroCinematic, Tutorial, Free, TakingPicture, Outro }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    [Header("References")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private ObjectiveManager objectiveManager;

    public event Action<GameState> stateChange;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetState(GameState.MainMenu);
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
                StartCoroutine(TransitionAndLoadOutro());
                break;
        }

        stateChange?.Invoke(currentState);
    }

    // This is now outside SetState where it belongs!
    private IEnumerator TransitionAndLoadOutro()
    {
        if (SceneTransitionUIManager.Instance != null)
        {
            SceneTransitionUIManager.Instance.StartTransition();
            yield return new WaitForSeconds(0.6f);
        }

        SceneManager.LoadScene("OutroCinematic");
    }

    public void FindReferences()
    {
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