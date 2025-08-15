using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private LayerMask obstacleMask;
    
    public void Initialize(List<EnemySO> enemyList)
    {
        foreach (var enemySO in enemyList)
        {
            PoolManager.Instance.CreatePool<Enemy>(enemySO.enemyPrefab, enemySO.maxSpawnCount*2);
            PoolManager.Instance.CreatePool<ParticleEffect>(enemySO.enemyWeaponSO.attackEffect, enemySO.maxSpawnCount*2);
            if (enemySO.enemyWeaponSO.enemyWeaponType == EnemyWeaponType.Ranged)
            {
                EnemyRangedWeaponSO enemyRangedWeaponSO = enemySO.enemyWeaponSO as EnemyRangedWeaponSO;
                PoolManager.Instance.CreatePool<Projectile>(enemyRangedWeaponSO.projectile, enemySO.maxSpawnCount*3);
                PoolManager.Instance.CreatePool<ParticleEffect>(enemyRangedWeaponSO.projectileParticle.gameObject, enemySO.maxSpawnCount*3);
            }
        }
    }

    public bool SpawnEnemy(EnemySO enemySO)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition(enemySO.minSpawnAreaBound, enemySO.maxSpawnAreaBound);
        Enemy enemy = PoolManager.Instance.ReuseComponent<Enemy>(enemySO.enemyPrefab, spawnPosition, Quaternion.identity);

        if (enemy != null)
        {
            enemy.gameObject.SetActive(true);
            enemy.Initialize(enemySO);
            return true;
        }
        
        return false;
    }

    public void SpawnAllEnemies(List<EnemySO> enemyList)
    {
        foreach (var enemySO in enemyList)
        {
            for (int i = 0; i < enemySO.maxSpawnCount; i++)
            {
                SpawnEnemy(enemySO);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 min, Vector3 max)
    {
        Vector3 randomPos;
        for (int i = 0; i < EnemySettings.maxEnemySpawnAttempts; i++)
        {
            randomPos = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
            );
            
            if (!Physics.CheckSphere(randomPos, EnemySettings.checkEnemySpawnRadius, obstacleMask))
            {
                return randomPos;
            }
        }
        
        return min;
    }
}