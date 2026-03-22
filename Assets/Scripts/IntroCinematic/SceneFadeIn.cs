using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f; // How long the fade takes

    void Start()
    {
        // Set image to solid black at the very start
        fadeImage.color = new Color(0, 0, 0, 1);
        StartCoroutine(DoFadeIn());
    }

    IEnumerator DoFadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            // This 'SmoothStep' makes the fade feel more natural/organic
            float alpha = Mathf.SmoothStep(1, 0, progress);

            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}