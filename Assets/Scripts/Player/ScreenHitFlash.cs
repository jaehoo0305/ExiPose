using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHitFlash : MonoBehaviour
{
    public Image flashImage;
    public float maxAlpha = 0.4f;
    public float fadeDuration = 0.25f;

    Coroutine currentRoutine;

    public void OnDamaged(int damage)
    {
        if (flashImage == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Color c = flashImage.color;
        // Set max alpha
        c.a = maxAlpha;
        flashImage.color = c;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(maxAlpha, 0f, t / fadeDuration);
            c.a = a;
            flashImage.color = c;
            yield return null;
        }

        c.a = 0f;
        flashImage.color = c;
        currentRoutine = null;
    }
}
