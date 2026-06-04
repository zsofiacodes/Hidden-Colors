using TMPro;
using UnityEngine;

public class ObjectivesListUI : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private TextMeshProUGUI[] objectiveTextSlots;

    private void OnEnable()
    {
        cameraManager.OnPhotoSuccesfullTaken += UpdateObjectivesList;
    }

    private void OnDisable()
    {
        cameraManager.OnPhotoSuccesfullTaken -= UpdateObjectivesList;
    }

    private void UpdateObjectivesList(int id)
    {
        objectiveTextSlots[id].color = Color.green;
        objectiveTextSlots[id].fontStyle = FontStyles.Strikethrough;
}
}
