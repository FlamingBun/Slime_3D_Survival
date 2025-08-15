using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO_", menuName = "ScriptableObject/Enemy/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Information")]
    public string enemyID;
    public string enemyName;

    public GameObject enemyPrefab;
    
    [Space(10)]
    [Header("Stats")]
    public float health;
    public float moveSpeed;
    public float runSpeed; // 도망또는 추격할 때
    public float hitRecoveryTime;
    public float detectDistance; // 최소 범위
    
    [Space(10)]
    [Header("Chase Setting")]
    public float maxTraceDistance;
    public float startWanderDistance; // 정찰 시작 Distance
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    public float minWanderDistance;
    public float maxWanderDistance;
    
    [Space(10)]
    [Header("Attack Setting")]
    public float fieldOfView;
    public EnemyWeaponSO enemyWeaponSO;


    [Space(10)]
    [Header("Spawn")]
    public float respawnTime;
    public int maxSpawnCount;
    public Vector3 minSpawnAreaBound;
    public Vector3 maxSpawnAreaBound;
}
