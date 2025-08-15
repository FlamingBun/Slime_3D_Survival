using UnityEngine;

public class LeafSlimeIdleState:EnemyState
{
    private LeafSlime enemy;
    private float wanderWaitTime;    
    private bool readyToWander;
    public LeafSlimeIdleState(LeafSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        readyToWander = false;
        wanderWaitTime = Random.Range(enemy.MinWanderWaitTime, enemy.MaxWanderWaitTime);
        
        enemy.CallIdleEvent();
    }
    
    public override void Update()
    {
        wanderWaitTime -= Time.deltaTime;
        if (wanderWaitTime <= 0)
        {
            readyToWander = true;
        }

        if (enemy.FindTarget(enemy.WanderDistance) && readyToWander)
        {
            enemy.StateMachine.ChangeState(enemy.WanderState);
        }

        if (enemy.FindTarget(enemy.TraceDistance))
        {
            enemy.StateMachine.ChangeState(enemy.TraceState);
        }
        
    }
}
