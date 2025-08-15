using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : MonoBehaviour
{
    public GameObject InfoWindow;

    public void ShowInfoWindow()
    {
        InfoWindow.SetActive(true);
    }
    public void CloseInfoWindow()
    {
        InfoWindow.SetActive(false);
    }
}
