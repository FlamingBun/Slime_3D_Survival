using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tent : MonoBehaviour, Buff
{
    private Coroutine healingCoroutine;
    private bool isPlayerInside = false;
    private float timeInterval = 1f;
    private float continueDuration = 60f;
    [SerializeField] private BuffUI buffUI;
    [SerializeField] private ArchitectureData tent;

    private void Start()
    {
        buffUI = FindAnyObjectByType<BuffUI>();
    }

    public void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            healingCoroutine = StartCoroutine(GetBuff());
            buffUI.stamina.SetActive(true);
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

            CharacterManager.Instance.Player.condition.uiCondition.stamina.Add(tent.sanctuarySpaces[0].value);

            yield return new WaitForSeconds(timeInterval);
        }

        if (buffUI != null)
        {
            buffUI.stamina.SetActive(false);
        }

        healingCoroutine = null;
    }
}
