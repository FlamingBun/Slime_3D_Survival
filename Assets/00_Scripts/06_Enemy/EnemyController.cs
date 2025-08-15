using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Vector3 spawnPosition;

    #region Move
    private float moveSpeed;
    private float runSpeed;
    private float detectDistance;
    private float attackDistance;
    private float maxTraceDistance;
    private float minWanderDistance;
    private float maxWanderDistance;
    private Vector3 minSpawnAreaBound;
    private Vector3 maxSpawnAreaBound;
    #endregion Move
    
    #region Attack
    private float targetDistance;
    private float fieldOfView; // 몬스터 시야각
    #endregion Attack
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(EnemySO enemySO)
    {
        moveSpeed = enemySO.moveSpeed;
        runSpeed = enemySO.runSpeed;
        detectDistance = enemySO.detectDistance;
        attackDistance = enemySO.enemyWeaponSO.attackDistance;
        
        minWanderDistance = enemySO.minWanderDistance;
        maxWanderDistance = enemySO.maxWanderDistance;
        maxTraceDistance = enemySO.maxTraceDistance;
        
        // spawn 할 때 포지션으로 변경
        spawnPosition = transform.position;
        minSpawnAreaBound = enemySO.minSpawnAreaBound;
        maxSpawnAreaBound = enemySO.maxSpawnAreaBound;
        
        fieldOfView = enemySO.fieldOfView;
        
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackDistance;
    }

    public void WanderNewLocation()
    {
        SetTargetPosition(GetWanderLoacation());
    }

    private Vector3 GetWanderLoacation()
    {
        NavMeshHit hit;
        spawnPosition.y = transform.position.y;
        NavMesh.SamplePosition(spawnPosition + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        
        int i = 0;

        while (Vector3.Distance(spawnPosition, hit.position) < detectDistance||!CheckInBounds(hit.position))
        {
            NavMesh.SamplePosition(spawnPosition + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)) + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i >= EnemySettings.maxGetWanderLoacationAttempts) break;
        }
        return hit.position;
    }

    private bool CheckInBounds(Vector3 _position)
    {
        if (_position.x >= maxSpawnAreaBound.x || _position.x <= minSpawnAreaBound.x)
        {
            return false;   
        }
        
        if (_position.z >= maxSpawnAreaBound.z || _position.z <= minSpawnAreaBound.z)
        {
            return false;   
        }

        return true;
    }

    public bool ArriveTargetPosition()
    {
        if (agent.remainingDistance <= attackDistance)
        {
            return true;
        }

        return false;
    }

    public void BackToOriginPosition()
    {
        SetTargetPosition(spawnPosition);
    }

    public void SetTargetPosition(Vector3 _targetPosition)
    {
        agent.speed = moveSpeed;
        agent.isStopped = false;
        agent.SetDestination(_targetPosition);
    }

    public bool IsFarFromSpawn()
    {
        if (Vector3.Distance(agent.destination, spawnPosition) > maxTraceDistance)
        {
            return true;
        }

        return false;
    }

    public bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        
        // 정면으로 바라보고 있는 위치에서 플레이어와 방향의 각
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        
        // 각을 반으로 -> 왼쪽 오른쪽 각
        return angle < fieldOfView * 0.5f;
    }


    public void StopNavigation()
    {
        agent.isStopped = true;
    }

    public float GetWalkAnimationSpeed()
    {
        return agent.speed / moveSpeed;
    }
    
    public float GetRunAnimationSpeed()
    {
        return agent.speed / runSpeed;
    }

    public void SetMoveSpeedMultiplier(float speedMultiplier)
    {
        moveSpeed *= speedMultiplier;
    }

}