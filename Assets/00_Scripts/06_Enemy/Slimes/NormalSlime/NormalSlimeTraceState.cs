using UnityEngine;

public class NormalSlimeTraceState:EnemyState
{
    private Vector3 targetPosition;
    private NormalSlime enemy; 
    public NormalSlimeTraceState(NormalSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        enemy.CallMoveEvent();
        enemy.TraceTarget();
        targetPosition = enemy.Target.transform.position;
    }

    public override void Update()
    {
        if (!enemy.FindTarget(enemy.TraceDistance))
        {
            enemy.StateMachine.ChangeState(enemy.WanderState);
        }

        if (enemy.IsFarFromSpawn())
        {
            enemy.StateMachine.ChangeState(enemy.ReturnState);
        }

        if (targetPosition != enemy.Target.transform.position)
        {
            targetPosition = enemy.Target.transform.position;
            enemy.TraceTarget();
        }
        
        if (enemy.ArriveTargetPosition())
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }

}
