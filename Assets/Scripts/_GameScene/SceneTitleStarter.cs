using UnityEngine;

public class SceneTitleStarter : MonoBehaviour
{
    public SceneTitleUI sceneTitlePrefab;
    public string titleMessage = "50th Floor - Office";
    public bool playOnStart = true;

    void Start()
    {
        if (!playOnStart) return;
        if (sceneTitlePrefab == null)
        {
            Debug.LogWarning("[SceneTitleStarter] SceneTitlePrefab is null");
            return;
        }

        // find canvas
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        SceneTitleUI instance;

        if (canvas != null)
        {
            instance = Instantiate(sceneTitlePrefab, canvas.transform, false);
        }
        else
        {
            // fallback: no canvas, spawn as root
            instance = Instantiate(sceneTitlePrefab);
        }

        instance.ShowTitle(titleMessage);
    }
}
