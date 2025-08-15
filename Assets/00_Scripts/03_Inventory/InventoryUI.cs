using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : BaseUI
{
    protected override UIKey uiKey => UIKey.InventoryUI;
    
    // 인벤토리 UI 싱글턴 인스턴스 (다른 스크립트에서 쉽게 접근 가능)
    public static InventoryUI Instance;

    // 인벤토리 슬롯 리스트 (UI에 표시될 슬롯 21개)
    public List<ItemSlot> slots;

    // 현재 선택된 탭 (자원, 장비, 소비 아이템 등)
    public InventoryTabType currentTab = InventoryTabType.Resource;

    // 인벤토리 창 패널 (활성/비활성 제어용)
    public GameObject inventoryWindow;

    private void Awake()
    {
        // 싱글턴 초기화
        if (Instance == null)
            Instance = this;
    }

    // 탭 클릭 시 호출되는 메서드
    public void OnClickTab(int tabIndex)
    {
        // 선택한 탭으로 현재 탭 갱신
        currentTab = (InventoryTabType)tabIndex;
        Debug.Log($"[UI] 현재 탭: {currentTab}");

        // UI 갱신
        RefreshUI();
    }

    // 인벤토리 슬롯 UI를 현재 탭 기준으로 새로 그리는 메서드
    public void RefreshUI()
    {
        // 모든 아이템 딕셔너리를 가져옴
        Dictionary<ItemData, int> allItems = InventoryManager.Instance.GetAllItems();

        // 현재 탭에 맞는 아이템만 필터링해서 리스트에 저장
        List<ItemDataAmount> filtered = new();

        foreach (var pair in allItems)
        {
            Debug.Log($"[UI 필터링] {pair.Key.itemName} | type: {pair.Key.itemType} | tab: {currentTab} | count: {pair.Value}");
            Debug.Log($"[RefreshUI] 전체 아이템: {pair.Key.itemName}, 타입: {pair.Key.itemType}, 수량: {pair.Value}");

            // 아이템 타입이 현재 탭과 일치하면 필터링 리스트에 추가
            if ((int)pair.Key.itemType == (int)currentTab)
            {
                filtered.Add(new ItemDataAmount
                {
                    item = pair.Key,
                    amount = pair.Value
                });
            }
        }

        // 슬롯에 필터링된 아이템 정보를 채워줌
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < filtered.Count)
                slots[i].Set(filtered[i].item, filtered[i].amount); // 아이템 세팅
            else
                slots[i].Clear(); // 빈 슬롯 처리
        }
    }

    // 인벤토리 창 토글 (열기/닫기)
    // public void Toggle()
    // {
    //     if (inventoryWindow.activeSelf)
    //     {
    //         UIManager.Instance.CloseUI();       
    //     }
    //     else
    //     {
    //         UIManager.Instance.OpenUI(UIKey.InventoryUI);
    //     }
    // }
}

// 슬롯에 표시할 아이템 정보 구조체
[System.Serializable]
public class ItemDataAmount
{
    public ItemData item;  // 아이템 정보 (아이콘, 이름 등)
    public int amount;     // 아이템 수량
}
