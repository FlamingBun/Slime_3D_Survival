using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance { get { return instance; } }
    private EnemyDatabaseSO enemyDatabaseSO;
    [SerializeField]private EnemySpawner enemySpawner;
    public DayNightCycle dayNightCycle;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            enemyDatabaseSO = Resources.Load<EnemyDatabaseSO>("EnemyDatabaseSO");
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        enemySpawner.Initialize(enemyDatabaseSO.enemyList);
        enemySpawner.SpawnAllEnemies(enemyDatabaseSO.enemyList);

    }

    public void RespawnEnemy(EnemySO enemySO)
    {
        StartCoroutine(EnemyRespawnRoutine(enemySO, enemySO.respawnTime));
    }

    private IEnumerator EnemyRespawnRoutine(EnemySO enemySO, float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        enemySpawner.SpawnEnemy(enemySO);
    }
}
