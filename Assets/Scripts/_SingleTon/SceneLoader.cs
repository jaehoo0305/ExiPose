using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public GameObject loadingCanvas;
    public float minimumLoadTime = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (loadingCanvas != null)
            loadingCanvas.SetActive(true);

        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            timer += Time.deltaTime;

            // 0.9f = 거의 로드 완료된 상태
            if (op.progress >= 0.9f && timer >= minimumLoadTime)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return null;

        if (loadingCanvas != null)
            loadingCanvas.SetActive(false);
    }
}
