using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.onStateChange += CloseTutorial;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onStateChange -= CloseTutorial;
    }

    public void CloseTutorial(GameState state)
    {
        if (state == GameState.Tutorial)
        {
            //Do something
        }
    }
}