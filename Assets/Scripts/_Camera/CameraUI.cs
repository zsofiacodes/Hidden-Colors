using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    [Header("Photo Taker UI")]
    [SerializeField] private Image photoDisplayTexture;
    [SerializeField] private Sprite photoTexture;
    [SerializeField] private GameObject photoframe;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private GameObject[] batteryUIBars;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    private void Start()
    {
        ShowCameraUI(false);
        StartCoroutine(ShowPhotoFrame(false));
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
        photoframe.SetActive(value);

        photoDisplayTexture.sprite = photoTexture;

        fadingAnimation.SetTrigger("FadePhotoIn");

        yield return new WaitForSeconds(10f);

        photoframe.SetActive(false);
    }
}