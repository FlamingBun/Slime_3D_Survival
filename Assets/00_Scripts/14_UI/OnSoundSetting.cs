using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OnSoundSetting : MonoBehaviour
{
    private QuestUI questui; // 퀘스트 UI
    private BuildManager buildManager; // 빌드 매니저
    private RemoverManager removerManager; // 제거 매니저
    private BuildSystemUI buildSystemUI; // 빌드 시스템 UI

    private void Start()
    {
        if (questui == null)
        {
            questui = FindObjectOfType<QuestUI>();
        }
        if (buildManager == null)
        {
            buildManager = FindObjectOfType<BuildManager>();
        }
        if (removerManager == null)
        {
            removerManager = FindObjectOfType<RemoverManager>();
        }
        if (buildSystemUI == null)
        {
            buildSystemUI = FindObjectOfType<BuildSystemUI>();
        }
    }

    public void OnSoundSettingUI(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (UIManager.Instance.UIStack.Count == 0)
            {
                UIManager.Instance.OpenUI(UIKey.SoundUI);
                CharacterManager.Instance.Player.controller.ToggleCursor(true); // 커서 잠금 해제
            }
            else
            {
                UIManager.Instance.CloseUI();
                CharacterManager.Instance.Player.controller.ToggleCursor(false); // 커서 잠금 해제
            }
        }
    }
}
