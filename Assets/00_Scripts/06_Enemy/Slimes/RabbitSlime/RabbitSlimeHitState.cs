public class RabbitSlimeHitState:EnemyState
{ 
    private RabbitSlime enemy; 
    public RabbitSlimeHitState(RabbitSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        enemy.CallHitEvent();
    }
}
