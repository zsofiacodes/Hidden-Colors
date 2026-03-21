using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    [Header("Objective Settings")]
    public string objectiveID;
    public bool isCaptured = false; // New: Prevents double-capturing

    private Renderer targetRenderer;

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    public bool IsInView(Camera cam)
    {
        // If already captured, we ignore this object
        if (isCaptured || targetRenderer == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, targetRenderer.bounds);
    }
}