using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private List<Objective> allObjectives = new();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        cameraManager.OnPhotoSuccesfullTaken += AddCompeltedObjective;

    }

    private void OnDisable()
    {
        cameraManager.OnPhotoSuccesfullTaken -= AddCompeltedObjective;
    }

    public void AddCompeltedObjective(int id)
    {
        Objective obj = allObjectives.FirstOrDefault(o => o.objectiveID == id);
        if (obj != null && !obj.IsCompleted())
        {
            obj.Capture();
            AllCompletedCheck();
        }
    }

    public void AllCompletedCheck()
    {
        if (allObjectives.All(obj => obj.IsCompleted()))
        {
            GameManager.Instance.SetState(GameState.Outro);
        }
    }
}