public class LeafSlimeReturnState:EnemyState
{
    private LeafSlime enemy; 
    public LeafSlimeReturnState(LeafSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        enemy.CallRunEvent();
        enemy.BackToOriginPosition();
    }

    public override void Update()
    {
        if (enemy.ArriveTargetPosition())
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }
}
