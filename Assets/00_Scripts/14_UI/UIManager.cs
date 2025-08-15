using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIKey
{
    SoundUI,
    QuestListWindow,
    RewardWindow,
    InventoryUI,
    BuildSystemPanel,
    DialogueUI
}


public class UIManager : Singleton<UIManager>
{
    private Stack<UIKey> uiStack;
    private Dictionary<UIKey, BaseUI> uiDictionary;
    public Dictionary<UIKey, BaseUI> UIDictionary { get => uiDictionary; }
    public Stack<UIKey> UIStack { get => uiStack; }

    public Action OnClose;
    
    protected override void Initialize()
    {
        uiStack = new Stack<UIKey>();
        uiDictionary = new Dictionary<UIKey, BaseUI>();
    }

    public void SetUI(UIKey uiKey, BaseUI ui)
    {
        uiDictionary.Add(uiKey, ui);
    }

    public void OpenUI(UIKey uiKey)
    {
        if (uiStack.Count > 0)
        {
            CloseUI();
        }

        uiStack.Push(uiKey);
        uiDictionary[uiKey].SetUIActive(true);
    }

    
    // 현재 UI 비활성화
    public void CloseUI()
    {
        if (uiStack.Count == 0) return;
        
        OnClose?.Invoke();
        uiDictionary[uiStack.Peek()].SetUIActive(false);
        uiStack.Pop();
    }
    
    // 현재 UI 비활성화 후 다음 UI 활성화
    public void ChangeUI(UIKey uiKey)
    {
        CloseUI();
        OpenUI(uiKey);
    }
}