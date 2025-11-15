using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;       // 화자 이름 (예: Player, 동료 이름)
    [TextArea]
    public string text;          // 실제 대사
}

[CreateAssetMenu(menuName = "ExiPose/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string dialogueId;    // "Rooftop_Ally_Intro" 같은 ID
    public DialogueLine[] lines; // 한 줄씩 대사
}
