using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QuestListWindow : BaseUI
{
    protected override UIKey uiKey => UIKey.QuestListWindow;
    
    QuestManager questManager;

    public List<Image> allQuestsToggle = new List<Image>();
    public List<TextMeshProUGUI> allQuestsText = new List<TextMeshProUGUI>();
 
    void OnEnable()
    {
        questManager = FindObjectOfType<QuestManager>();
        SetQuestInfo();
    }

    // 퀘스트 정보 세팅
    void SetQuestInfo()
    {
       for(int i = 0; i < questManager.questData.Count; i++) 
        {
            // 퀘스트 텍스트 표시
            allQuestsText[i].text = questManager.questData[i].title + " : " + questManager.questData[i].description + "\n";

            Color color = allQuestsText[i].color; 

            // 완료한 퀘스트라면
            if (questManager.questData[i].status == QuestStatus.Completed)
            {
                // 확인 표시
                color.a = 1f;
                allQuestsToggle[i].enabled = true;
            }
            else
            {
                // 확인 표시 안함
                color.a = 0.5f;
                allQuestsToggle[i].enabled = false;
            }
            // 글자 색상 변경
            allQuestsText[i].color = color;
        }
    }
}
