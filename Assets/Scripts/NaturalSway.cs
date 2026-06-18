using UnityEngine;

public class NaturalSway : MonoBehaviour
{
    public float speed = 1.0f;
    public float swayAmount = 1.0f; // Degrees to sway

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // Use Sin to create a smooth, gentle rocking motion
        float angle = Mathf.Sin(Time.time * speed) * swayAmount;
        transform.localRotation = startRotation * Quaternion.Euler(angle, 0, angle * 0.5f);
    }
}