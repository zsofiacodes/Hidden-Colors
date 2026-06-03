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

    public void Interact()
    {
        //When the user presses E, then you do this (bring camera up)
    }

    public void Capture()
    {
        //When the user presses C, then you do this (make the picture)
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