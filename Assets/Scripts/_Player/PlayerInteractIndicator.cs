using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteractIndicator : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private CanvasGroup indicatorCanvasGroup;
    [SerializeField] private PlayerInteractor playerInteractor;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2f, 0);

    private RectTransform uiElement;
    private Transform targetObject;

    private void Awake()
    {
        uiElement = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        cameraManager.OnPhotomode += IndiactorHelperMethod;
        playerInteractor.OnWithinInteractRange += IndiactorHelperMethod;
        playerInteractor.OnUpdateActiveObjective += UpdateActiveObjective;
    }

    private void OnDisable()
    {
        cameraManager.OnPhotomode -= IndiactorHelperMethod;
        playerInteractor.OnWithinInteractRange -= IndiactorHelperMethod;
        playerInteractor.OnUpdateActiveObjective -= UpdateActiveObjective;
    }


    private void Start()
    {
        HideIndicator();
    }

    private void Update()
    {
        if (targetObject == null) return;

        Vector3 targetPositionWithOffset = targetObject.position + worldOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPositionWithOffset);

        uiElement.position = screenPos;

        //if (screenPos.z > 0)
        //{
        //    uiElement.position = screenPos;
        //    Debug.Log($"True");
        //}
        //else
        //{
        //    Debug.Log($"False");
        //}
    }

    public void IndiactorHelperMethod(bool value)
    {
        if (value)
        {
            ShowIndicator();
        }
        else
        {
            HideIndicator();
        }
    }

    public void UpdateActiveObjective(Objective newObjective)
    {
        targetObject = newObjective.transform;
    }

    public void ShowIndicator()
    {
        indicatorCanvasGroup.alpha = 1f;
    }

    public void HideIndicator()
    {
        indicatorCanvasGroup.alpha = 0f;
    }
}
