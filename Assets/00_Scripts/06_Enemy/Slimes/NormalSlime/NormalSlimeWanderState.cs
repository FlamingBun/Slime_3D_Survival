using UnityEngine;

public class NormalSlimeWanderState:EnemyState
{
    private NormalSlime enemy; 
    public NormalSlimeWanderState(NormalSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }
    
    public override void Enter()
    {
        enemy.CallMoveEvent();
        enemy.Wander();
    }
    
    public override void Update()
    {
        if (enemy.IsHit && enemy.FindTarget(enemy.TraceDistance))
        {
            enemy.StateMachine.ChangeState(enemy.TraceState);
        }
        
        if(!enemy.FindTarget(enemy.WanderDistance))
        {
            enemy.StateMachine.ChangeState(enemy.ReturnState);
        }

        if (enemy.ArriveTargetPosition())
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }
    
}
