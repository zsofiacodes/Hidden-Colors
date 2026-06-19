using TMPro;
using UnityEngine;

public class ObjectivesListUI : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI[] objectiveTextSlots;


    private void Start()
    {
        // If we are in the Final Reality, hide the objectives list immediately
        if (GameManager.Instance.currentState == GameState.FinalReality)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void OnEnable()
    {
        cameraManager.OnPhotomode += ToggleObjectiveUI;
        cameraManager.OnPhotoSuccesfullTaken += UpdateObjectivesList;
    }

    private void OnDisable()
    {
        cameraManager.OnPhotomode -= ToggleObjectiveUI;
        cameraManager.OnPhotoSuccesfullTaken -= UpdateObjectivesList;
    }

    private void ToggleObjectiveUI(bool value)
    {
        if (value) 
        {
            canvasGroup.alpha = 1f;
        }
        else 
        {
            canvasGroup.alpha = 0f;
        }
    }

    private void UpdateObjectivesList(int id)
    {
        
        for (int i = 0; i < objectiveTextSlots.Length; i++)
        {
            if (i == id)
            {
                objectiveTextSlots[i].color = Color.green;
                objectiveTextSlots[i].fontStyle = FontStyles.Strikethrough;
            }
        }
    }
}
