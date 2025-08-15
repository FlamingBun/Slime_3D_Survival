using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour, Buff
{
    private Coroutine thirstCoroutine;
    private bool isPlayerInside = false;
    private float timeInterval = 1f;
    private float continueDuration = 60f;
    [SerializeField] private BuffUI buffUI;
    [SerializeField] private ArchitectureData market;

    private void Start()
    {
        buffUI = FindAnyObjectByType<BuffUI>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            thirstCoroutine = StartCoroutine(GetBuff());
            buffUI.thirst.SetActive(true);
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

            CharacterManager.Instance.Player.condition.uiCondition.thirst.Add(market.sanctuarySpaces[0].value);

            yield return new WaitForSeconds(timeInterval);
        }

        if (buffUI != null)
        {
            buffUI.thirst.SetActive(false);
        }

        thirstCoroutine = null;
    }
}