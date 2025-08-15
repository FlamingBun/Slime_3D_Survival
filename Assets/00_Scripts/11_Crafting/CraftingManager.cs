using UnityEngine;

// 제작 시스템을 관리하는 매니저 클래스
public class CraftingManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static CraftingManager Instance;

    private void Awake()
    {
        // 중복 생성 방지 및 싱글턴 인스턴스 설정
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 해당 레시피의 제작이 가능한지 확인 (재료가 충분한지)
    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var req in recipe.requiredItems)
        {
            // 현재 인벤토리에 있는 아이템 수량이 부족하면 false 반환
            if (InventoryManager.Instance.GetItemCount(req.item) < req.count)
                return false;
        }
        return true;
    }

    // 아이템 제작 수행 (재료 소모 + 결과 아이템 추가)
    public void Craft(CraftingRecipe recipe)
    {
        // 1. 필요한 재료를 인벤토리에서 차감
        foreach (var req in recipe.requiredItems)
        {
            InventoryManager.Instance.RemoveItem(req.item, req.count);
        }

        // 2. 제작된 결과 아이템을 인벤토리에 추가
        InventoryManager.Instance.AddItem(recipe.resultItem, recipe.resultCount);

        // 3. 제작 성공 로그 출력
        Debug.Log($"[Crafting] {recipe.resultItem.itemName} 제작 완료");
    }
}

