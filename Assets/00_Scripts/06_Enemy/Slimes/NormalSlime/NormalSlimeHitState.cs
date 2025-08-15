public class NormalSlimeHitState:EnemyState
{ 
    private NormalSlime enemy; 
    public NormalSlimeHitState(NormalSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        enemy.CallHitEvent();
    }
}
