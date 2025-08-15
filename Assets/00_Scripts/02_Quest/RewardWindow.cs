using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RewardWindow : BaseUI
{
    protected override UIKey uiKey => UIKey.RewardWindow;
    
    public Image itemImage;
    public TMP_Text ContentText;
    [HideInInspector] public int rewardItemCount; // 보상으로 받은 아이템 개수

    [HideInInspector] public Quest quest;

    void OnEnable()
    {
       // ShowReward();
    }

    public void ShowReward()
    {
        itemImage.sprite = quest.rewardItem.icon;
        ContentText.text = quest.rewardItem.name + "를 " + quest.rewardItemCount + "개 획득하셨습니다.";
    }
}
