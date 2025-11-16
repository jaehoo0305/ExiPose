using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToGameScene : MonoBehaviour
{
    [SerializeField] private string sceneName = "GameScene";

    public void GoToGameScene()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene(sceneName);   // 로딩 화면 있는 버전
        }
        else
        {
            SceneManager.LoadScene(sceneName);           // 혹시 SceneLoader 없을 때 대비
        }
    }
}
