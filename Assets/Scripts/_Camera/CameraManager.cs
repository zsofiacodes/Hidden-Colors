using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CameraUI cameraUI;
    [SerializeField] private CinemachineCamera photoPOVCamera;
    [SerializeField] private GameObject playerMesh;

    public event Action<bool> OnTakingPicture;
    public event Action OnPhotoTaken;

    private int photosLeft = 5;
    private Texture2D screenCapture;
    private Objective currentTarget;
    
    private bool isCameraModeActive = false;
    private bool viewingPhoto;

    private void Start()
    {
        photoPOVCamera.Priority = 5;
    }

    private void Update()
    {
        if (GameManager.Instance.currentState != GameState.Tutorial)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (!isCameraModeActive) return;

                EnterCameraMode();
            }

            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                if (!isCameraModeActive) return;

                if (!viewingPhoto)
                {
                    EnterCameraMode();
                }
                else
                {
                    TakeSnapShot();
                }
            }
        }
    }


    private void EnterCameraMode()
    {
        cameraUI.ShowCameraUI(true);
    }

    private void TakeSnapShot()
    {
        UseBattery();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        // Check if the object we are looking at is a valid objective
        if (currentTarget != null)
        {
            if (currentTarget.IsInView(Camera.main))
            {
                currentTarget.IsCompleted = true;
                if (ObjectiveManager.Instance != null)
                {
                    ObjectiveManager.Instance.CompleteObjective(currentTarget, currentTarget.objectiveID);
                }
            }
            currentTarget = null;
        }

        UseBattery();

        viewingPhoto = true;

        ShowPhoto();
    }

    private void ExitCameraMode()
    {
        viewingPhoto = false;
        isCameraModeActive = false;

        playerMesh.SetActive(true);
        photoPOVCamera.Priority = 5;

        cameraUI.ShowCameraUI(false);

        GameManager.Instance.SetState(GameState.Free);
    }

    public void UseBattery()
    {
        if (photosLeft > 0)
        {
            photosLeft--;
            OnPhotoTaken.Invoke();
        }

        if (photosLeft <= 0)
        {
            GameManager.Instance.SetState(GameState.Outro);
        }
    }

    private void ShowPhoto()
    {
        //Button or after sometime?

        cameraUI.ShowCameraUI(true);


        //Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);

        //cameraUI.ReceiveTakenPicture(photoSprite.texture);
        //photoDisplayArea.sprite = photoSprite;

        //cameraUI.ShowPhotoFrame(true);
        //photoFrame.SetActive(true);
    }
}
