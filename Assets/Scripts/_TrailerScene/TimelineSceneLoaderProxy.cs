using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineSceneLoaderProxy : MonoBehaviour
{
    [SerializeField] private string sceneName = "RooftopScene";
    [SerializeField] private GameObject loadingCanvas;

    // 타임라인 Signal Receiver에서 이 함수를 호출 중
    public void GoToRooftop()
    {
        if (GameFlow.Instance != null)
            GameFlow.Instance.startRooftopIntro = true;

        Debug.Log("[TimelineSceneLoaderProxy] GoToRooftop called");
        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        if (loadingCanvas != null)
        {
            Debug.Log("[TimelineSceneLoaderProxy] Enable LoadingCanvas");
            loadingCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[TimelineSceneLoaderProxy] loadingCanvas is NULL");
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 로딩 진행 체크
        while (op.progress < 0.9f)
        {
            //Debug.Log($"[TimelineSceneLoaderProxy] Loading... {op.progress}");
            yield return null;
        }

        // 살짝 연출 시간
        yield return new WaitForSeconds(0.3f);

        Debug.Log("[TimelineSceneLoaderProxy] Activate next scene");
        op.allowSceneActivation = true;
    }
}
