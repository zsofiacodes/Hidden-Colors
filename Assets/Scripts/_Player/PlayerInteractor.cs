using System;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform rayStartPoint;

    private OutlineManager focusedInteractable;

    public Action<bool> OnWithinInteractRange;
    public Action<Objective> OnUpdateActiveObjective;

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        if (GameManager.Instance.currentState == GameState.Tutorial)
            return;

        Vector3 rayStart = rayStartPoint.position + (rayStartPoint.forward * 0.1f);
        Ray ray = new Ray(rayStart, rayStartPoint.forward);
        Debug.DrawRay(rayStart, rayStartPoint.forward * interactRange, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out OutlineManager target))
            {
                if (target.GetComponent<Objective>().isCompleted)
                {
                    if (focusedInteractable != null)
                    {
                        focusedInteractable.OnLoseFocus();
                        focusedInteractable = null;
                    }
                        
                    OnWithinInteractRange?.Invoke(false);
                    return;
                }

                if (target != focusedInteractable)
                {
                    if (focusedInteractable != null) 
                        focusedInteractable.OnLoseFocus();

                    focusedInteractable = target;
                    focusedInteractable.OnGainFocus();

                    OnWithinInteractRange?.Invoke(true);
                    OnUpdateActiveObjective?.Invoke(target.GetComponent<Objective>());
                }

                return;
            }
        }

        if (focusedInteractable != null)
        {
            focusedInteractable.OnLoseFocus();
            focusedInteractable = null;
            OnWithinInteractRange?.Invoke(false);
        }
    }
}