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
    [SerializeField] private GameObject checklistPanel;

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera photoPOVCamera;

    [Header("Player Settings")]
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private GameObject playerMesh;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    private Texture2D screenCapture;
    private bool viewingPhoto;
    private bool isCameraModeActive;
    private PhotoTarget currentTarget;

    private void Start()
    {
        // Initialize the texture for capturing the screen
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Ensure UI is hidden at start
        cameraUI.SetActive(false);
        photoFrame.SetActive(false);

        if (photoPOVCamera != null) photoPOVCamera.Priority = 5;
    }

    private void Update()
    {
        // Connection: Check with Boss if we are allowed to use the camera
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameState.Imagination) return;

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

    // Called by PhotoTarget when player presses E
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
        cameraUI.SetActive(false);
        if (checklistPanel != null) checklistPanel.SetActive(false);
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        // Check if the object we are looking at is a valid objective
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

        // Connection: Tell the Boss to use one battery segment
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UseBattery();
        }

        ShowPhoto();

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

        Cursor.lockState = CursorLockMode.Locked;
        if (playerMovementScript != null) playerMovementScript.enabled = true;
    }
}