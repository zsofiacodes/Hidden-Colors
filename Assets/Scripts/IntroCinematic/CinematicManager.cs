using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{

    public void StartGame()
    {
        Debug.Log("ANIMATION EVENT: StartGame() function called from Timeline."); // New Log
        SceneManager.LoadScene("Game");
    }
}