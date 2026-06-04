using UnityEngine;

public class Objective : MonoBehaviour
{
    public int objectiveID;
    public bool isCompleted = false;
    private Renderer targetRenderer;

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    public void Capture()
    {
        isCompleted = true;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public bool IsInView(Camera cam)
    {
        if (isCompleted || targetRenderer == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, targetRenderer.bounds);
    }
}