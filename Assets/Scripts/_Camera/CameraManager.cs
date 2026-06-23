using System;
using System.Collections;
using System.IO;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CameraUI cameraUI;
    [SerializeField] private CinemachineCamera photoPOVCamera;
    [SerializeField] private GameObject playerMesh;
    [SerializeField] private GameObject playerMeshPoverty;
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

    [Header("Render Textures")]
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
        if (GameManager.Instance.currentState == GameState.FinalReality)
            return;

        if (isCameraModeActive)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll != 0)
            {
                float zoomAmount = scroll * 0.05f;
                float currentFOV = photoPOVCamera.Lens.FieldOfView;
                currentFOV -= zoomAmount * zoomSpeed;
                photoPOVCamera.Lens.FieldOfView = Mathf.Clamp(currentFOV, minFOV, maxFOV);
            }
        }

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
        if (GameManager.Instance.currentState == GameState.FinalReality) return false;

        Vector3 rayStart = rayStartPoint.position + (rayStartPoint.forward * 0.1f);
        Ray ray = new Ray(rayStart, rayStartPoint.forward);

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
        playerMeshPoverty.SetActive(false);
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
        playerMeshPoverty.SetActive(true);
        playerController.enabled = true;
        cameraUI.ShowCameraUI(false);
        OnPhotomode?.Invoke(true);
        GameManager.Instance.SetState(GameState.Free);
    }

    private void ClampCameraMovement(bool isClamped)
    {
        var panTilt = photoPOVCamera.GetComponent<CinemachinePanTilt>();
        if (panTilt == null) return;
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
        ScreenCaptureLogic(id, "poverty", povertyRT);
        ScreenCaptureLogic(id, "normal", normalRT);
        Sprite photo = LoadSingleImage(id, "normal");
        cameraUI.ReceiveTakenPicture(photo);
        UseBattery();
        StartCoroutine(DisplayAndCloseSequence());
    }

    private IEnumerator DisplayAndCloseSequence()
    {
        yield return new WaitForSeconds(3f);
        cameraUI.HidePhotoFrame();
        ExitCameraMode();
    }

    private void ScreenCaptureLogic(int areaID, string state, RenderTexture rt)
    {
        RenderTextureDescriptor desc = new RenderTextureDescriptor(rt.width, rt.height, RenderTextureFormat.ARGB32, 0);
        desc.sRGB = true;
        RenderTexture tempRT = RenderTexture.GetTemporary(desc);
        Graphics.Blit(rt, tempRT);
        RenderTexture.active = tempRT;
        Texture2D texture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRT);
        string fileName = $"Area{areaID}_{state}.png";
        string path = Path.Combine(Application.persistentDataPath, fileName);
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        screenCapture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public Sprite LoadSingleImage(int areaID, string state)
    {
        string fileName = $"Area{areaID}_{state}.png";
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(fullPath)) return null;
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
}