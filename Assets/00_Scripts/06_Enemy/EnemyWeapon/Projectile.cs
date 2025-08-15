using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private EnemyRangedWeaponSO enemyRangedWeaponSO;

    private Vector3 direction;
    private float range;
    
    public void Initialize(EnemyRangedWeaponSO _enemyRangedWeaponSO, Vector3 _direction)
    {
        enemyRangedWeaponSO = _enemyRangedWeaponSO;
        direction = _direction;
        range = enemyRangedWeaponSO.projectileRange;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        Vector3 distanceVector = direction * enemyRangedWeaponSO.projectileSpeed * Time.deltaTime;

        transform.position += distanceVector;

        // Disable after max range reached
        range -= distanceVector.magnitude;

        if (range < 0f)
        {
            DisableProjectile();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (layerMask.value == (layerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.GetComponent<IDamageable>()?.TakeDamage(EnemyManager.Instance.dayNightCycle.InNight?enemyRangedWeaponSO.damage*2f:enemyRangedWeaponSO.damage);
        }

        // TODO: 피격 사운드 재생
        DisableProjectile();
    }
    
    private void DisableProjectile()
    {
        PoolManager.Instance.ReuseComponent<ParticleEffect>(enemyRangedWeaponSO.projectileParticle.gameObject, transform.position, Quaternion.identity).gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
