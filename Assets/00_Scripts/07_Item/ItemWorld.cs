using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    private ItemData item;
    private int amount;
    private SpriteRenderer spriteRenderer;

    private bool canPickup = false;
    public float pickupDelay = 2.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("[ItemWorld] SpriteRenderer가 없습니다. 아이콘을 표시할 수 없습니다.");
        }
    }

    public void Set(ItemData newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;

        if (spriteRenderer != null && item.icon != null)
        {
            spriteRenderer.sprite = item.icon;
        }

        Invoke(nameof(EnablePickup), pickupDelay);
    }

    private void EnablePickup()
    {
        canPickup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canPickup)
        {
            if (item == null)
            {
                Debug.LogError("ItemWorld: item이 null입니다!");
                return;
            }

            Debug.Log($"[ItemWorld] {item.itemName} x{amount} 획득 시도");

            if (InventoryManager.Instance == null)
            {
                Debug.LogError("[ItemWorld] InventoryManager.Instance가 null입니다!");
            }
            else
            {
                InventoryManager.Instance.AddItem(item, amount);
                Debug.Log("[ItemWorld] AddItem 호출 완료");
            }

            Destroy(gameObject);
        }
    }
}

