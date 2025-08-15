using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class QuestUI : MonoBehaviour
{
    public Canvas canvas;
    NPCInteractor interactor;
    private QuestManager questManager;

    Dictionary<int, GameObject> questWindows = new Dictionary<int, GameObject>(); // 수주한 퀘스트 창들

    public GameObject QuestContainer;
    public GameObject questWindowPrefab;

    public GameObject npcWindow; // NPC 대화창
    public GameObject questListWindow; // 전체 퀘스트 목록창
    public GameObject RewardWindow; // 보상 완료창

    public bool isQuestUIOpen = false; // UI창이 켜져 있는지 여부
    bool isClose;
    public bool IsClose { get { return isClose; } }

    private void Update()
    {
        Debug.Log("isClose: " + isClose);
        CheckClose();
    }

    private void OnEnable()
    {
        ShowQuestProgress();
    }
    // 화면 우측에 퀘스트 UI 추가
    public void ShowQuestProgress()
    {
        interactor = FindObjectOfType<NPCInteractor>();
        questManager = FindObjectOfType<QuestManager>();

        if (questManager.curQuest == null) { return; }

        // 화면 우측 퀘스트 창 띄우기
        GameObject _questWindow = Instantiate(questWindowPrefab, Vector2.zero, Quaternion.identity);
        _questWindow.transform.SetParent(QuestContainer.transform);

        questWindows[questManager.curQuest.questId] = _questWindow;

        string questDescription = questManager.curQuest.description + " " + questManager.curQuest.curProgress + " / " + questManager.curQuest.totalProgress; ;
        _questWindow.transform.GetChild(0).GetComponent<TMP_Text>().text = questDescription;
    }
    // 화면 우측 퀘스트 UI 삭제
    public void DeleteQuestProgress(int _questId)
    {
        GameObject obj = questWindows[_questId];
        questWindows.Remove(_questId);
        Destroy(obj);
    }
    // 키보드 O키 누르면 전체 퀘스트 목록창 활성화
    public void OnQuestListInfo()
    {
        UIManager.Instance.OpenUI(UIKey.QuestListWindow);
    }
    // 창 활성화 및 비활성화
    public void ShowWindow(GameObject window, bool value)
    {
        if (value) { window.transform.SetAsLastSibling(); isQuestUIOpen = true;  }
        else { window.transform.SetSiblingIndex(1); }
        window.SetActive(value);
    }
    // ESC키 누를시 열려있는 UI 닫기
    public void OnUIClose(InputAction.CallbackContext context)
    {
        if (isClose == true)
        {
            try
            {
                //if (context.phase != InputActionPhase.Started || !isQuestUIOpen)
                //{
                //    return;
                //}
                if (context.phase != InputActionPhase.Started)
                {
                    return;
                }
                //CloseUI();
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
    void CloseUI()
    {
        //isQuestUIOpen = false;
        interactor.isTaking = false;

        // 시점 고정 풀기
        CharacterManager.Instance.Player.controller.ToggleCursor(false);

        Transform canvasTransform = canvas.transform;
        int lastIndex = canvasTransform.childCount - 1;

        // 캔버스의 가장 마지막 UI 닫기
        if (lastIndex >= 0)
        {
            GameObject lastChild = canvasTransform.GetChild(lastIndex).gameObject;
            lastChild.transform.SetSiblingIndex(1);
            UIManager.Instance.CloseUI();
        }
    }

    void CheckClose()
    {
        if (npcWindow.activeSelf == false && questListWindow.activeSelf == false && RewardWindow.activeSelf== false)
        {
            isClose = false; // NPC 대화창 그리고 퀘스트창이 닫히면 isClose를 false로 설정
        }
        else if(npcWindow.activeSelf || questListWindow.activeSelf || RewardWindow.activeSelf)
        {
            isClose = true; // NPC 대화창 또는 퀘스트창이 열려있으면 isClose를 true로 설정
        }
    }
}
