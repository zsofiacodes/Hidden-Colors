using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Outline outline;
    public UnityEvent onInteract;

    void Start()
    {
        outline.enabled = false;
    }

    public void OnGainFocus()
    {
        outline.enabled = true;
    }

    public void OnLoseFocus()
    {
        outline.enabled = false;
    }

    public void Interact()
    {
        onInteract?.Invoke();
    }
}