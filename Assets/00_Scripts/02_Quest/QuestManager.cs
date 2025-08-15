using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class QuestManager : MonoBehaviour
{
    public Quest curQuest;
    QuestUI questUI;

    public List<Quest> questData = new List<Quest>(); // 전체 퀘스트 데이터 목록
    public List<Quest> allQuests = new List<Quest>(); // 수락한 퀘스트 목록
    public List<ItemData> itemData = new List<ItemData>(); // 퀘스트 보상 목록
  
    private void Start()
    {
        questUI = GetComponent<QuestUI>();
    }
    private void Update()
    {
        AllQuestClear();
    }
    // 퀘스트 수락 확인
    public void AcceptQuest()
    {
        if (curQuest.status == QuestStatus.NotAccepted)
        {
            curQuest.status = QuestStatus.Accepted;
            allQuests.Add(curQuest);
            questUI.ShowQuestProgress();  
        }
     }
    // 퀘스트 완료 확인 
    public void CompleteQuest(int _questId)
    {
        if (curQuest.status == QuestStatus.Accepted
            && curQuest.curProgress == curQuest.totalProgress)
        {
            // 퀘스트 상태를 완료로 변경하고
            curQuest.status = QuestStatus.Completed;

            // 퀘스트 보상
            RewardQuest(_questId,3);

            // 수락한 퀘스트 목록에서 해당 퀘스트 제거
            DeleteQuest(_questId);
            // 화면 우측 퀘스트 UI 삭제
            questUI.DeleteQuestProgress(_questId);
        }
    }
    // 전체 퀘스트 완료 확인
    void AllQuestClear()
    {
        foreach (Quest quest in questData)
        {
            if (quest.status != QuestStatus.Completed)
            {
                return;
            }
        }
        SceneManager.LoadScene("EndingScene");
        return;
    }
    // 퀘스트 진행도 증가
    public void AddProgress(int _questId,int amount )
    {
        curQuest = null;
        curQuest = FindQuest(_questId);

        if (curQuest == null) {return;}

        if (CheckProgress())
        {
            curQuest.curProgress += amount;

            // 퀘스트 완료 확인 
            if (curQuest.curProgress == curQuest.totalProgress)
            {
                CompleteQuest(_questId);
            }
        }
    }
    // 진행도가 올라갈 수 있는지
    public bool CheckProgress()
    {
        return curQuest.curProgress < curQuest.totalProgress ? true : false;
    }
    // 수락한 퀘스트 목록에서 찾고자하는 퀘스트 반환하기
    public Quest FindQuest(int _questId)
    {
        return allQuests.Find(i => i.questId == _questId);
    }
    // 수락한 퀘스트 목록에서 해당 퀘스트 제거
    public void DeleteQuest(int _questId)
    {
         allQuests.RemoveAll(i => i.questId == _questId);
        
    }
    // 퀘스트 보상 주기
    public void RewardQuest(int _questId, int itemCount)
    {
        // 퀘스트 보상 UI 띄우기
        UIManager.Instance.OpenUI(UIKey.RewardWindow);
        RewardWindow rewardWindow = questUI.RewardWindow.GetComponent<RewardWindow>();

        // 보상 아이템 데이터 전달
        rewardWindow.quest = FindQuest(_questId);
        rewardWindow.rewardItemCount = itemCount;
        rewardWindow.ShowReward(); 

        // 인벤토리에 보상 지급
        InventoryManager.Instance.AddItem(itemData[_questId], 3);

        // 퀘스트 완료 확인
        AllQuestClear();
    }
}
