using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlbumCloser : MonoBehaviour
{
    [SerializeField] private Button closeAlbumButton;

    private void Awake()
    {
        closeAlbumButton.onClick.AddListener(CloseAlbumAndEndGame);
    }

    public void CloseAlbumAndEndGame()
    {
        GameManager.Instance.SetState(GameState.FinalReality);
        SceneManager.LoadScene("Game");
    }
}