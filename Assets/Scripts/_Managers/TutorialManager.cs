using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Button closeTutorialButton;
    [SerializeField] private GameObject tutorialPanel;

    private void Awake()
    {
        closeTutorialButton.onClick.AddListener(CloseTutorial);
    }

    private void Start()
    {
        ShowTutorialPanel();
    }

    private bool ShowTutorialPanel()
    {
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            tutorialPanel.SetActive(false);
            return false;
        }

        tutorialPanel.SetActive(true);
        return true;
    }

    private void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        PlayerPrefs.SetInt("TutorialDone", 1);
        PlayerPrefs.Save();
        GameManager.Instance.SetState(GameState.Free);
    }
}