using System;
using System.Collections;
using System.IO;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CameraUI cameraUI;
    [SerializeField] private CinemachineCamera photoPOVCamera;
    [SerializeField] private GameObject playerMesh;
    [SerializeField] private ThirdPersonController playerController;

    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform rayStartPoint;

    [Header("Zoom Settings")]
    [SerializeField] private float minFOV = 15f;
    [SerializeField] private float maxFOV = 60f;
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Camera Culling")]
    [SerializeField] private Camera normalCamera;
    [SerializeField] private Camera povertyCamera;

    [Header("Camera Culling")]
    [SerializeField] private RenderTexture normalRT;
    [SerializeField] private RenderTexture povertyRT;

    public event Action<bool> OnPhotomode;
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

        ClampCameraMovement(false);
    }

    private void Update()
    {
        if (GameManager.Instance.currentState == GameState.Tutorial) return;

        // --- NEW: Zoom Logic ---
        if (isCameraModeActive)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            if (scroll != 0)
            {
                // Remove * Time.deltaTime to make it reactive to the scroll wheel movement
                // Increase the 0.05f multiplier to make it zoom faster if needed
                float zoomAmount = scroll * 0.05f;

                float currentFOV = photoPOVCamera.Lens.FieldOfView;
                currentFOV -= zoomAmount * zoomSpeed; // zoomSpeed now acts as a simple sensitivity multiplier

                photoPOVCamera.Lens.FieldOfView = Mathf.Clamp(currentFOV, minFOV, maxFOV);
            }
        }
        // -----------------------

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isCameraModeActive && TryGetValidObjective(out Objective objective))
            {
                currentTarget = objective;
                currentObjectiveID = objective.objectiveID;
                EnterCameraMode();
            }
            else if (isCameraModeActive)
            {
                ExitCameraMode();
            }
        }

        if (isCameraModeActive && Keyboard.current.cKey.wasPressedThisFrame && currentTarget != null && !currentTarget.isCompleted)
        {
            TakeSnapShot(currentTarget.objectiveID);
        }
    }


    private bool TryGetValidObjective(out Objective foundObjective)
    {
        foundObjective = null;

        Vector3 rayStart = rayStartPoint.position + (rayStartPoint.forward * 0.1f);
        Ray ray = new Ray(rayStart, rayStartPoint.forward);

        // This is the line that matters! 
        // It will now check everything assigned to the 'interactableLayer' mask.
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out Objective obj) && !obj.isCompleted)
            {
                foundObjective = obj;
                return true;
            }

        }
        return false;

    }

    private void EnterCameraMode()
    {
        ClampCameraMovement(true);

        photoPOVCamera.Priority = 20;
        isCameraModeActive = true;
        playerMesh.SetActive(false);
        playerController.enabled = false;
        cameraUI.ShowCameraUI(true);

        OnPhotomode?.Invoke(false);

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

        OnPhotomode?.Invoke(true);

        GameManager.Instance.SetState(GameState.Free);
    }

    private void ClampCameraMovement(bool isClamped)
    {
        var panTilt = photoPOVCamera.GetComponent<CinemachinePanTilt>();

        if (isClamped)
        {
            panTilt.TiltAxis.Range = new Vector2(-45f, 100f);

            panTilt.PanAxis.Range = new Vector2(-15f, 15f);
        }
        else
        {
            panTilt.TiltAxis.Range = new Vector2(-40f, 40f);
            panTilt.PanAxis.Range = new Vector2(-180f, 180f);
        }
    }


    private void TakeSnapShot(int id)
    {
        // 1. Capture the images
        ScreenCaptureLogic(id, "poverty", povertyRT);
        ScreenCaptureLogic(id, "normal", normalRT);

        // 2. Load and display immediately
        Sprite photo = LoadSingleImage(id, "normal");
        cameraUI.ReceiveTakenPicture(photo);

        // 3. Handle game logic
        UseBattery();

        // 4. Start the sequence timer
        StartCoroutine(DisplayAndCloseSequence());
    }

    private IEnumerator DisplayAndCloseSequence()
    {
        // The photo is already visible. 
        // We wait for the 3 seconds requested.
        yield return new WaitForSeconds(3f);

        // After the wait, clean up UI and exit the mode
        cameraUI.HidePhotoFrame();
        ExitCameraMode();
    }

    private void ScreenCaptureLogic(int areaID, string state, RenderTexture rt)
    {
        // Create a temporary Render Texture that is explicitly in sRGB format
        RenderTextureDescriptor desc = new RenderTextureDescriptor(rt.width, rt.height, RenderTextureFormat.ARGB32, 0);
        desc.sRGB = true; // This forces the color conversion!

        RenderTexture tempRT = RenderTexture.GetTemporary(desc);

        // Copy the contents of your camera's RT to our sRGB-enabled temporary RT
        Graphics.Blit(rt, tempRT);

        // Now read from the temporary RT
        RenderTexture.active = tempRT;
        Texture2D texture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Clean up
        RenderTexture.ReleaseTemporary(tempRT);

        // Save to file
        string fileName = $"Area{areaID}_{state}.png";
        string path = Path.Combine(Application.persistentDataPath, fileName);
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        // Create sprite for UI
        Sprite screenshotSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        screenCapture = screenshotSprite;
    }

    public void OpenSaveFolder()
    {
        // Using System.Diagnostics.ProcessStartInfo ensures it opens smoothly across OS environments
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
        {
            FileName = Application.persistentDataPath,
            UseShellExecute = true,
            Verb = "open"
        });
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

    private IEnumerator ShowPhoto(int areaID, string state)
    {
        cameraUI.ReceiveTakenPicture(LoadSingleImage(areaID, state));

        // Reduced to 1 second for a quick "snap" feel
        yield return new WaitForSeconds(1f);

        ExitCameraMode();
    }
}