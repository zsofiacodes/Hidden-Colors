using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private UnityEvent onInteract;

    private PhotoTarget photoTarget; // Reference to our target script
    private Outline outline;

    public string DisplayName => displayName;

    // This now checks if the object was already photographed!
    public bool CanInteract()
    {
        if (photoTarget != null && photoTarget.isCaptured) return false;
        return isEnabled;
    }

    private void Awake()
    {
        photoTarget = GetComponent<PhotoTarget>();
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 3f;
        outline.enabled = false;
    }

    public void Interact()
    {
        onInteract?.Invoke();
    }

    public void OnFocusGained()
    {
        if (CanInteract()) outline.enabled = true;
    }

    public void OnFocusLost()
    {
        outline.enabled = false;
    }

    public Transform Transform => transform;
}