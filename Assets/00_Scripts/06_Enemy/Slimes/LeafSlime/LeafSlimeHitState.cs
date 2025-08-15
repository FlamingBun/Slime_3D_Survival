public class LeafSlimeHitState:EnemyState
{ 
    private LeafSlime enemy; 
    public LeafSlimeHitState(LeafSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        enemy.CallHitEvent();
    }
}
