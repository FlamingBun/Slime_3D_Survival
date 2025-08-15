using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class StoryBoardManager : MonoBehaviour
{
   SettingSound settingSound;

    public Image image;
    public TMP_Text storyBoardText;
    public List<Sprite> list;
    public List<string> text;

    public float fadeTime = 0.8f;
    public float typingSpeed = 0.05f;

    public int currentIndex = 1;
    private bool isPlaying = false;
    public bool endingScene = false;

    private void Start()
    {
        Time.timeScale = 1.0f;
        settingSound = GetComponent<SettingSound>();
        StartCoroutine(PrintSentence(text[currentIndex - 1]));


    }
    public void OnStoryboard(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isPlaying)
        {
            settingSound.SfxPlay();

            // 한 장면씩 출력
            if (currentIndex < list.Count)
            {
                StartCoroutine(PlayStoryboardStep());
            }
            // 마지막 장면이라면
            else if (currentIndex == list.Count)
            {
                
                IsEnd();
            }
        }
    }
    private IEnumerator PlayStoryboardStep()
    {
        isPlaying = true;

        // 페이드 아웃
        yield return image.DOFade(0f, fadeTime).WaitForCompletion();

        // 이미지 교체
        image.sprite = list[currentIndex];

        // 텍스트 교체 (끝날 때까지 기다림)
        yield return StartCoroutine(PrintSentence(text[currentIndex]));

        currentIndex++;

        // 페이드 인
        yield return image.DOFade(1f, fadeTime).WaitForCompletion();

        isPlaying = false;
    }
    // 한글자씩 글쓰기 효과
    private IEnumerator PrintSentence(string sentence)
    {
        storyBoardText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            storyBoardText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    // 엔딩씬인지 확인
    void IsEnd()
    {
        // 엔딩씬이면
        if (endingScene)
        {
            // 게임 종료
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서는 플레이 모드 종료
#else
    Application.Quit(); // 빌드된 게임에서는 종료
#endif
        }
        // 아니면
        else
        {
            // 게임 시작
            MainScene();
        }
    }

    // MainScene으로 이동
    public void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
