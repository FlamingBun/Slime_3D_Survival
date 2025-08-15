using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus
{
    NotAccepted, // 미수락
    Accepted, // 수락
    Completed // 완료
}
[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest")]
public class Quest : ScriptableObject
{
    [Header("QuestInfo")]
    public int questId; // 몇번째 퀘스트인지
    public string title; // 제목
    public string description; // 설명
    public QuestStatus status = QuestStatus.NotAccepted;

    public ItemData rewardItem; // 보상 아이템
    public int rewardItemCount; // 보상 아이템 개수

    public int curProgress; // 현재 진행도
    public int totalProgress; // 총 진행도

    // 초기값
    QuestStatus init_status = QuestStatus.NotAccepted; // 초기 상태
     int init_curProgress = 0 ; // 초기 진행도


    public void Init()
    {
        status = init_status;
        curProgress = init_curProgress;
    }
}
