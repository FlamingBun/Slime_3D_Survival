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

public abstract class Enemy : MonoBehaviour,IDamageable
{
    protected EnemySO enemySO;
    protected EnemyWeaponSO enemyWeaponSO;
    protected EnemyController enemyController;
    protected EnemyAnimationController enemyAnimationController;
    protected EnemyCondition enemyCondition;
    protected EnemyStateMachine stateMachine;

    public bool IsPowerUP { get; protected set; }
    [SerializeField]protected GameObject powerUpEffect;
    [SerializeField]protected Transform attackPoint;
    
    public Player Target { get; protected set; }
    public bool IsAttacking { get; protected set; }

    public EnemyStateMachine StateMachine { get { return stateMachine; } }


    public bool ReadyToWander { get; protected set; }
    public float WanderDistance { get; protected set; }
    public float TraceDistance { get; protected set; }
    public float AttackDistance { get; protected set; }


    public float MinWanderWaitTime { get; protected set; }
    public float MaxWanderWaitTime { get; protected set; }
    
    #region Actions
    public event Action OnIdle;
    public event Action OnMove;
    public event Action OnRun;
    public event Action OnAttack;
    public event Action OnHit;
    public event Action OnDie;
    #endregion Actions
    
    
    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        
        InitializeStates();
        
        enemyController = GetComponent<EnemyController>();
        enemyAnimationController = GetComponent<EnemyAnimationController>();
        enemyCondition = GetComponent<EnemyCondition>();
    }

    protected abstract void InitializeStates();

    public abstract void Initialize(EnemySO _enemySO);

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
        if (IsPowerUP != EnemyManager.Instance.dayNightCycle.InNight)
        {
            IsPowerUP = EnemyManager.Instance.dayNightCycle.InNight;
            powerUpEffect.SetActive(IsPowerUP);
        }
    }

    public void StopNavigation()
    {
        enemyController.StopNavigation();
    }
    
    public bool FindTarget(float maxDistance)
    {
        if (Target == null)
        {
            Target = CharacterManager.Instance.Player;
            return false;
        }
        
        float targetDistace = Vector3.Distance(transform.position, Target.transform.position);
        if (targetDistace < maxDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TraceTarget()
    {
        enemyController.SetTargetPosition(Target.transform.position);
    }

    public bool IsFarFromSpawn()
    {
        return enemyController.IsFarFromSpawn();
    }

    public virtual void BackToOriginPosition()
    {
        enemyController.BackToOriginPosition();
    }

    public bool ArriveTargetPosition()
    {
        return enemyController.ArriveTargetPosition();
    }

    public void Wander()
    {
        enemyController.WanderNewLocation();
    }

    public abstract void Attack();

    public void Respawn()
    {
        IsAttacking = false;
        EnemyManager.Instance.RespawnEnemy(enemySO);
        this.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
    }
#endif

    public void CallIdleEvent()
    {
        OnIdle?.Invoke();
    }

    public void CallMoveEvent()
    {
        OnMove?.Invoke();
    }

    public void CallRunEvent()
    {
        OnRun?.Invoke();
    }

    public void CallAttackEvent()
    {
        OnAttack?.Invoke();
    }

    public void CallHitEvent()
    {
        OnHit?.Invoke();
    }
    
    public void CallDieEvent()
    {
        OnDie?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        enemyCondition.TakeDamage(damage);
    }

    public abstract void ChangeToIdleState();
    public abstract void ChangeToHitState();
    public abstract void ChangeToDieState();
    

    protected IEnumerator AttackCoolDown(float time)
    {
        IsAttacking = true;
        yield return new WaitForSeconds(time);
        IsAttacking = false;
    }

    protected void SetAttackAnimationSpeed(float attackRate)
    {
        enemyAnimationController.SetAttackAnimationSpeed(attackRate);
    }
}
