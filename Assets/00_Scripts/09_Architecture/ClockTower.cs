using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockTower : MonoBehaviour ,Buff
{
    private Coroutine speedCoroutine;
    private bool isPlayerInside = false;
    private float timeInterval = 1f;
    private float continueDuration = 60f;
    [SerializeField] private BuffUI buffUI;
    [SerializeField] private ArchitectureData clockTower;

    private void Start()
    {
        buffUI = FindAnyObjectByType<BuffUI>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            speedCoroutine = StartCoroutine(GetBuff());
            buffUI.speed.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    public IEnumerator GetBuff()
    {
        float remainingTime = continueDuration;

        while (isPlayerInside || remainingTime > 0f)
        {
            if (!isPlayerInside)
            {
                remainingTime -= timeInterval;
            }
            else
            {
                remainingTime = continueDuration;
            }

            CharacterManager.Instance.Player.controller.moveSpeed = clockTower.sanctuarySpaces[0].value;

            yield return new WaitForSeconds(timeInterval);
        }

        if (buffUI != null)
        {
            buffUI.speed.SetActive(false);
        }

        CharacterManager.Instance.Player.controller.moveSpeed = 5f;
        speedCoroutine = null;
    }
}