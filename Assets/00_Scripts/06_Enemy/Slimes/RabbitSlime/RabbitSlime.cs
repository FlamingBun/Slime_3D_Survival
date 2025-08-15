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

public class RabbitSlime : Enemy
{
    public RabbitSlimeIdleState IdleState { get; private set; }
    public RabbitSlimeWanderState WanderState { get; private set; }
    public RabbitSlimeTraceState TraceState { get; private set; }
    public RabbitSlimeReturnState  ReturnState { get; private set; } // 스폰 위치로 되돌아가는 상태 
    public RabbitSlimeAttackState AttackState { get; private set; }
    public RabbitSlimeHitState  HitState { get; private set; }
    public RabbitSlimeDieState DieState { get; private set; }
    public bool IsEnraged { get; private set; }
    [SerializeField]private GameObject enragedEffect;

    private void OnEnable()
    {
        enemyCondition.SubscribeOnHealthChange(CheckHealthChange);
    }

    private void OnDisable()
    {
        enemyCondition.UnSubscribeOnHealthChange(CheckHealthChange);
    }

    private void CheckHealthChange(float currentHealth, float maxHealth)
    {
        if (!IsEnraged&&currentHealth <= maxHealth / 2)
        {
            IsEnraged = true;
            enragedEffect.SetActive(true);
            SetAttackAnimationSpeed(enemyWeaponSO.attackRate/EnemySettings.enragedSpeed);
            enemyController.SetMoveSpeedMultiplier(EnemySettings.enragedSpeed);
        }
    }

    protected override void InitializeStates()
    {
        // enemy States
        IdleState = new RabbitSlimeIdleState(this, stateMachine);
        WanderState = new RabbitSlimeWanderState(this, stateMachine);
        TraceState = new RabbitSlimeTraceState(this, stateMachine);
        ReturnState = new RabbitSlimeReturnState(this, stateMachine);
        AttackState = new RabbitSlimeAttackState(this, stateMachine);
        HitState = new RabbitSlimeHitState(this, stateMachine);
        DieState = new RabbitSlimeDieState(this, stateMachine);
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
        IsEnraged = false;
        enragedEffect.SetActive(false);

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
            StartCoroutine(AttackCoolDown(IsEnraged?enemyWeaponSO.attackRate/EnemySettings.enragedSpeed:enemyWeaponSO.attackRate));
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
        
        EnemyMeleeWeaponSO enemyMeleeWeaponSO = enemyWeaponSO as EnemyMeleeWeaponSO;
        Gizmos.color = new Color(1, 0, 0, 0.8f);
        Matrix4x4 matrix = Matrix4x4.TRS(attackPoint.position + attackPoint.TransformVector(enemyMeleeWeaponSO.attackOffset), attackPoint.rotation, Vector3.one);
        Gizmos.matrix = matrix;
        
        Gizmos.DrawWireCube(Vector3.zero, enemyMeleeWeaponSO.attackRange);
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
