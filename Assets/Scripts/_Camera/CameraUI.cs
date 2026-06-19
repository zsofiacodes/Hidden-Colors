using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    [Header("Photo Taker UI")]
    [SerializeField] private Image photoDisplayTexture;
    [SerializeField] private GameObject photoframe;
    [SerializeField] private CanvasGroup photoframeCanvasGroup;
    [SerializeField] private GameObject cameraUI;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    private void Start()
    {
        ShowCameraUI(false);
        photoframe.SetActive(false);
        photoframeCanvasGroup.alpha = 0f;
    }

    public void ShowCameraUI(bool value)
    {
        cameraUI.SetActive(value);
    }

    public void ReceiveTakenPicture(Sprite texture)
    {
        // Simply display the photo
        photoDisplayTexture.sprite = texture;

        photoframe.SetActive(true);
        photoframeCanvasGroup.alpha = 1f;
        fadingAnimation.SetTrigger("FadePhotoIn");
    }

    public void HidePhotoFrame()
    {
        photoframe.SetActive(false);
        photoframeCanvasGroup.alpha = 0f;
    }
}