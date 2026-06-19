using UnityEngine;

public class WorldSwapManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask defaultLayers;
    [SerializeField] private LayerMask normalLayers;
    [SerializeField] private LayerMask povertyLayers;

    private void OnEnable()
    {
        GameManager.Instance.onStateChange += ShowWorldType;
    }

    private void OnDisable()
    {
        GameManager.Instance.onStateChange -= ShowWorldType;
    }

    private void Start()
    {
        ShowWorldType(GameManager.Instance.currentState);
    }

    public void ShowWorldType(GameState newState)
    {
        if (newState == GameState.FinalReality)
        {
            mainCamera.cullingMask = defaultLayers | povertyLayers;
        }
        else
        {
            mainCamera.cullingMask = defaultLayers | normalLayers;
        }
    }
}