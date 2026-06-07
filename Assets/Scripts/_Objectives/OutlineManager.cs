using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    private Objective objective;
    private Outline outline;
    [SerializeField] private PlayerInteractor playerInteractor;
    [SerializeField] private CameraManager cameraManager;
    private bool isCurrentObjective = false;

    private void Awake()
    {
        objective = GetComponent<Objective>();
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        cameraManager.OnPhotomode += HandleOnPhotoModeEvent;
    }

    private void OnDisable()
    {
        cameraManager.OnPhotomode -= HandleOnPhotoModeEvent;
    }

    void Start()
    {
        outline.enabled = false;
    }

    private void HandleOnPhotoModeEvent(bool value)
    {
        if (!isCurrentObjective)
            return;

        if (value)
        {
            OnGainFocus();
        }
        else
        {
            OnLoseFocus();
        }
    }

    public void OnGainFocus()
    {
        if (objective.isCompleted)
            return;

        outline.enabled = true;

        isCurrentObjective = true;
    }

    public void OnLoseFocus()
    {
        outline.enabled = false;

        isCurrentObjective = false;
    }
}