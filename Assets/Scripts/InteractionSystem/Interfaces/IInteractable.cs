using UnityEngine;

public interface IInteractable 
{
  Transform Transform { get; }

    string DisplayName { get; }

    bool CanInteract ();

    void Interact ();

    void OnFocusGained ();

    void OnFocusLost ();
}
