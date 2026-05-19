using System;
using UnityEngine;

public enum GameState { MainMenu, IntroCinematic, Tutorial, Free, TakingPicture, Outro}

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
                Debug.Log("Switching to Reality Mode...");
                break;
        }

        stateChange?.Invoke(currentState);
    }

    public void FindReferences()
    {
        if (cameraManager == null)
        {
            cameraManager = FindFirstObjectByType<CameraManager>();
        }

        if (objectiveManager == null)
        {
            objectiveManager = FindFirstObjectByType<ObjectiveManager>();
        }
    }
}