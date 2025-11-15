using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }

    // Rooftop 씬 들어갈 때 자동 대화 시작 플래그
    public bool startRooftopIntro;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
    }
}
