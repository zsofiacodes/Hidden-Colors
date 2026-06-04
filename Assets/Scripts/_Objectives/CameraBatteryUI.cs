using UnityEngine;
using UnityEngine.UI;

public class CameraBatteryUI : MonoBehaviour
{
    [SerializeField] private Image[] batterySprites;
    [SerializeField] private CameraManager cameraManager;

    private void OnEnable()
    {
        cameraManager.OnPhotoSuccesfullTaken += CameraManager_OnPhotoTaken;
    }

    private void OnDisable()
    {
        cameraManager.OnPhotoSuccesfullTaken -= CameraManager_OnPhotoTaken;
    }

    private void CameraManager_OnPhotoTaken(int id)
    {
        for (int i = batterySprites.Length - 1; i >= 0; i--)
        {
            if (batterySprites[i].color.a > 0f)
            {
                Color transparent = batterySprites[i].color;
                transparent.a = 0f;

                batterySprites[i].color = transparent;
                break;
            }
        }
    }
}
