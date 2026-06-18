using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlbumSlider : MonoBehaviour, IDragHandler
{
    [SerializeField] private Image sliderMask; // Drag your SliderMask object here

    // This allows the player to drag their mouse across the photo slot
    public void OnDrag(PointerEventData eventData)
    {
        // Calculate the position of the mouse relative to this object
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        // Get the width of the slot
        float width = GetComponent<RectTransform>().rect.width;

        // Calculate a 0 to 1 value based on where the mouse is
        float normalizedValue = (localPoint.x + (width / 2)) / width;

        // Clamp the value so it stays between 0 and 1
        normalizedValue = Mathf.Clamp01(normalizedValue);

        // Apply it to the mask's fillAmount
        if (sliderMask != null)
        {
            sliderMask.fillAmount = normalizedValue;
        }
    }
}