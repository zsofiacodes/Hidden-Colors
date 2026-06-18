using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    [Header("Photo Taker UI")]
    [SerializeField] private Image photoDisplayTexture;
    [SerializeField] private Sprite photoTexture;
    [SerializeField] private GameObject photoframe;
    [SerializeField] private CanvasGroup photoframeCanvasGroup;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private GameObject[] batteryUIBars;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    private void Start()
    {
        ShowCameraUI(false);
        photoframeCanvasGroup.alpha = 0f;
    }

    public void ShowCameraUI(bool value)
    {
        cameraUI.SetActive(value);
    }

    public void ReceiveTakenPicture(Sprite texture)
    {
        photoTexture = texture;
        photoDisplayTexture.sprite = photoTexture;

        StartCoroutine(ShowPhotoFrame(true));
    }

    public IEnumerator ShowPhotoFrame(bool value)
    {
        photoframe.SetActive(true);

        photoDisplayTexture.sprite = photoTexture;
        fadingAnimation.SetTrigger("FadePhotoIn");

        yield return new WaitForSeconds(3f);

        float duration = 1.0f; // Time in seconds
        float elapsed = 0f;

        photoframeCanvasGroup.alpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            photoframeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null; // Wait for the next frame
        }
        photoframeCanvasGroup.alpha = 0f;
        photoframe.SetActive(false);
    }
}