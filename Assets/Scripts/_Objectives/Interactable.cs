using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    private MonoBehaviour outline; // Using MonoBehaviour to be extra safe

    void Start()
    {
        // Try to find an Outline script, but don't crash if it's missing
        outline = GetComponent("Outline") as MonoBehaviour;
        if (outline != null) outline.enabled = false;
    }

    public void OnGainFocus()
    {
        if (outline != null) outline.enabled = true;
    }

    public void OnLoseFocus()
    {
        if (outline != null) outline.enabled = false;
    }

    public void Interact()
    {
        onInteract?.Invoke();
    }
}