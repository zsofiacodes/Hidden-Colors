using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    [Header("Photo Taker UI")]
    [SerializeField] private Image photoDisplayTexture;
    [SerializeField] private GameObject photoframe;
    [SerializeField] private CanvasGroup photoFrameCanvasGroup;
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
        photoframe.SetActive(value);

        photoFrameCanvasGroup.alpha = 1f;

        fadingAnimation.SetTrigger("FadeOut");
    }

    public void ReceiveTakenPicture(Sprite texture)
    {
        photoDisplayTexture.sprite = texture;

        StartCoroutine(FadeOutPhoto());
    }

    private IEnumerator FadeOutPhoto()
    {
        yield return new WaitForSeconds(2f);

        photoFrameCanvasGroup.alpha = 0f;
    }
}