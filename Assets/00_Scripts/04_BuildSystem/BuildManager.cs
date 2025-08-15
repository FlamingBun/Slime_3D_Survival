using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [Header("Resource")]
    public ItemData woodItem; // 나무 자원
    public ItemData stoneItem; // 돌 자원
    public ItemData metalItem;  // 금속 자원

    [Header("Ray")]
    [SerializeField] private LayerMask groundMask; // 땅 체크용
    [SerializeField] private LayerMask obstacleMask; // 장애물 체크용
    [SerializeField] private LayerMask houseMask; // 집 체크용
    [SerializeField] private float obstacleCheckRadius; // 충돌 검사 반경
    [SerializeField] private float houseCheckRadius; // House 거리 체크 반경
    private float maxRayDistance = 10f; // Ray의 최대 거리 설정

    [Header("Build")]
    private bool canPlace;
    private bool isShowPrint; // 프리뷰 확인
    public bool IsShowPrint { get { return isShowPrint; } }
    private Vector3 previewPosition; // 프리뷰의 위치
    private Vector3 previewLookDirection; //프리뷰가 바라보는 방향
    private float previewRotationY; // 프리뷰 로테이션의 Y 값
    private float baseRotationY; // 플레이어 기준 회전하는 Y 값
    private Vector3 lastValidPosition; // 마지막 위치
    private Quaternion previewRotation; // 프리뷰 회전값
    private GameObject currentPreviewObject; // 현재 미리보기 오브젝트
    public GameObject CurrentPreviewObject { get { return currentPreviewObject; } } // 현재 미리보기 오브젝트 반환
    private GameObject finalBuilding; // 최종 건축
    private BuildSystemUI buildSystemUI; // 빌드 시스템 UI

    public ArchitectureData currentSelectedArchitecture; // 현재 선택된 건축물 데이터
    [SerializeField] private ArchitectureData[] architectureDatas; // 건축물 데이터 보관소
    public ArchitectureData[] ArchitectureDatas {  get { return architectureDatas; } }

    private void Start()
    {
        buildSystemUI = FindObjectOfType<BuildSystemUI>(); // 빌드 시스템 UI를 찾음
    }

    private void Update()
    {
        BuildInput(); // 빌드 입력 처리 
    }

    private void BuildInput()
    {
        if (currentPreviewObject != null) // 현재 미리보기 오브젝트가 있다면
        {
            MovePreviewToMouse(); // 마우스 위치로 미리보기 오브젝트 이동
            UpdatePreviewColor(); // 실시간 설치 가능 여부 확인 + 색상 업데이트
            PreViewRotationY(); // Q,E로 로테이션
            CheckBuild(); //최종설치 여부
        }
    }

    private void PreViewRotationY() //Q,E로 로테이션
    {
        if (isShowPrint && currentPreviewObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                previewRotationY -= 90f;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                previewRotationY += 90f;
            }
        }
    }

    private void CheckBuild() //최종 설치 여부
    {
        if (Input.GetMouseButtonDown(0)) // 좌클시 설치 확정
        {
            if (canPlace)
            {
                PlaceBuilding();
                buildSystemUI.ingText.text = string.Empty; // 안내문구 비활성화
            }
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) // 우클 또는 ESC 누르면 설치 취소
        {
            CancelBuildPreview();
            buildSystemUI.ingText.text = string.Empty; // 안내문구 비활성화
        }
    }

    private bool HasBuildCost(ArchitectureData architectureData, BuildCostManager buildCostManagers) // 자원양이 충분한지 체크하는 불리언 값을 확인하는 메서드
    {
        bool hasWoodCost = false;
        bool hasStoneCost = false;
        bool hasMetalCost = false;

        // 건축물에 필요한 자원 비용을 가져옴
        int woodCost = (int)buildCostManagers.buildWoodCost; // 필요한 나무 수
        int stoneCost = (int)buildCostManagers.buildStoneCost; // 필요한 돌 수
        int metalCost = (int)buildCostManagers.buildMetalCost; // 필요한 금속 수

        // 현재 인벤토리에 존재하는 자원 수를 확인 (InventoryManager에서 가져옴)
        int haveWood = InventoryManager.Instance.GetItemCount(woodItem); // 가진 나무 수
        int haveStone = InventoryManager.Instance.GetItemCount(stoneItem); // 가진 돌 수
        int haveMetal = InventoryManager.Instance.GetItemCount(metalItem); // 가진 금속 수

        if (haveWood >= woodCost)
        {
            hasWoodCost = true;
        }
        if (haveStone >= stoneCost)
        {
            hasStoneCost = true;
        }
        if (haveMetal >= metalCost)
        {
            hasMetalCost = true;
        }

        // 자원이 전부 충분해야만 true 반환 → 그래야 건축 허용
        return hasWoodCost && hasStoneCost && hasMetalCost;
    }

    public void CheckResources(ArchitectureData architectureData) //자원 양이 충분하면 미리보기를 실행하는 메서드
    {
        if (HasBuildCost(architectureData, architectureData.buildCosts)) // 자원이 충분하다면
        {
            currentSelectedArchitecture = architectureData; // 현재 선택된 건축물 데이터 설정
            buildSystemUI.buildSystemPanel.SetActive(false); // 빌드 시스템 UI 패널 비활성화
            ShowBluePrint(architectureData); // 건축물 프리팹을 미리보기로 보여줌
            buildSystemUI.isBuildMode = false;
            buildSystemUI.ingText.text = "건축모드\n좌클릭 설치\n 우클릭 또는 ESC 취소\nQ,E 회전"; // 안내문구 활성화
        }
        else
        {
            buildSystemUI.ShowWarningText(); // 자원 부족 문구
        }
    }

    private void ShowBluePrint(ArchitectureData architectureData) // 건축물의 프리팹을 미리보기로 보여주는 메서드
    {
        isShowPrint = true;
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject); // 이전에 생성된 미리보기 오브젝트가 있다면 제거
        }

        currentPreviewObject = Instantiate(architectureData.architecturePrefab); // 건축물 프리팹을 인스턴스화하여 미리보기 오브젝트로 설정
        previewRotationY = 0f;
    }

    private void UpdatePreviewColor() // 현재 미리보기 오브젝트의 색상을 업데이트하는 메서드
    {
        if (currentPreviewObject == null) return; // 현재 미리보기 오브젝트가 없다면 X

        canPlace = CanPlaceBuilding(); // 현재 위치에 건축물을 설치할 수 있는지 확인
        Color color = canPlace ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f); // 색상 설정 (초록색: 설치 가능, 빨간색: 설치 불가능)

        foreach (Renderer renderer in currentPreviewObject.GetComponentsInChildren<Renderer>()) // 현재 미리보기 오브젝트의 모든 렌더러를 가져와서 색상 업데이트
        {
            foreach (Material material in renderer.materials) // 각 랜더러의 재질을 업데이트
            {
                material.shader = Shader.Find("Transparent/Diffuse");
                material.color = color;
            }
        }
    }

    private bool CanPlaceBuilding() // 현재 위치에 건축물을 설치할 수 있는지 확인하는 불리언 값을 확인하는 메서드
    {
        if (currentPreviewObject == null) // 현재 미리보기 오브젝트가 없다면
            return false; // 설치 불가능

        Vector3 position = currentPreviewObject.transform.position; // 현재 미리보기 오브젝트의 위치를 가져옴
        // 1. 바닥 체크
        bool isGrounded = false;
        Ray groundRay = new Ray(position + Vector3.up * 0.1f, Vector3.down); // 레이 발사
        if (Physics.Raycast(groundRay, out RaycastHit ground, 0.2f, groundMask)) // 레이 방향, 그라운드 부딪칠때
        {
            if (Vector3.Angle(ground.normal, Vector3.up) <= 15f) // 충돌면 각도 15도 이하
            {
                isGrounded = true;
            }
        }

        // 2. 장애물 체크
        bool hasObstacle = false;
        Collider[] hits = Physics.OverlapSphere(position, obstacleCheckRadius, obstacleMask); //CheckSphere는 구 안에 하나라도 Colider가 있는지 확인, OverlapSphere는 구 안에 겹치는 colider를 모두 가져옴
        foreach (Collider hit in hits)
        {
            if (hit.gameObject == currentPreviewObject) continue; // 미리보기 오브젝트는 무시
            hasObstacle = true;  // 다른 충돌이 있으면 장애물로 간주
            break;
        }

        // 3. 자원 체크
        bool hasResources = HasBuildCost(currentSelectedArchitecture, currentSelectedArchitecture.buildCosts); // 현재 선택된 건축물의 자원 비용이 충분한지 확인

        // 4. 집 여부 체크
        bool nearHouse = true; // 기본적으로 true로 설정, 나중에 SanctuarySpace 타입일 때만 false로 변경
        if (currentSelectedArchitecture.architectureType == ArchitectureType.SanctuarySpace) // 현재 선택된 건축물이 SanctuarySpace 타입이라면
        {
            nearHouse = Physics.CheckSphere(position, houseCheckRadius, houseMask); // 현재 위치에서 houseCheckRadius 반경 내에 집이 있는지 확인
        }
        return isGrounded && !hasObstacle && nearHouse && hasResources; // 모든 조건을 만족하면 true 반환, 아니면 false 반환
    }

    private void MovePreviewToMouse() // 마우스 위치로 미리보기 오브젝트를 이동시키는 메서드
    {
        if (currentPreviewObject != null) // 현재 미리보기 오브젝트가 있다면
        {
            previewPosition = GetPreviewMousePosition(); // 현재 선택된 미리보기 오브젝트를 마우스 위치로 이동
            currentPreviewObject.transform.position = previewPosition; // 마우스 위치로 미리보기 오브젝트 위치 설정

            CheckDirection(); // 1.방향 계산
            CheckRotation(); // 2.회전값 계산

            previewRotation = currentPreviewObject.transform.rotation;
        }
    }

    private void CheckDirection() // 방향 계산
    {
        previewLookDirection = previewPosition - CharacterManager.Instance.Player.transform.position; // 플레이어 위치에서 미리보기 오브젝트 위치로 향하는 방향 벡터 계산
        previewLookDirection.y = 0f; // Y축을 0으로 설정하여 수평 방향으로만 회전하도록 함

        if (previewLookDirection.sqrMagnitude > 0.01f)
        {
            baseRotationY = Quaternion.LookRotation(previewLookDirection).eulerAngles.y;
        }
    }

    private void CheckRotation() // 회전 계산
    {
        float fixedX = currentSelectedArchitecture.index == ItemIndex.House ? -90f : 0f; // 집일때만 X 값 - 90
        float finalRotationY = baseRotationY + previewRotationY;

        if (currentSelectedArchitecture.index > ItemIndex.Tent) // 인덱스 값이 1보다 크면 + 180
        {
            finalRotationY += 180f;
        }
        currentPreviewObject.transform.rotation = Quaternion.Euler(fixedX, finalRotationY, 0f);
    }

    private Vector3 GetPreviewMousePosition() // 마우스 위치에 미리보기 오브젝트를 이동시키는 메서드
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 Ray 생성
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, groundMask)) // Ray가 충돌하는 지점이 groundMask를 가지고 있다면
        {
            lastValidPosition = hit.point;
            return hit.point; // 충돌 지점의 위치를 반환
        }
        return lastValidPosition; // 충돌 지점이 없으면 마지막 위치를 반환
    }

    private void PlaceBuilding() // 현재 선택된 건축물을 설치하는 메서드
    {
        if (currentSelectedArchitecture == null || currentPreviewObject == null) // 현재 선택된 건축물 데이터나 미리보기 오브젝트가 없다면
            return;

        // 현재 선택된 건축물의 건축 비용 정보를 가져옴
        BuildCostManager cost = currentSelectedArchitecture.buildCosts;

        // 자원 차감
        InventoryManager.Instance.RemoveItem(woodItem, (int)cost.buildWoodCost);
        InventoryManager.Instance.RemoveItem(stoneItem, (int)cost.buildStoneCost);
        InventoryManager.Instance.RemoveItem(metalItem, (int)cost.buildMetalCost);

        // 최종 설치
        finalBuilding = Instantiate(currentSelectedArchitecture.architecturePrefab, previewPosition, previewRotation); //프리뷰 값 그대로 설치

        ChangeMeshCollider();// 메시 콜라이더 교체
        TurnOnBuff(); // 버프 활성화

        // 설치 이후 초기화
        Destroy(currentPreviewObject);
        currentPreviewObject = null;
        currentSelectedArchitecture = null;
        finalBuilding = null;
        isShowPrint = false;
        CharacterManager.Instance.Player.controller.ToggleCursor(false); // 커서 잠금
        FindObjectOfType<QuestManager>().AddProgress(1, 1); // 건축 퀘스트 진행도 증가
    }

    private void ChangeMeshCollider()
    {
        MeshCollider existing = finalBuilding.GetComponent<MeshCollider>(); //MeshCollider를 찾아오기
        if (existing != null)
        {
            Mesh sharedMesh = existing.sharedMesh; // 메시 정보 저장
            Destroy(existing); // convex=true인 기존 콜라이더 제거

            MeshCollider newCollider = finalBuilding.AddComponent<MeshCollider>(); //MeshCollider를 새로 생성
            newCollider.sharedMesh = sharedMesh; //메시 저장 정보 값 할당
            newCollider.convex = false; // convex 끔
        }
    }

    private void TurnOnBuff() // 버프 영역 활성화
    {
        Transform buffTransform = finalBuilding.transform.Find("Buff"); // Buff 이름을 찾아서
        if (buffTransform != null)
        {
            buffTransform.gameObject.SetActive(true); // 활성화
        }
    }

    private void CancelBuildPreview()
    {
        if (currentPreviewObject != null) //  미리보기 오브젝트가 있다면
        {
            Destroy(currentPreviewObject); // 미리보기 오브젝트 제거
            currentPreviewObject = null; // 초기화
            currentSelectedArchitecture = null; // 현재 선택된 건축물 데이터도 초기화
            isShowPrint = false;
            CharacterManager.Instance.Player.controller.ToggleCursor(false); // 커서 잠금
        }
    }
}