using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // 싱글턴 인스턴스. 어디서든 InventoryManager.Instance로 접근 가능
    public static InventoryManager Instance;

    // 아이템 데이터와 수량을 저장하는 딕셔너리
    private Dictionary<ItemData, int> itemDictionary = new();

    private void Awake()
    {
        // 싱글턴 설정
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[InventoryManager] 싱글턴 인스턴스 설정 완료");
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    // 아이템을 인벤토리에 추가하는 함수
    public void AddItem(ItemData item, int amount)
    {
        if (item == null)
        {
            Debug.LogError("[AddItem] item이 null입니다!");
            return;
        }

        Debug.Log($"[AddItem] {item.itemName} x{amount} 추가");

        // 장비 아이템은 고유 복사본을 여러 개 추가
        if (item.itemType == ItemType.Equipment)
        {
            for (int i = 0; i < amount; i++)
            {
                itemDictionary.Add(GetUniqueItemCopy(item), 1);
            }
        }
        else
        {
            // 이미 있는 아이템이면 수량 증가, 없으면 새로 추가
            if (itemDictionary.ContainsKey(item))
                itemDictionary[item] += amount;
            else
                itemDictionary.Add(item, amount);

            // 자원 채집 퀘스트 진행도 1 증가 (퀘스트 시스템 연동)
            FindObjectOfType<QuestManager>().AddProgress(0, 1);
        }

        // UI 갱신
        InventoryUI.Instance?.RefreshUI();
    }

    // 장비 아이템용 고유 복제본 생성
    private ItemData GetUniqueItemCopy(ItemData original)
    {
        ItemData copy = ScriptableObject.Instantiate(original); // 원본 복사
        copy.name = original.name + "_" + System.Guid.NewGuid().ToString(); // 고유 이름 부여
        return copy;
    }

    // 특정 아이템의 현재 수량을 반환하는 함수
    public int GetItemCount(ItemData item)
    {
        if (itemDictionary.TryGetValue(item, out int count))
            return count;
        return 0;
    }

    // 아이템을 지정된 수량만큼 제거하는 함수
    public bool RemoveItem(ItemData item, int amount)
    {
        // 아이템이 없거나 수량이 부족하면 실패
        if (!itemDictionary.ContainsKey(item) || itemDictionary[item] < amount)
            return false;

        itemDictionary[item] -= amount;

        // 수량이 0 이하가 되면 딕셔너리에서 제거
        if (itemDictionary[item] <= 0)
            itemDictionary.Remove(item);

        // UI 갱신
        InventoryUI.Instance?.RefreshUI();
        return true;
    }

    // 전체 아이템 정보를 반환 (외부에서 조회용으로 사용)
    public Dictionary<ItemData, int> GetAllItems()
    {
        return itemDictionary;
    }
}
