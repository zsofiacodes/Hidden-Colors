using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    private Objective objective;
    private Outline outline;

    private void Awake()
    {
        objective = GetComponent<Objective>();
        outline = GetComponent<Outline>();
    }

    void Start()
    {
        outline.enabled = false;
    }

    public void OnGainFocus()
    {
        if (objective.isCompleted)
            return;

        outline.enabled = true;
    }

    public void OnLoseFocus()
    {
        outline.enabled = false;
    }
}