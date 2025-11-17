using System.Collections;
using UnityEngine;
using TMPro;

public class SceneTitleUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public float fadeInTime = 0.6f;
    public float stayTime = 1.2f;
    public float fadeOutTime = 0.6f;

    bool isPlaying;

    void Awake()
    {
        if (titleText == null)
            titleText = GetComponentInChildren<TextMeshProUGUI>();

        if (titleText != null)
        {
            Color c = titleText.color;
            c.a = 0f;
            titleText.color = c;
        }
    }

    public void ShowTitle(string message)
    {
        if (titleText == null) return;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        titleText.text = message;

        if (!isPlaying)
            StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        isPlaying = true;

        yield return Fade(0f, 1f, fadeInTime);
        if (stayTime > 0f)
            yield return new WaitForSeconds(stayTime);
        yield return Fade(1f, 0f, fadeOutTime);

        isPlaying = false;
        Destroy(gameObject);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        if (titleText == null) yield break;

        if (duration <= 0f)
        {
            Color c = titleText.color;
            c.a = to;
            titleText.color = c;
            yield break;
        }

        float t = 0f;
        Color color = titleText.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / duration);
            color.a = alpha;
            titleText.color = color;
            yield return null;
        }

        color.a = to;
        titleText.color = color;
    }
}
