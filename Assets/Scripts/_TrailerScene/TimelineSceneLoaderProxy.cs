using UnityEngine;

public class TimelineSceneLoaderProxy : MonoBehaviour
{
    public void Load(string sceneName)
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(sceneName);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
