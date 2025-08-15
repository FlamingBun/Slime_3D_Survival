public class LeafSlimeDieState:EnemyState
{
    private LeafSlime enemy; 
    public LeafSlimeDieState(LeafSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }
    

    public override void Enter()
    {
        enemy.CallDieEvent();
    }
}
