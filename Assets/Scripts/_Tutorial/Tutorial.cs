using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.stateChange += CloseTutorial;
    }

    private void OnDestroy()
    {
        GameManager.Instance.stateChange -= CloseTutorial;
    }

    public void CloseTutorial(GameState state)
    {
        if (state == GameState.Tutorial)
        {
            //Do something
        }
    }
}