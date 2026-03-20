using Unity.Collections;
using UnityEngine;
// 1. Add this namespace to access the new Input System
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private InteractPrompt prompt;

    private Collider[] buffer = new Collider[32];
    private IInteractable focused;

    private void Update()
    {
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);

        // 2. Updated the input check to use the Keyboard class
        // This checks if the 'E' key was pressed this frame
        if (focused != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (focused.CanInteract()) focused.Interact();
        }
    }

    private IInteractable FindNearestInteractable()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, interactableLayer, QueryTriggerInteraction.Collide);
        IInteractable nearest = null;
        float bestDistSq = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider col = buffer[i];
            if (col == null) continue;
            IInteractable interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;
            if (!interactable.CanInteract()) continue;
            float distSq = (col.transform.position - transform.position).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                nearest = interactable;
            }
        }
        return nearest;
    }

    private void UpdateFocus(IInteractable nearest)
    {
        if (ReferenceEquals(focused, nearest)) return;
        focused?.OnFocusLost();
        focused = nearest;
        if (focused != null)
        {
            focused.OnFocusGained();
            prompt.Show(focused);
        }
        else
        {
            prompt.Hide();
        }
    }
}