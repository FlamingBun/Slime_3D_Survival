public class NormalSlimeDieState:EnemyState
{
    private NormalSlime enemy; 
    public NormalSlimeDieState(NormalSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }
    

    public override void Enter()
    {
        enemy.CallDieEvent();
    }
}




