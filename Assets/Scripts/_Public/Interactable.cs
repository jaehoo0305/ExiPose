using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [TextArea]
    public string message = "";

    public UnityEvent onInteract;

    bool playerInRange;
    bool promptVisible;   // 지금 E UI가 떠 있는지 추적

    void Awake()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;   // 이 콜라이더는 상호작용 범위용
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        ShowPrompt();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        HidePrompt();
    }

    void Update()
    {
        if (!playerInRange)
            return;

        //  대화 중일 때: E UI 숨기고, E 입력은 무시
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsActive)
        {
            if (promptVisible)
                HidePrompt();

            return;
        }

        //  대화가 끝났고, 아직 범위 안인데 UI가 안 떠 있으면 다시 켜주기
        if (!promptVisible)
            ShowPrompt();

        // 여기서부터는 "대화 안 하는 상태 + 범위 안"일 때만 E 처리
        if (Input.GetKeyDown(KeyCode.E))
        {
            // E UI는 계속 떠 있어도 상관 없으면 HidePrompt() 안 해도 됨
            // 깔끔하게 숨기고 싶다면 아래 주석을 풀어도 됨:
            // HidePrompt();

            onInteract.Invoke();
        }
    }

    void OnDisable()
    {
        if (promptVisible && InteractionUI.Instance != null)
        {
            InteractionUI.Instance.Hide();
            promptVisible = false;
        }

        playerInRange = false;
    }

    void ShowPrompt()
    {
        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.Show(message);
            promptVisible = true;
        }
    }

    void HidePrompt()
    {
        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.Hide();
            promptVisible = false;
        }
    }
}
