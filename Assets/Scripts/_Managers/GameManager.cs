using UnityEngine;

public enum GameState { Tutorial, Imagination, Transition, Reality }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    [Header("References")]
    public GameObject tutorialUI;

    [Header("Battery Settings")]
    public int photosLeft = 5;
    public GameObject[] batteryUIBars; // Drag your 5 bars here (0 to 4)

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
                if (tutorialUI != null) tutorialUI.SetActive(true);
                break;

            case GameState.Imagination:
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (tutorialUI != null) tutorialUI.SetActive(false);
                break;

            case GameState.Transition:
                // This triggers the moment the 5th photo is snapped
                SetState(GameState.Reality);
                break;

            case GameState.Reality:
                Debug.Log("Switching to Reality Mode...");
                // We will put the code to turn off the happy filter here next!
                break;
        }
    }

    public void UseBattery()
    {
        if (photosLeft > 0)
        {
            photosLeft--;
            // Hides the bars one by one
            if (photosLeft < batteryUIBars.Length && batteryUIBars[photosLeft] != null)
            {
                batteryUIBars[photosLeft].SetActive(false);
            }
        }

        if (photosLeft <= 0)
        {
            SetState(GameState.Transition);
        }
    }
}