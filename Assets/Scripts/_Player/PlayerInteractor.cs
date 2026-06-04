using System;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform rayStartPoint;

    private OutlineManager focusedInteractable;

    public Action OnWithinInteractRange;

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Vector3 rayStart = rayStartPoint.position + (rayStartPoint.forward * 0.1f);
        Ray ray = new Ray(rayStart, rayStartPoint.forward);
        Debug.DrawRay(rayStart, rayStartPoint.forward * interactRange, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out OutlineManager target))
            {
                if (target != focusedInteractable)
                {
                    if (focusedInteractable != null) focusedInteractable.OnLoseFocus();
                    focusedInteractable = target;
                    focusedInteractable.OnGainFocus();
                    OnWithinInteractRange?.Invoke();
                }

                return;
            }
        }

        if (focusedInteractable != null)
        {
            focusedInteractable.OnLoseFocus();
            focusedInteractable = null;
        }
    }
}