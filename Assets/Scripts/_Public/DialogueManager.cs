using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    public GameObject panel;          // DialoguePanel
    public TextMeshProUGUI nameText;  // 화자 이름
    public TextMeshProUGUI bodyText;  // 대사 본문

    [Header("Typing")]
    public float typeSpeed = 0.03f;   // 글자 한 개 나오는 속도 (초)

    private DialogueData currentData;
    private int currentIndex;
    private bool isActive;

    private string currentSpeaker;
    public string CurrentSpeaker => isActive ? currentSpeaker : null;

    // 외부에서 "대화 중인가?" 체크할 때 사용 (플레이어 이동/공격 막기 용)
    public bool IsActive => isActive;

    // 타자기 효과 관련
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentFullText;

    // (옵션) 로그
    private List<string> log = new List<string>();

    void Awake()
    {
        Instance = this;
        if (panel != null)
            panel.SetActive(false);
    }

    public void StartDialogue(DialogueData data)
    {
        if (data == null) return;

        currentData = data;
        currentIndex = 0;
        isActive = true;

        if (panel != null)
            panel.SetActive(true);

        ShowCurrentLine();
    }

    void Update()
    {
        if (!isActive) return;

        // 스페이스 or 마우스 왼쪽 클릭
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 1) 아직 글자 나오는 중이면 → 전부 표시하고 타자기 중단
            if (isTyping)
            {
                FinishTypingInstant();
            }
            // 2) 이미 다 나온 상태면 → 다음 줄로
            else
            {
                NextLine();
            }
        }
    }

    void ShowCurrentLine()
    {
        // 끝났으면 종료
        if (currentData == null || currentIndex >= currentData.lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentData.lines[currentIndex];

        currentSpeaker = line.speaker;

        if (nameText != null)
            nameText.text = line.speaker;

        currentFullText = line.text;

        // 로그에 저장(옵션)
        log.Add($"{line.speaker}: {line.text}");

        // 이전 코루틴 돌고 있으면 정리
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // 타자기 시작
        typingCoroutine = StartCoroutine(TypeLine(currentFullText));
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;
        if (bodyText != null)
            bodyText.text = "";

        foreach (char c in text)
        {
            if (bodyText != null)
                bodyText.text += c;

            // 글자 하나 찍고 기다리기
            yield return new WaitForSeconds(typeSpeed);

            // 중간에 스킵으로 isTyping이 false 되면 멈춤
            if (!isTyping)
                yield break;
        }

        // 전부 출력 완료
        isTyping = false;
    }

    void FinishTypingInstant()
    {
        // 코루틴 중단 + 전체 텍스트 바로 표시
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (bodyText != null)
            bodyText.text = currentFullText;
    }

    void NextLine()
    {
        currentIndex++;
        ShowCurrentLine();
    }

    void EndDialogue()
    {
        isActive = false;

        if (panel != null)
            panel.SetActive(false);

        currentData = null;

        // 타자기 관련 상태 정리
        isTyping = false;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // 필요하면 여기에서 log를 파일로 저장하거나, 플래그 세팅 가능
        // SaveLog();
    }
}
