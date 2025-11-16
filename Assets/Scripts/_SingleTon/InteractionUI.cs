using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; } //싱글톤

    public GameObject panel;          // InteractionPanel 전체
    public TextMeshProUGUI text;      // 안내 텍스트

    [SerializeField] private CanvasGroup canvasGroup;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string message)
    {
        if (panel != null)
            panel.SetActive(true);

        if (text != null)
            text.text = message;
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void HideNow()
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        // 필요하다면 내부 상태도 초기화
        // currentInteractable = null; 같은 것들
    }
}
