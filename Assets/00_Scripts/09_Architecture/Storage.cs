using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour, Buff
{
    private Coroutine hungerCoroutine;
    private bool isPlayerInside = false;
    private float timeInterval = 1f;
    private float continueDuration = 60f;
    [SerializeField] private BuffUI buffUI;
    [SerializeField] private ArchitectureData storage;

    private void Start()
    {
        buffUI = FindAnyObjectByType<BuffUI>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            hungerCoroutine = StartCoroutine(GetBuff());
            buffUI.hunger.SetActive(true);
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

            CharacterManager.Instance.Player.condition.uiCondition.hunger.Add(storage.sanctuarySpaces[0].value);

            yield return new WaitForSeconds(timeInterval);
        }

        if (buffUI != null)
        {
            buffUI.hunger.SetActive(false);
        }

        hungerCoroutine = null;
    }
}