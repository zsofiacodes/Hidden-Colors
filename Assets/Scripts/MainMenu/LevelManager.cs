using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This matches the way your MainMenu script was trying to call it
    public void LoadScene(string sceneName, string transitionType)
    {
        // For now, it just loads the scene normally
        // Later you can add "CrossFade" logic here if you follow a transition tutorial!
        SceneManager.LoadScene(sceneName);
    }
}