using UnityEngine;

public class LeafFlutter : MonoBehaviour
{
    public float speed = 1.0f;
    public float intensity = 0.05f;

    void Update()
    {
        // Random "wind" movement using Perlin Noise
        float x = Mathf.PerlinNoise(Time.time * speed, 0) - 0.5f;
        float z = Mathf.PerlinNoise(0, Time.time * speed) - 0.5f;

        transform.localPosition += new Vector3(x, 0, z) * intensity;
    }
}