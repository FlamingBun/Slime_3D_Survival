using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public string npcName;
    public List<string> dialogue; // 퀘스트 받기 전 대사
    public List<string> dialogue1; // 퀘스트 완료 후 대사
    public Quest quest;
}
