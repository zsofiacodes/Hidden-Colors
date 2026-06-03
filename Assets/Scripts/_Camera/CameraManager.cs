using System;
using System.Collections;
using System.IO;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CameraUI cameraUI;
    [SerializeField] private CinemachineCamera photoPOVCamera;
    [SerializeField] private GameObject playerMesh;

    public event Action<bool> OnTakingPicture;
    public event Action OnPhotoTaken;

    private int photosLeft = 5;
    private Sprite screenCapture;
    private Objective currentTarget;
    
    private bool isCameraModeActive = false;
    private bool viewingPhoto;

    private void Awake()
    {
        screenCapture = Sprite.Create(
            new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false),
            new Rect(0, 0, Screen.width, Screen.height),
            new Vector2(0.5f, 0.5f)
        );
    }

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
                if (!isCameraModeActive)
                    EnterCameraMode();

                else
                    ExitCameraMode();
            }

            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                if (!isCameraModeActive) return;
                
                StartCoroutine(TakeSnapShot());
            }
        }
    }


    private void EnterCameraMode()
    {
        photoPOVCamera.Priority = 20;
        isCameraModeActive = true;
        playerMesh.SetActive(false);
        cameraUI.ShowCameraUI(true);

        GameManager.Instance.SetState(GameState.TakingPicture);
    }

    private void ExitCameraMode()
    {
        photoPOVCamera.Priority = 5;
        isCameraModeActive = false;
        playerMesh.SetActive(true);
        cameraUI.ShowCameraUI(false);

        GameManager.Instance.SetState(GameState.Free);
    }

    private IEnumerator TakeSnapShot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        Sprite screenshotSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        screenCapture = screenshotSprite;

        byte[] bytes = texture.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, "CustomScreenshot.png");
        File.WriteAllBytes(path, bytes);

        //Destroy(texture);

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
        cameraUI.ReceiveTakenPicture(screenCapture);
    }
}
