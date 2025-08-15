using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_MeleeWeaponSO_", menuName = "ScriptableObject/Enemy/Enemy_MeleeWeapon")]
public class EnemyMeleeWeaponSO : EnemyWeaponSO
{
    public Vector3 attackRange;
    public Vector3 attackOffset;
    public override void Attack(Transform attackPoint)
    {
        Vector3 worldCenter = attackPoint.position + attackPoint.TransformVector(attackOffset);
        Vector3 halfExtents = attackRange / 2;
        
        Collider[] hitColliders = Physics.OverlapBox(worldCenter, halfExtents, Quaternion.identity,targetLayer);
        
        PoolManager.Instance.ReuseComponent<ParticleEffect>(attackEffect,worldCenter, Quaternion.identity).gameObject.SetActive(true);
        foreach (Collider collider in hitColliders)
        {
            IDamageable hitPlayer = collider.GetComponent<IDamageable>();
            if (hitPlayer != null)
            {
                hitPlayer.TakeDamage(EnemyManager.Instance.dayNightCycle.InNight?damage*2f:damage);
            }
        }
    }
}
