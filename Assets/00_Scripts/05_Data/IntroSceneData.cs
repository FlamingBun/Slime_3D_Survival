using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("ShowIntro", 1);
    }
}
