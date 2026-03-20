using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    [Header("Objective Settings")]
    public string objectiveID; // This MUST match the ID in your ObjectiveManager (e.g., "CubeGoal")

    private Renderer targetRenderer;

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    // This function checks if the object is actually inside the camera's view
    public bool IsInView(Camera cam)
    {
        if (targetRenderer == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, targetRenderer.bounds);
    }
}