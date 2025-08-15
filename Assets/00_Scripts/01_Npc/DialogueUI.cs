using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : BaseUI
{
    protected override UIKey uiKey => UIKey.DialogueUI;
    
    NPCInteractor interactor;
    QuestManager questManager;
    QuestUI questUI;

    TMP_Text dialogueText;

    [HideInInspector] public Quest quest;
    List<string> currentSentences;

   [SerializeField] private float typingSpeed = 0.05f; 
   [SerializeField] private int currentLine =0 ;
    private bool isTyping = false;

    private void OnEnable()
    {
        interactor = FindObjectOfType<NPCInteractor>();
        questManager = FindObjectOfType<QuestManager>();
        questUI = FindObjectOfType<QuestUI>();
    }
    // 대화 시작
    public void StartDialogue(List<string> dialogueLines)
    {
        // 초기화
        dialogueText = transform.GetChild(0).GetComponent<TMP_Text>();
        currentSentences = dialogueLines;
        isTyping = false;
        currentLine = 0;

        // 대사 출력
        OnDialogue();
    }
    // 대사 출력
    public void OnDialogue()
    {
        if (currentSentences == null) { return; }
        if (!isTyping && currentLine < currentSentences.Count)
        {
            StartCoroutine(PrintSentence(currentSentences[currentLine]));    
        }
        // 마지막 문장이라면
        else if (currentLine == currentSentences.Count )
        {
            questManager.curQuest = quest;
            questManager.AcceptQuest(); // 퀘스트 수락
            //questUI.ShowWindow(questUI.npcWindow,false); // NPC 대화창 닫기
            UIManager.Instance.CloseUI();
            interactor.isTaking = false;
            CharacterManager.Instance.Player.controller.ToggleCursor(false); // 시점 고정 풀기
        }
    }
    // 한글자씩 글쓰기 효과
    private IEnumerator PrintSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
           // Debug.Log(letter);
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        currentLine++;
    }
}
