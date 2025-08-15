public class RabbitSlimeDieState:EnemyState
{
    private RabbitSlime enemy; 
    public RabbitSlimeDieState(RabbitSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }
    

    public override void Enter()
    {
        enemy.CallDieEvent();
    }
}
