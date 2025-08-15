using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public bool godMode;
   
    public void Start()
    {
        if (godMode)
        {
            PlayerPrefs.DeleteAll();
        }     
    }
    // IntoScene으로 이동
    public void IntroScene()
    {
        int showIntro = PlayerPrefs.GetInt("ShowIntro", 0);

        if (showIntro == 0)
        {
            SceneManager.LoadScene("IntroScene");
            return;
        }
        SceneManager.LoadScene("MainScene");
        
    }
}
