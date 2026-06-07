using UnityEngine;

public class Objective : MonoBehaviour
{
    public int objectiveID;
    public bool isBeingFocused = false;
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

    public void Focus(bool value)
    {
        isBeingFocused = value;
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