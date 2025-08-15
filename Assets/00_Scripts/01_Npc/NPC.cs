using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public Canvas canvas;
    public GameObject player;
    public float rotationSpeed;

    QuestUI questUI;
    DialogueUI dialogueUI;
    NPCData npcData;
    
    private void Awake()
    {
        npcData = GetComponent<NPCData>();
    }
    public void Interact()
    {
        // NPC 대화창 생성
        questUI = FindObjectOfType<QuestUI>();
        //questUI.ShowWindow(questUI.npcWindow, true);
        UIManager.Instance.OpenUI(UIKey.DialogueUI);

        // 대사 전달
        dialogueUI = questUI.npcWindow.GetComponent<DialogueUI>();
        dialogueUI.quest = npcData.quest;

        // 플레이어 돌아보기
        LookAtPlayer();

        // 퀘스트 완료 후 
         if (npcData.quest.status == QuestStatus.Completed)
        {
            // 대화 시작
            dialogueUI.StartDialogue(npcData.dialogue1);
            return;
        }
        // 대화 시작
        dialogueUI.StartDialogue(npcData.dialogue);
       
    } 
    
    // 플레이어 돌아보기
    void LookAtPlayer()
    {
        Vector3 pos = (player.transform.position-transform.position);
        if (pos != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), rotationSpeed * Time.deltaTime);
        }
    }
}
