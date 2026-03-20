using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera photoPOVCamera;

    [Header("Player Settings")]
    [SerializeField] private MonoBehaviour playerMovementScript; // Drag your Player Movement script here

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    [Header("Battery UI")]
    [SerializeField] private List<Image> batteryBars;
    private int currentBattery;

    private Texture2D screenCapture;
    private bool viewingPhoto;
    private bool isCameraModeActive;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraUI.SetActive(false);
        photoFrame.SetActive(false);

        if (photoPOVCamera != null) photoPOVCamera.Priority = 5;
        currentBattery = batteryBars.Count;
    }

    private void Update()
    {
        if (isCameraModeActive && Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (!viewingPhoto)
            {
                StartCoroutine(CapturePhotoRoutine());
            }
            else
            {
                RemovePhoto();
            }
        }
    }

    public void TakePhotoNow()
    {
        if (!isCameraModeActive)
        {
            isCameraModeActive = true;
            cameraUI.SetActive(true);

            if (photoPOVCamera != null) photoPOVCamera.Priority = 20;

            // Lock the cursor and disable player movement
            Cursor.lockState = CursorLockMode.Locked;
            if (playerMovementScript != null) playerMovementScript.enabled = false;
        }
    }

    private IEnumerator CapturePhotoRoutine()
    {
        cameraUI.SetActive(false);
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        // --- OBJECTIVE SYSTEM LOGIC START ---

        // 1. Find all objects in the scene that have the PhotoTarget script
        PhotoTarget[] allTargets = Object.FindObjectsByType<PhotoTarget>(FindObjectsSortMode.None);

        foreach (PhotoTarget target in allTargets)
        {
            // 2. Check if that specific object is visible to the camera
            if (target.IsInView(Camera.main))
            {
                // 3. Tell the ObjectiveManager to cross it off the list
                if (ObjectiveManager.Instance != null)
                {
                    ObjectiveManager.Instance.CheckPhoto(target.objectiveID);
                }
            }
        }

        // --- OBJECTIVE SYSTEM LOGIC END ---

        // --- BATTERY LOGIC ---
        if (currentBattery > 0)
        {
            currentBattery--;
            batteryBars[currentBattery].enabled = false; // Hides the top-most bar
        }

        if (currentBattery <= 0)
        {
            Debug.Log("Battery Dead! Trigger Ending Cinematic.");
            // We can add the ending trigger here later!
        }

        ShowPhoto();
        if (cameraAudio != null) cameraAudio.Play();
    }

    private void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;
        photoFrame.SetActive(true);
        if (fadingAnimation != null) fadingAnimation.Play("PhotoFade");
    }

    private void RemovePhoto()
    {
        viewingPhoto = false;
        isCameraModeActive = false;
        photoFrame.SetActive(false);
        cameraUI.SetActive(false);

        if (photoPOVCamera != null) photoPOVCamera.Priority = 5;

        // Unlock cursor and re-enable player movement
        Cursor.lockState = CursorLockMode.None;
        if (playerMovementScript != null) playerMovementScript.enabled = true;
    }
}