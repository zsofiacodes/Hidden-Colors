using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("RayStarting point")]
    [SerializeField] private Transform rayStartPoint;

    [Header("UI Reference")]
    [SerializeField] private GameObject interactPrompt;

    private Interactable focusedInteractable;

    private void Start()
    {
        interactPrompt.SetActive(false);
    }

    private void Update()
    {
        CheckForInteractable();
    }

    public void ShowPrompt(bool value)
    {
        interactPrompt.SetActive(value);
    }

    private void CheckForInteractable()
    {
        // Start ray from camera, nudged forward 0.1 to avoid player body
        Vector3 rayStart = rayStartPoint.position + (rayStartPoint.forward * 0.1f);
        Ray ray = new Ray(rayStart, rayStartPoint.forward);
        Debug.DrawRay(rayStart, rayStartPoint.forward * interactRange, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            Interactable target = hit.collider.GetComponent<Interactable>();

            if (target != null)
            {
                ShowPrompt(true);

                if (target != focusedInteractable)
                {
                    if (focusedInteractable != null) focusedInteractable.OnLoseFocus();
                    focusedInteractable = target;
                    focusedInteractable.OnGainFocus();
                }
                
                return;
            }
            else
            {
                ShowPrompt(false);
            }
        }

        // If we hit nothing, clean up
        if (focusedInteractable != null)
        {
            focusedInteractable.OnLoseFocus();
            focusedInteractable = null;
        }

        if (interactPrompt.activeSelf)
        {
            interactPrompt.SetActive(false);
        }
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed && focusedInteractable != null)
        {
            Debug.Log("Interacting with: " + focusedInteractable.gameObject.name);
            focusedInteractable.Interact();

            // Clear focus immediately after interaction to prevent errors
            if (interactPrompt != null) interactPrompt.SetActive(false);
            focusedInteractable = null;
        }
    }
}