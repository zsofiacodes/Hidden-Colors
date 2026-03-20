using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [System.Serializable]
    public class ObjectiveData
    {
        public string id;           // A unique name like "CubeGoal"
        public string description;  // The text like "• photo one"
        public TextMeshProUGUI uiText; // Drag the UI object here
        public bool completed;
    }

    public List<ObjectiveData> objectives = new List<ObjectiveData>();

    private void Awake()
    {
        Instance = this;
    }

    public void CheckPhoto(string targetID)
    {
        foreach (var obj in objectives)
        {
            if (obj.id == targetID && !obj.completed)
            {
                obj.completed = true;
                // Wraps the text in <s> </s> for strikethrough effect
                obj.uiText.text = "<s>" + obj.description + "</s>";
                obj.uiText.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Fade it out
                Debug.Log("Crossed off: " + obj.id);
            }
        }
    }
}