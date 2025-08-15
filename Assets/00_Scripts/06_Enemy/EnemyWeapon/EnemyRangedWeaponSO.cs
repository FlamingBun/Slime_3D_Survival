using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_RangedWeaponSO_", menuName = "ScriptableObject/Enemy/Enemy_RangedWeapon")]
public class EnemyRangedWeaponSO : EnemyWeaponSO
{
    public GameObject projectile;
    public ParticleEffect projectileParticle;
    
    public float projectileSpeed;
    public float projectileRange;
    
    public override void Attack(Transform attackPoint)
    {
        PoolManager.Instance.ReuseComponent<ParticleEffect>(attackEffect,attackPoint.position, Quaternion.identity).gameObject.SetActive(true);
        Projectile reusedProjectile = PoolManager.Instance.ReuseComponent<Projectile>(projectile, attackPoint.position, Quaternion.identity);
        reusedProjectile.Initialize(this,attackPoint.forward);
    }
}