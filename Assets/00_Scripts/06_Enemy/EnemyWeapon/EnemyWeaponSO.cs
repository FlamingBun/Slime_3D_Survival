using UnityEngine;

public abstract class EnemyWeaponSO : ScriptableObject
{
    public EnemyWeaponType enemyWeaponType;
    public LayerMask targetLayer;
    
    public float damage;
    
    public float attackRate;
    public float attackDistance;
    public GameObject attackEffect;
    public abstract void Attack(Transform attackPoint);
}
