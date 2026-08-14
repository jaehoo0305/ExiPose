using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Loading UI")]
    public GameObject loadingCanvas;
    public float minimumLoadTime = 1f;


    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            FindLoadingCanvas();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindLoadingCanvas();
    }

    private void FindLoadingCanvas()
    {
        if (loadingCanvas != null) return;

        // 1) Tag search
        var tagged = GameObject.FindWithTag("LoadingCanvas");
        if (tagged != null)
        {
            loadingCanvas = tagged;
            return;
        }

        // 2) Name search
        var byName = GameObject.Find("LoadingCanvas");
        if (byName != null)
        {
            loadingCanvas = byName;
            return;
        }

        // 3) Any Canvas that contains "loading" in its name (include inactive)
#if UNITY_2023_1_OR_NEWER
        var canvases = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
        var canvases = Object.FindObjectsOfType<Canvas>(true);
#endif
        foreach (var c in canvases)
        {
            if (c != null && c.name.ToLower().Contains("loading"))
            {
                loadingCanvas = c.gameObject;
                return;
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        FindLoadingCanvas();

        if (loadingCanvas != null)
            loadingCanvas.SetActive(true);

        yield return new WaitForSeconds(minimumLoadTime);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }

        FindLoadingCanvas();

        if (loadingCanvas != null)
            loadingCanvas.SetActive(false);
    }
}
