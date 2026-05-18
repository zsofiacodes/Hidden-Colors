using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    [Header("Objective Settings")]
    public string objectiveID;
    public bool isCaptured = false;

    private Renderer targetRenderer;

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    // This is the function called by the PlayerInteractor script
    public void Interact()
    {
        PhotoCapture pc = FindFirstObjectByType<PhotoCapture>();
        if (pc != null && !isCaptured)
        {
            pc.TakePhotoNow(this);
        }
    }

    public bool IsInView(Camera cam)
    {
        if (isCaptured || targetRenderer == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, targetRenderer.bounds);
    }
}