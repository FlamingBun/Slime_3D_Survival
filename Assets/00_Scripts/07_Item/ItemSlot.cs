using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("UI")]
    public Image icon;                  // 아이템 아이콘 이미지
    public TextMeshProUGUI amountText; // 아이템 수량 텍스트

    private ItemData currentItem;      // 현재 슬롯에 설정된 아이템
    private int currentAmount;         // 현재 아이템의 수량

    // 아이템 정보를 슬롯에 설정
    public void Set(ItemData item, int amount)
    {
        currentItem = item;
        currentAmount = amount;

        if (item != null)
        {
            icon.sprite = item.icon;                      // 아이콘 이미지 설정
            icon.gameObject.SetActive(true);              // 아이콘 표시
            amountText.text = (item.itemType == ItemType.Equipment) ? "" : amount.ToString(); // 장비면 수량 표시 X

            Debug.Log($"[ItemSlot] 아이템 슬롯에 {item.itemName} x{amount} 세팅됨");
            Debug.Log($"[ItemSlot] 아이템 슬롯에 {item.itemName} x{amount} 세팅됨 / 아이콘 있음? {item.icon != null}");
        }
        else
        {
            Clear(); // 아이템이 null이면 슬롯 초기화
        }
    }

    // 슬롯 비우기
    public void Clear()
    {
        currentItem = null;
        currentAmount = 0;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        amountText.text = "";
    }

    // 슬롯에 해당 아이템이 있는지 확인
    public bool HasItem(ItemData item)
    {
        return currentItem == item;
    }

    // 슬롯이 비어있는지 확인
    public bool IsEmpty()
    {
        return currentItem == null;
    }

    // 슬롯 클릭 시 실행 (필요 시 장착 등 추가 동작 구현 가능)
    public void OnClick()
    {
        if (currentItem == null) return;

        Debug.Log($"슬롯 클릭됨: {currentItem.itemName}");

        if (currentItem.itemType == ItemType.Equipment)
        {
            Debug.Log("장비 아이템입니다. (장착 구현 가능)");
        }
    }
}
