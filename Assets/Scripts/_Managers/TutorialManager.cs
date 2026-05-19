using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Button closeTutorialButton;
    [SerializeField] private GameObject tutorialPanel;

    private void Awake()
    {
        closeTutorialButton.onClick.AddListener(CloseTutorial);
    }

    private void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        GameManager.Instance.SetState(GameState.Free);
    }
}
