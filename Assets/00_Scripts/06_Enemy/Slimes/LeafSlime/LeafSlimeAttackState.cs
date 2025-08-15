public class LeafSlimeAttackState:EnemyState
{
    private LeafSlime enemy; 
    public LeafSlimeAttackState(LeafSlime _enemy, EnemyStateMachine _stateMachine)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        enemy.Attack();
    }

    public override void Update()
    {
        if (!enemy.IsAttacking&&enemy.FindTarget(enemy.AttackDistance))
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }

        if (!enemy.IsAttacking && !enemy.FindTarget(enemy.AttackDistance))
        {
            enemy.StateMachine.ChangeState(enemy.TraceState);
        }
    }
}
