using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    [Header("자원 데이터")]
    public ResourceData data;

    private int currentDurability;

    [SerializeField] private GameObject itemWorldPrefab;
    [SerializeField] private float respawnTime = 10f; // 리스폰 시간

    private void Start()
    {
        if (data == null)
        {
            Debug.LogError("ResourceData가 할당되지 않았습니다: " + gameObject.name);
            return;
        }

        currentDurability = data.maxDurability;
    }

    public void Interact(ToolType tool)
    {
        if (data == null)
        {
            Debug.LogError("ResourceData가 비어있습니다.");
            return;
        }

        if (tool != data.requiredTool)
        {
            Debug.Log("이 자원은 " + data.requiredTool + "로만 채집할 수 있습니다.");
            return;
        }

        currentDurability--;
        Debug.Log($"{data.resourceName} 채집! 남은 내구도: {currentDurability}");

        if (currentDurability <= 0)
        {
            DropItem();
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    private void DropItem()
    {
        if (data.dropItem == null)
        {
            Debug.LogError($"[DropItem] dropItem이 비어 있습니다! Resource: {data.resourceName}");
            return;
        }

        Debug.Log($"{data.dropItem.name} x{data.dropAmount} 드롭!");

        // 나중에 ItemDropManager를 붙이면 여기서 인벤토리 추가 또는 드롭 처리 가능

        Vector3 dropPos = transform.position + Vector3.up * 0.2f;
        GameObject drop = Instantiate(itemWorldPrefab, dropPos, Quaternion.identity);
        drop.GetComponent<ItemWorld>().Set(data.dropItem, data.dropAmount);

    }
    private void Respawn()
    {
        currentDurability = data.maxDurability;
        gameObject.SetActive(true);
    }
}
