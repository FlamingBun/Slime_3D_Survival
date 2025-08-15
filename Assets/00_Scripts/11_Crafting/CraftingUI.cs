using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 제작 UI를 관리하는 클래스. 제작창 열기, 재료 표시, 제작 버튼 등 처리.
public class CraftingUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;                          // 제작 UI 패널
    public Image itemIcon;                            // 현재 레시피의 아이콘 이미지
    public TextMeshProUGUI itemNameText;              // 아이템 이름 텍스트
    public TextMeshProUGUI requiredText;              // 필요한 재료 목록 텍스트
    public Button craftButton;                        // 제작 버튼
    public Button prevButton;                         // 이전 레시피 버튼
    public Button nextButton;                         // 다음 레시피 버튼

    [Header("레시피")]
    public CraftingRecipe[] recipes;                  // 등록된 레시피 배열
    private int currentIndex = 0;                     // 현재 선택 중인 레시피 인덱스

    [SerializeField]
    private PlayerController playerController;        // 커서 잠금 등을 위해 참조되는 플레이어 컨트롤러

    private bool toggle = false;                      // UI On/Off 상태 토글용

    private void Start()
    {
        panel.SetActive(false); // 시작 시 제작창 비활성화

        // 플레이어 컨트롤러 할당 (인스펙터에서 없을 경우 자동 찾기)
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        // 버튼 이벤트 등록
        craftButton.onClick.AddListener(OnClickCraft);
        nextButton.onClick.AddListener(NextRecipe);
        prevButton.onClick.AddListener(PrevRecipe);

        UpdateUI(); // 초기 UI 세팅
    }

    private void Update()
    {
        // C 키로 제작창 열고 닫기 + 커서 토글
        if (Input.GetKeyDown(KeyCode.C))
        {
            toggle = !toggle;
            playerController.ToggleCursor(toggle); // 커서 잠금/해제
            bool isOpen = !panel.activeSelf;
            panel.SetActive(isOpen);
            UpdateUI();
        }
    }

    // 제작 버튼 클릭 시 호출되는 함수
    private void OnClickCraft()
    {
        var recipe = recipes[currentIndex];

        if (CraftingManager.Instance.CanCraft(recipe))
        {
            CraftingManager.Instance.Craft(recipe);  // 제작 실행
            UpdateUI();                              // 재료 업데이트
        }
        else
        {
            Debug.Log("재료가 부족합니다.");
        }
    }

    // 다음 레시피 보기
    private void NextRecipe()
    {
        currentIndex = (currentIndex + 1) % recipes.Length;
        UpdateUI();
    }

    // 이전 레시피 보기
    private void PrevRecipe()
    {
        currentIndex = (currentIndex - 1 + recipes.Length) % recipes.Length;
        UpdateUI();
    }

    // 현재 선택된 레시피에 따라 UI 요소 갱신
    void UpdateUI()
    {
        var recipe = recipes[currentIndex];

        // 아이콘 및 이름 표시
        itemIcon.sprite = recipe.resultItem.icon;
        itemNameText.text = recipe.resultItem.itemName;

        // 필요한 재료 표시
        requiredText.text = "";
        foreach (var req in recipe.requiredItems)
        {
            int owned = InventoryManager.Instance.GetItemCount(req.item);
            requiredText.text += $"{req.item.itemName}: {owned}/{req.count}\n";
        }
    }
}
