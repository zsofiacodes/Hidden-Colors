using UnityEngine;

public class Objective : MonoBehaviour, IObjective
{
    public int objectiveID;
    public bool IsCompleted = false;
    private Renderer targetRenderer;

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    // This is the function called by the PlayerInteractor script
    public void Interact()
    {
        
    }

    public void Capture()
    {
        IsCompleted = true;
        ObjectiveManager.Instance.CompleteObjective(this, objectiveID);
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public bool IsInView(Camera cam)
    {
        if (IsCompleted || targetRenderer == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, targetRenderer.bounds);
    }
}