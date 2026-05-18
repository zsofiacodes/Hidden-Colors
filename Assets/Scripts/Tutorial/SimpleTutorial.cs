using UnityEngine;

public class SimpleTutorial : MonoBehaviour
{
    public void CloseTutorial()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameState.Imagination);
        }
    }
}