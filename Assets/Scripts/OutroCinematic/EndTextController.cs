using UnityEngine;

public class EndTextController : MonoBehaviour
{
    [SerializeField] private CanvasGroup endTextCanvasGroup;

    private void Start()
    {
        HandleGameStateChanged(GameManager.Instance.currentState);
    }
    private void HandleGameStateChanged(GameState newState)
    {
        if (newState != GameState.FinalReality)
        {
            endTextCanvasGroup.alpha = 0f;
            Debug.Log("EndTextController: Hiding end text because game state is not FinalReality.");
        }
        else
        {
            endTextCanvasGroup.alpha = 1f;
            Debug.Log("EndTextController: Showing end text because game state is FinalReality.");
        }
    }
}
