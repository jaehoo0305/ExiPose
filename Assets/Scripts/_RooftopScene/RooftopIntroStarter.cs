using System.Collections;
using UnityEngine;

public class RooftopIntroStarter : MonoBehaviour
{
    public DialogueData introDialogue; // 옥상 첫 대화 ScriptableObject

    private IEnumerator Start()
    {
        // 씬이 완전히 전환될 때까지 한 프레임 대기
        yield return null;

        Debug.Log("[RooftopIntroStarter] Start, GameFlow=" + GameFlow.Instance);

        if (GameFlow.Instance != null && GameFlow.Instance.startRooftopIntro)
        {
            Debug.Log("[RooftopIntroStarter] Flag is TRUE, start intro");
            GameFlow.Instance.startRooftopIntro = false; // 플래그 한 번 쓰고 끄기

            if (DialogueManager.Instance != null && introDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(introDialogue);
            }
            else
            {
                Debug.LogWarning("[RooftopIntroStarter] DialogueManager or introDialogue is null");
            }
        }
        else
        {
            Debug.Log("[RooftopIntroStarter] Flag is FALSE or GameFlow is null. No intro.");
        }
    }
}
