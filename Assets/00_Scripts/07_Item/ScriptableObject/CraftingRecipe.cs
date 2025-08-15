using UnityEngine;

// 제작 레시피를 정의하는 ScriptableObject
[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    // 제작 결과로 생성되는 아이템
    public ItemData resultItem;

    // 제작 시 생성되는 아이템 수량
    public int resultCount = 1;

    // 제작에 필요한 재료 아이템과 수량을 정의한 구조체
    [System.Serializable]
    public struct RequiredItem
    {
        // 필요한 아이템 종류
        public ItemData item;

        // 필요한 아이템 수량
        public int count;
    }

    // 제작에 필요한 재료들의 목록
    public RequiredItem[] requiredItems;
}
