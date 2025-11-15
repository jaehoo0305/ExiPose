using UnityEngine;
using UnityEngine.SceneManagement;

public class TrailerToRooftop : MonoBehaviour
{
    public string rooftopSceneName = "Rooftop"; // 실제 씬 이름과 같게

    public void GoToRooftop()
    {
        if (GameFlow.Instance != null)
            GameFlow.Instance.startRooftopIntro = true;

        SceneManager.LoadScene(rooftopSceneName);
    }
}
