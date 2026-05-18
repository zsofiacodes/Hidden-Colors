using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [SerializeField] private List<Objective> allObjectives = new();

    private void Awake()
    {
        Instance = this;
    }

    public void CompleteObjective(Objective objective, int id)
    {
        if (allObjectives.Contains(objective))
        {
            if (objective.IsCompleted) 
                return;

            objective.Complete();

            if (allObjectives.TrueForAll(obj => obj.IsCompleted))
            {
                Debug.Log("All objectives completed!");
                GameManager.Instance.SetState(GameState.Outro);
            }
        }
    }
}