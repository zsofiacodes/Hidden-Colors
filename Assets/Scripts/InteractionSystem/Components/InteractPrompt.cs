using TMPro;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private string keyHint = "E";

    void Awake()
    {
        Hide();
    }

    public void Show(IInteractable interactable)
    {
        label.text = $"{keyHint}{interactable.DisplayName}";
        label.gameObject.SetActive(true);
    }
    public void Hide()
    {
        label.
            .SetActive(false);
    }
}
