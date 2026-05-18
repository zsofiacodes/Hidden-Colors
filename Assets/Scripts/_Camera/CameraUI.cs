using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;

public class CameraUI : MonoBehaviour
{
    [Header("Photo Taker UI")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private GameObject[] batteryUIBars;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    private Texture2D screenCapture;

    private void Start()
    {
        ShowCameraUI(false);
        ShowPhotoFrame(false);
    }

    public void ShowCameraUI(bool value)
    {
        cameraUI.SetActive(value);
    }

    public void ShowPhotoFrame(bool value)
    {
        photoFrame.SetActive(value);
    }

    public void ReceiveTakenPicture(Texture2D texture)
    {
        screenCapture = texture;
    }
}