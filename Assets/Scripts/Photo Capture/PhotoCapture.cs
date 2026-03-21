using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker UI")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private GameObject checklistPanel; // NEW: Drag your ChecklistPanel here

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera photoPOVCamera;

    [Header("Player Settings")]
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private GameObject playerMesh;

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
    private PhotoTarget currentTarget;

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

    public void TakePhotoNow(PhotoTarget target)
    {
        if (!isCameraModeActive)
        {
            currentTarget = target;
            isCameraModeActive = true;
            cameraUI.SetActive(true);

            if (playerMesh != null) playerMesh.SetActive(false);
            if (photoPOVCamera != null) photoPOVCamera.Priority = 20;

            Cursor.lockState = CursorLockMode.Locked;
            if (playerMovementScript != null) playerMovementScript.enabled = false;
        }
    }

    private IEnumerator CapturePhotoRoutine()
    {
        // --- 1. HIDE ALL UI ---
        cameraUI.SetActive(false);
        if (checklistPanel != null) checklistPanel.SetActive(false);
        viewingPhoto = true;

        // Wait for the end of the frame so the UI is actually gone from the screen
        yield return new WaitForEndOfFrame();

        // --- 2. TAKE THE PHOTO ---
        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        if (currentTarget != null)
        {
            if (currentTarget.IsInView(Camera.main))
            {
                currentTarget.isCaptured = true;
                if (ObjectiveManager.Instance != null)
                {
                    ObjectiveManager.Instance.CheckPhoto(currentTarget.objectiveID);
                }
            }
            currentTarget = null;
        }

        if (currentBattery > 0)
        {
            currentBattery--;
            batteryBars[currentBattery].enabled = false;
        }

        // --- 3. SHOW RESULTS ---
        ShowPhoto();
        // Bring the checklist back on the main HUD (not in the photo)
        if (checklistPanel != null) checklistPanel.SetActive(true);

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

        if (playerMesh != null) playerMesh.SetActive(true);
        if (photoPOVCamera != null) photoPOVCamera.Priority = 5;

        Cursor.lockState = CursorLockMode.None;
        if (playerMovementScript != null) playerMovementScript.enabled = true;
    }
}