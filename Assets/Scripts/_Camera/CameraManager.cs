using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CameraUI cameraUI;
    [SerializeField] private CinemachineCamera photoPOVCamera;
    [SerializeField] private GameObject playerMesh;
    [SerializeField] private ThirdPersonController playerController;

    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayer;

    public event Action<int> OnPhotoSuccesfullTaken;
    public event Action<bool> OnChangeWorld;

    private int photosLeft = 5;
    private Sprite screenCapture;
    private Objective currentTarget;
    
    private bool isCameraModeActive = false;
    private int currentObjectiveID;

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
                if (!isCameraModeActive && TryGetValidObjective(out Objective objective))
                    EnterCameraMode();

                else
                    ExitCameraMode();
            }

            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                if (!isCameraModeActive) return;

                ObjectiveInsightCheck();
            }
        }
    }

    private void ObjectiveInsightCheck()
    {
        // 1. Logic check: Is there a valid, incomplete objective in sight?
        if (TryGetValidObjective(out Objective objective))
        {
            // 2. Action: If we found one, perform the snapshot
            currentTarget = objective;
            currentObjectiveID = objective.objectiveID;

            StartCoroutine(TakeSnapShot(objective.objectiveID));
        }
        else
        {
            // Handle the "nothing found" state
            currentTarget = null;
            currentObjectiveID = -1;
            Debug.Log("No valid, incomplete objective in sight.");
        }
    }

    private bool TryGetValidObjective(out Objective foundObjective)
    {
        foundObjective = null;
        Ray ray = new Ray(photoPOVCamera.transform.position, photoPOVCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            Debug.DrawRay(photoPOVCamera.transform.position, photoPOVCamera.transform.forward * interactRange, Color.green);

            // Check if it has the component AND it is not already completed
            if (hit.collider.TryGetComponent<Objective>(out var objective) && !objective.isCompleted)
            {
                foundObjective = objective;
                return true;
            }
        }
        return false;
    }

    private void EnterCameraMode()
    {
        //Make a method that clamp the rotation of the POV camera to prevent the player from looking at their own body and breaking immersion.
        ClampCameraMovement(true);

        photoPOVCamera.Priority = 20;
        isCameraModeActive = true;
        playerMesh.SetActive(false);
        playerController.enabled = false;
        cameraUI.ShowCameraUI(true);

        GameManager.Instance.SetState(GameState.TakingPicture);
    }

   
    private void ExitCameraMode()
    {
        ClampCameraMovement(false);

        photoPOVCamera.Priority = 5;
        isCameraModeActive = false;
        playerMesh.SetActive(true);
        playerController.enabled = true;
        cameraUI.ShowCameraUI(false);

        GameManager.Instance.SetState(GameState.Free);
    }

    private void ClampCameraMovement(bool isClamped)
    {
        var panTilt = photoPOVCamera.GetComponent<CinemachinePanTilt>();

        if (isClamped)
        {
            // Set range to 0 to effectively "lock" the tilt
            panTilt.TiltAxis.Range = new Vector2(-5, 5f);
            panTilt.PanAxis.Range = new Vector2(-15f, 15f);
        }
        else
        {
            // Restore standard tilt range
            panTilt.TiltAxis.Range = new Vector2(-40, 40f);
            panTilt.PanAxis.Range = new Vector2(-180, 180f);
        }
    }


    private IEnumerator TakeSnapShot(int id)
    {
        yield return new WaitForEndOfFrame();

        OnChangeWorld?.Invoke(false);
        ScreenCaptureLogic(id, "poverty");

        yield return new WaitForEndOfFrame();

        OnChangeWorld?.Invoke(true);
        ScreenCaptureLogic(id, "normal");

        UseBattery();

        ShowPhoto();
    }

    private void ScreenCaptureLogic(int areaID, string state)
    {
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        Sprite screenshotSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        string fileName = $"Area{areaID}_{state}.png";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        Destroy(texture);

        screenCapture = screenshotSprite;
    }

    public Sprite LoadSingleImage(int areaID, string state)
    {
        string fileName = $"Area{areaID}_{state}.png";
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"No screenshot found at: {fullPath}");
            return null;
        }

        byte[] fileData = File.ReadAllBytes(fullPath);

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public void UseBattery()
    {
        if (photosLeft > 0)
        {
            photosLeft--;
            OnPhotoSuccesfullTaken?.Invoke(currentObjectiveID);
        }
    }

    private void ShowPhoto()
    {
        cameraUI.ReceiveTakenPicture(screenCapture);
    }
}
