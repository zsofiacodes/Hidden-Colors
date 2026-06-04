using UnityEngine;

public class WorldSwapManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameObject worldNormal;
    [SerializeField] private GameObject worldPoverty;

    private void OnEnable()
    {
        cameraManager.OnChangeWorld += ShowWorldType;
    }

    private void OnDisable()
    {
        cameraManager.OnChangeWorld -= ShowWorldType;
    }

    private void Start()
    {
        ShowWorldType(true);
    }

    private void ShowWorldType(bool showNormal)
    {
        if (showNormal)
        {
            worldNormal.SetActive(true);
            worldPoverty.SetActive(false);
        }
        else
        {
            worldNormal.SetActive(false);
            worldPoverty.SetActive(true);
        }
    }
}
