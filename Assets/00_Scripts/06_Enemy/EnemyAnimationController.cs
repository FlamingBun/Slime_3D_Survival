
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    #region Animation Hash
    private readonly int isIdle = Animator.StringToHash("IsIdle");
    private readonly int isMoving = Animator.StringToHash("IsMoving");
    private readonly int isAttacking = Animator.StringToHash("IsAttacking");
    private readonly int isHit = Animator.StringToHash("IsHit");
    private readonly int isDying = Animator.StringToHash("IsDying");
    #endregion Animation Hash
    
    
    private Animator animator;
    private Enemy enemy;
    private EnemyController enemyController;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        enemyController = GetComponent<EnemyController>();
    }

    private void OnEnable()
    {
        enemy.OnIdle += SetIdleAnimation;
        enemy.OnMove += SetMoveAnimation;
        enemy.OnRun += SetRunAnimation;
        enemy.OnAttack += SetAttackAnimation;
        enemy.OnHit += SetHitAnimation;
        enemy.OnDie += SetDieAnimation;
    }

    private void OnDisable()
    {
        enemy.OnIdle -= SetIdleAnimation;
        enemy.OnMove -= SetMoveAnimation;
        enemy.OnAttack -= SetAttackAnimation;
        enemy.OnHit -= SetHitAnimation;
        enemy.OnDie -= SetDieAnimation;
    }

    private void SetIdleAnimation()
    {
        SetInitializeAnimation();
        animator.SetBool(isIdle, true);
    }
    
    private void SetMoveAnimation()
    {
        SetInitializeAnimation();
        animator.SetBool(isMoving, true);
    }

    private void SetRunAnimation()
    {
        SetInitializeAnimation();
        animator.SetBool(isMoving, true);
        animator.speed = enemyController.GetRunAnimationSpeed();
    }

    private void SetAttackAnimation()
    {
        SetInitializeAnimation();
        animator.SetTrigger(isAttacking);
    }

    private void SetHitAnimation()
    {
        SetInitializeAnimation();
        animator.SetTrigger(isHit);
    }

    private void SetDieAnimation()
    {
        SetInitializeAnimation();
        animator.SetTrigger(isDying);
    }

    private void SetInitializeAnimation()
    {
        animator.SetBool(isIdle, false);
        animator.SetBool(isMoving, false);
        animator.speed = enemyController.GetWalkAnimationSpeed();
    }

    public void SetAttackAnimationSpeed(float attackRate)
    {
        float attackSpeed = Mathf.Clamp(1f / attackRate, 0.5f, 2f);
        animator.SetFloat("AttackSpeed", attackSpeed);
    }
}