using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        // Safety check: Only play if the MusicManager exists
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic("MainMenu");
        }
    }

    public void Play()
    {
        // 1. Tell the music to switch to "Game" FIRST so the fade starts
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic("Game");
        }

        // 2. Then load the scene
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}