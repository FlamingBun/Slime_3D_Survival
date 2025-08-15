using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

#region REQUIRE COMPONENTS
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyCondition))]
#endregion REQUIRE COMPONENTS

public class LeafSlime : Enemy
{
    public LeafSlimeIdleState IdleState { get; private set; }
    public LeafSlimeWanderState WanderState { get; private set; }
    public LeafSlimeTraceState TraceState { get; private set; }
    public LeafSlimeReturnState  ReturnState { get; private set; } // 스폰 위치로 되돌아가는 상태 
    public LeafSlimeAttackState AttackState { get; private set; }
    public LeafSlimeHitState  HitState { get; private set; }
    public LeafSlimeDieState DieState { get; private set; }

    protected override void InitializeStates()
    {
        // enemy States
        IdleState = new LeafSlimeIdleState(this, stateMachine);
        WanderState = new LeafSlimeWanderState(this, stateMachine);
        TraceState = new LeafSlimeTraceState(this, stateMachine);
        ReturnState = new LeafSlimeReturnState(this, stateMachine);
        AttackState = new LeafSlimeAttackState(this, stateMachine);
        HitState = new LeafSlimeHitState(this, stateMachine);
        DieState = new LeafSlimeDieState(this, stateMachine);
    }

    public override void Initialize(EnemySO _enemySO)
    {
        enemySO = _enemySO;
        enemyWeaponSO = enemySO.enemyWeaponSO;
        
        // 거리
        WanderDistance = enemySO.startWanderDistance;
        TraceDistance = enemySO.maxTraceDistance;
        AttackDistance = enemyWeaponSO.attackDistance;
        
        MinWanderWaitTime = enemySO.minWanderWaitTime;
        MaxWanderWaitTime = enemySO.maxWanderWaitTime;
        
        enemyController.Initialize(enemySO);
        enemyCondition.Initialize(this, enemySO.health, enemySO.hitRecoveryTime);

        Target = CharacterManager.Instance.Player;
        ReadyToWander = false;
        IsAttacking = false;

        SetAttackAnimationSpeed(enemyWeaponSO.attackRate);
        
        stateMachine.Initialize(IdleState);
    }

    public override void Attack()
    {
        if (IsAttacking) return;
        
        if (enemyController.IsPlayerInFieldOfView())
        {
            CallAttackEvent();
            enemyWeaponSO.Attack(attackPoint);
            StartCoroutine(AttackCoolDown(enemyWeaponSO.attackRate));
        }
        else
        {
            Vector3 directionToPlayer = (Target.transform.position - transform.position).normalized;

            
            directionToPlayer.y = 0f;
            
            if (directionToPlayer.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 회전 속도 조절
            }
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        if (enemyWeaponSO == null) return;
        if (enemyWeaponSO.enemyWeaponType != EnemyWeaponType.Melee) return;
        
        EnemyMeleeWeaponSO enemyRangedWeaponSO = enemyWeaponSO as EnemyMeleeWeaponSO;
        Gizmos.color = new Color(1, 0, 0, 0.8f);
        Matrix4x4 matrix = Matrix4x4.TRS(attackPoint.position + attackPoint.TransformVector(enemyRangedWeaponSO.attackOffset), attackPoint.rotation, Vector3.one);
        Gizmos.matrix = matrix;
        
        Gizmos.DrawWireCube(Vector3.zero, enemyRangedWeaponSO.attackRange);
    }
    #endif

    public override void ChangeToDieState()
    {
        StateMachine.ChangeState(DieState);
    }

    public override void ChangeToHitState()
    {
        StateMachine.ChangeState(HitState);
    }

    public override void ChangeToIdleState()
    {
        StateMachine.ChangeState(IdleState);
    }
}
