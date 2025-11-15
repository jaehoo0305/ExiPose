using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DialogueTalkAnimator : MonoBehaviour
{
    private Animator anim;

    [Header("Speker")]
    public string speakerName = "Player";   // 예: "Player", "Ally", "???"

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (DialogueManager.Instance == null)
            return;

        bool talking = false;

        if (DialogueManager.Instance.IsActive)
        {
            // 현재 대화 중이고, 현재 화자 이름과 내 이름이 같을 때만 talking = true
            string currentSpeaker = DialogueManager.Instance.CurrentSpeaker;
            if (!string.IsNullOrEmpty(currentSpeaker) &&
                currentSpeaker == speakerName)
            {
                talking = true;
            }
        }

        anim.SetBool("IsTalking", talking);
    }
}
