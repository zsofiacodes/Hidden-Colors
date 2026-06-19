using UnityEngine;
using UnityEngine.UI;

public class AlbumSlider : MonoBehaviour
{
    [SerializeField] private Image sliderMask;

    public void ShowAlbum()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Call this if you need to hide it again (e.g., if the animation resumes)
    public void HideAlbum()
    {
        // Only lock if you don't need the mouse for something else
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // The 'public' is what allows the Slider to call this function
    public void OnSliderChanged(float value)
    {
        Debug.Log("Slider moved to: " + value);
        if (sliderMask != null)
        {
            sliderMask.fillAmount = value;
        }
    }
}