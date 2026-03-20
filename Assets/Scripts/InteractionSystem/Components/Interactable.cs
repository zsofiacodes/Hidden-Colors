using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private UnityEvent onInteract;

    public string DisplayName => displayName;
    public bool CanInteract() => isEnabled;
    private Outline outline;
    private void Awake()
    {
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
       outline.enabled = true;
    }

    public void OnFocusLost()
    {
       outline.enabled = false;
    }

    public Transform Transform => transform;
}