using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildSystemUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject buildSystemPanel; // 빌드 시스템 UI 패널
    public TextMeshProUGUI currentResourceText; // 현재 자원 텍스트
    public TextMeshProUGUI buildCostText; // 소모 자원 텍스트
    public TextMeshProUGUI ingText; // 안내 텍스트
    public Image architectureImage; // 건축물 이미지
    public Button buildButton; // 건축 버튼

    private int buildIndex; // 건축물 번호
    public bool isBuildMode; // 빌드 모드 여부
    private Coroutine warningCoroutine; // 경고 코루틴
    private BuildManager buildManager; // 빌드 매니저
    private RemoverManager removerManager; // 삭제 매니저

    private void Awake()
    {
        buildManager = FindObjectOfType<BuildManager>(); // 빌드 매니저를 찾음
        removerManager = FindObjectOfType<RemoverManager>(); // 삭제 매니저 찾음
    }

    private void Update()
    {
        UpdateResourceUI(); // 자원 UI 업데이트
    }

    public void ExitBuildUI()
    {
        //buildSystemPanel.SetActive(false); // 빌드 시스템 UI 패널 비활성화
        UIManager.Instance.CloseUI();
        CharacterManager.Instance.Player.controller.ToggleCursor(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UpdateResourceUI() // 현재 자원 UI 업데이트 메서드
    {
        int wood = InventoryManager.Instance.GetItemCount(buildManager.woodItem); // 인벤토리에서 나무(Wood) 수량을 가져옴
        int stone = InventoryManager.Instance.GetItemCount(buildManager.stoneItem); // 인벤토리에서 돌(Stone) 수량을 가져옴
        int metal = InventoryManager.Instance.GetItemCount(buildManager.metalItem); // 인벤토리에서 금속(Metal) 수량을 가져옴

        currentResourceText.text = $"보유자원\nWood : {wood}   Rock : {stone}   Metal : {metal}";
    }

    public void UpdateCost()
    {
        float woodCost = buildManager.currentSelectedArchitecture.buildCosts.buildWoodCost;
        float stoneCost = buildManager.currentSelectedArchitecture.buildCosts.buildStoneCost;
        float metalCost = buildManager.currentSelectedArchitecture.buildCosts.buildMetalCost;

        buildCostText.text = $"필요자원\nWood : {woodCost}   Rock : {stoneCost}   Metal : {metalCost}";
    }

    public void UpdateImage()
    {
        architectureImage.sprite = buildManager.currentSelectedArchitecture.image;
    }

    public void NextButton()
    {
        buildIndex++;
        if (buildIndex >  buildManager.ArchitectureDatas.Length -1)
        {
            buildIndex = 0;
        }
        buildManager.currentSelectedArchitecture = buildManager.ArchitectureDatas[buildIndex];
        UpdateCost();
        UpdateImage();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void PrevButton()
    {
        buildIndex--;
        if (buildIndex < 0)
        {
            buildIndex = buildManager.ArchitectureDatas.Length -1;
        }
        buildManager.currentSelectedArchitecture = buildManager.ArchitectureDatas[buildIndex];
        UpdateCost();
        UpdateImage();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Build()
    {
        buildManager.CheckResources(buildManager.currentSelectedArchitecture);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ShowWarningText()
    {
        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);

        warningCoroutine = StartCoroutine(ShowWarningCoroutine());
    }

    public IEnumerator ShowWarningCoroutine()
    {
        ingText.text = "자원이 부족합니다";
        yield return new WaitForSeconds(0.5f);
        ingText.text = string.Empty;
    }

    public void OnBuildSystem(InputAction.CallbackContext context) // 빌드 시스템 활성화/비활성화 메서드
    {
        if (context.phase == InputActionPhase.Started && buildManager.IsShowPrint == false && removerManager.IsRemoving == false)
        {
            if (!buildSystemPanel.activeSelf)
            {
                // buildSystemPanel.SetActive(true); // 빌드 시스템 UI 패널 활성화
                UIManager.Instance.OpenUI(UIKey.BuildSystemPanel);
                CharacterManager.Instance.Player.controller.ToggleCursor(true);
                isBuildMode = true; // 빌드 모드 활성화
                buildManager.currentSelectedArchitecture = buildManager.ArchitectureDatas[0];
                buildIndex = 0;
                UpdateCost();
                UpdateImage();
            }
            else
            {
                // buildSystemPanel.SetActive(false); // 빌드 시스템 UI 패널 비활성화
                UIManager.Instance.CloseUI();
                isBuildMode = false; // 빌드 모드 비활성화
                CharacterManager.Instance.Player.controller.ToggleCursor(false);
            }
        }
    }
}