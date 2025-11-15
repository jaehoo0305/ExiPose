using UnityEngine;

public class NPCDialogueStarter : MonoBehaviour
{
    public DialogueData dialogue;  // 이 NPC가 재생할 대화

    public void StartDialogue()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.StartDialogue(dialogue);
    }
}
