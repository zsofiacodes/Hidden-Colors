using UnityEngine;
using UnityEngine.UI;

public class CameraBatteryUI : MonoBehaviour
{
    [SerializeField] private Image[] batterySprites;
    [SerializeField] private CameraManager cameraManager;

    private void OnEnable()
    {
        cameraManager.OnPhotoTaken += CameraManager_OnPhotoTaken;
    }

    private void OnDisable()
    {
        cameraManager.OnPhotoTaken -= CameraManager_OnPhotoTaken;
    }

    private void CameraManager_OnPhotoTaken()
    {
        
    }
}
