using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoverManager : MonoBehaviour
{
    [Header("Layer")]
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask architectureMask;

    [Header("Ray")]
    private float maxRayDistance = 5f;

    private GameObject removePreviewObject;
    private bool canRemove = false;
    private bool isRemoving; // 건물 삭제모드 실행여부
    public bool IsRemoving { get { return isRemoving; } }

    private BuildSystemUI buildSystemUI;

    private void Start()
    {
        buildSystemUI = FindObjectOfType<BuildSystemUI>(); // 빌드 시스템 UI를 찾음
    }

    private void Update()
    {
        RemoveInput();
    }

    private void RemoveInput()
    {
        if (isRemoving)
        {
            ShowRemovePreview();
            CheckRemoveInput();
        }
    }

    public void RemoveMode() // 건물 삭제모드
    {
        isRemoving = true;
        buildSystemUI.buildSystemPanel.SetActive(false);
        buildSystemUI.ingText.text = "제거모드\n좌클릭 설치\n우클릭 또는 ESC 취소"; // 안내문구 활성화
        CharacterManager.Instance.Player.controller.ToggleCursor(false);
        buildSystemUI.isBuildMode = false; // 빌드모드 비활성화
    }

    private void ShowRemovePreview() // 프리뷰모드
    {
        if (removePreviewObject != null)
        {
            ResetPreviewMaterial(removePreviewObject);
            removePreviewObject = null;
        }

        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 화면 중앙에서
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, architectureMask)) // 레이를 발사하여 건축물 레이어 구분
        {
            removePreviewObject = hit.collider.gameObject;
            canRemove = true;

            SetPreviewMaterial(removePreviewObject, new Color(1f, 0f, 0f, 0.5f));
        }
        else
        {
            canRemove = false;
        }
    }

    private void CheckRemoveInput() // 삭제 결정 시스템
    {
        if (Input.GetMouseButtonDown(0) && canRemove) // 좌클릭하면 삭제
        {
            RemoveArchitecture();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) // 우클릭이나 ECS 누르면 삭제
        {
            CancelRemove();
        }
    }

    private void RemoveArchitecture() // 삭제 확정
    {
        if (removePreviewObject != null)
        {
            Destroy(removePreviewObject);
            removePreviewObject = null;
        }

        canRemove = false;
        isRemoving = false;
        buildSystemUI.ingText.text = string.Empty; // 안내문구 비활성화
    }

    private void CancelRemove() // 삭제 취소
    {
        if (removePreviewObject != null)
        {
            ResetPreviewMaterial(removePreviewObject);
            removePreviewObject = null;
        }

        canRemove = false;
        isRemoving = false;
        buildSystemUI.ingText.text = string.Empty; // 안내문구 비활성화
    }

    private void SetPreviewMaterial(GameObject architecture, Color color) // 투명화 상태로 보여주기
    {
        foreach (Renderer renderer in architecture.GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in renderer.materials)
            {
                mat.shader = Shader.Find("Transparent/Diffuse");
                mat.color = color;
            }
        }
    }

    private void ResetPreviewMaterial(GameObject obj) // 취소할 경우 다시 원래 상태 복구
    {
        foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in renderer.materials)
            {
                mat.shader = Shader.Find("Kawaii/ToonLit");
                mat.color = Color.white;
            }
        }
    }
}
