using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light playerLight;

    // Update is called once per frame
    void Update()
    {
        if(EnemyManager.Instance.dayNightCycle.InNight)
        {
            playerLight.gameObject.SetActive(true);
        }   
        else
        {
            playerLight.gameObject.SetActive(false);
        }
    }
}
