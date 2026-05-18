using System;
using UnityEngine;

public enum GameState { Tutorial, Free, TakingPicture, Outro}

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
        Instance = this;
    }

    private void Start()
    {
        SetState(GameState.Tutorial);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Tutorial:
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case GameState.Free:
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case GameState.TakingPicture:
               
                break;

            case GameState.Outro:
                Debug.Log("Switching to Reality Mode...");
                break;
        }

        stateChange.Invoke(currentState);
    }
}