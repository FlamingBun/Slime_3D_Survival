using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DieWindow : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
      //  Cursor.visible = true;
        // 시점 고정 풀기
        CharacterManager.Instance.Player.controller.ToggleCursor(true);
    }
    // 메인 화면으로 가기
    public void MainMenu()
    {

        SceneManager.LoadScene("StartScene");
    }
    // 다시 시작
    public void ReStart()
    {
        SceneManager.LoadScene("MainScene");
    }
   
}
