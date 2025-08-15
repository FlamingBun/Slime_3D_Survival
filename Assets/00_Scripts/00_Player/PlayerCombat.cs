using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageable
{
    void TakeDamage(float damage);
}
public class PlayerCombat : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    protected Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition thirst { get { return uiCondition.thirst; } }
    Condition bodyTemp { get { return uiCondition.bodyTemp; } }

    private bool isInCombat=false;
    public int attackPower;
    public int defence;

    public float attackSpeed;
    public float attackCooldown;
    private bool canAttack = true;
    public GameObject targetEnemy;
    public Vector2 enemyDistance;
    public float lastHitTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health.Subtract(damage);
        GetComponent<PlayerCondition>().CallTakeDamageEvent();
        //onTakeDamage?.Invoke();
    }
}
