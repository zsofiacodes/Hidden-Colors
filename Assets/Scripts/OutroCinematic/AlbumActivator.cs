using UnityEngine;

public class AlbumActivator : MonoBehaviour
{
    void OnEnable()
    {
        // This runs automatically the moment the AlbumUI appears!
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnDisable()
    {
        // Optional: lock it again if the album closes
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}