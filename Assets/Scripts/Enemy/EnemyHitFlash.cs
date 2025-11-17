using System.Collections;
using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    public Renderer[] targetRenderers;
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;

    Color[] originalColors;
    bool initialized;

    void Awake()
    {
        if (targetRenderers == null || targetRenderers.Length == 0) return;

        originalColors = new Color[targetRenderers.Length];
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            // Use .material to instance material per enemy
            if (targetRenderers[i] != null)
            {
                originalColors[i] = targetRenderers[i].material.color;
            }
        }
        initialized = true;
    }

    public void OnDamaged(int damage)
    {
        if (!initialized) return;
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // Set hit color
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] != null)
                targetRenderers[i].material.color = hitColor;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore original color
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] != null)
                targetRenderers[i].material.color = originalColors[i];
        }
    }
}
