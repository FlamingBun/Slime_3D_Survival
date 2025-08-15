using System;
using System.Collections;
using UnityEngine;

public class EnemyCondition : MonoBehaviour
{
    private Enemy enemy;
    
    #region Health
    private float currentHealth;
    private float maxHealth;
    private event Action<float, float> OnHealthChange;
    #endregion Health
    
    #region Hit
    private float hitRecoveryTime;
    private float currentHitRecoveryTime;
    private bool isHit = false;
    private bool isDead = false;
    #endregion Hit
    
    public void Initialize(Enemy _enemy, float _maxHealth, float _hitRecoveryTime)
    {
        enemy = _enemy;
        isHit = false;
        isDead = false;
        hitRecoveryTime = _hitRecoveryTime;
        currentHealth = _maxHealth;
        maxHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead||isHit) return;
        
        currentHealth -= damage;
        OnHealthChange?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        { 
            isDead = true;
            enemy.ChangeToDieState();
            FindObjectOfType<QuestManager>().AddProgress(2, 1); // 적 퀘스트 진행도 증가
            return;
        }
        
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        isHit = true;
        enemy.ChangeToHitState();
        currentHitRecoveryTime = hitRecoveryTime;
        while (currentHitRecoveryTime > 0)
        {
            currentHitRecoveryTime -= Time.deltaTime;
            yield return null;
        }
        enemy.ChangeToIdleState();
        
        isHit = false;
    }

    
    public void SubscribeOnHealthChange(Action<float, float> action)
    {
        OnHealthChange += action;
    }

    public void UnSubscribeOnHealthChange(Action<float, float> action)
    {
        OnHealthChange -= action;
    }

}