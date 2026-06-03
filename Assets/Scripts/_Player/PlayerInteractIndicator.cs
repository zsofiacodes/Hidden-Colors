using UnityEngine;

public class PlayerInteractIndicator : MonoBehaviour
{
    [SerializeField] private CanvasGroup indicatorCanvasGroup;
    [SerializeField] private PlayerInteractor playerInteractor;

    private void OnEnable()
    {
        playerInteractor.OnWithinInteractRange += ShowIndicator;
    }

    private void OnDisable()
    {
        playerInteractor.OnWithinInteractRange -= ShowIndicator;
    }


    private void Start()
    {
        HideIndicator();
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
