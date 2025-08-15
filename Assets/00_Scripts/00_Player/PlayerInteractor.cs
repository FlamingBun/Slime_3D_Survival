using System;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private readonly int isAttack = Animator.StringToHash("IsAttack");
    public PlayerEquipment equipment;
    
    public float interactRange = 3f; // 상호작용 거리
    public ToolType equippedTool = ToolType.Axe; // 현재 장착한 도구 (예시)

    public LayerMask enemyLayer;
    public Transform attackPoint;
    public Transform slashPoint;
    
    public Animator animator;
    public ParticleEffect slashEffect;
    public ParticleEffect hitEffect;
    
    
    // TODO: 장비 제작되면 -> 장비의 옵션으로 변경
    public float damage;
    public Vector3 attackOffset;
    public Vector3 attackRange;

    private void Awake()
    {
        equipment = GetComponent<PlayerEquipment>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PoolManager.Instance.CreatePool<ParticleEffect>(slashEffect.gameObject, slashEffect.poolSize);
        PoolManager.Instance.CreatePool<ParticleEffect>(hitEffect.gameObject, hitEffect.poolSize);
    }

    void Update()
    {
        if (UIManager.Instance.UIStack.Count > 0)
        {
            return;
        }

        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        //Debug.DrawRay(ray.origin, ray.direction * 30f, Color.red);
        //if (Input.GetMouseButton(0))
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            equippedTool = ToolType.Axe;
            equipment.EquipNew(ToolType.Axe);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            equippedTool = ToolType.Pickaxe;
            equipment.EquipNew(ToolType.Pickaxe);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            equippedTool = ToolType.Sword;
            equipment.EquipNew(ToolType.Sword);
        }

        if(CharacterManager.Instance.Player.condition.uiCondition.stamina.curValue >= 5f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger(isAttack);
                
                GameObject particle = PoolManager.Instance.ReuseComponent<ParticleEffect>(
                    slashEffect.gameObject,
                    attackPoint.position + attackPoint.TransformVector(attackOffset),
                    attackPoint.rotation
                ).gameObject;
                particle.transform.parent = slashPoint;
                particle.gameObject.SetActive(true);
            }
        }
        
        
    }

    public void Swing()
    {
        if (CharacterManager.Instance.Player.condition.UseStamina(5))
        {
            switch (equippedTool)
            {
                case ToolType.Axe:
                case ToolType.Pickaxe:
                    TryInteract();
                    break;
                case ToolType.Sword:
                    TryAttack();
                    break;
            }
        }
        
    }

    private void TryInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            ResourceObject resource = hit.collider.GetComponent<ResourceObject>();
            if (resource != null)
            {
                resource.Interact(equippedTool);
                Vector3 effectPos = hit.transform.position;
                PoolManager.Instance.ReuseComponent<ParticleEffect>(
                    hitEffect.gameObject,
                    effectPos,
                    Quaternion.identity
                ).gameObject.SetActive(true);
            }
        }
    }

    private void TryAttack()
    {
        Vector3 worldCenter = attackPoint.position + attackPoint.TransformVector(attackOffset);
        Vector3 halfExtents = attackRange / 2;
        
        Collider[] hitColliders = Physics.OverlapBox(worldCenter, halfExtents, attackPoint.rotation, enemyLayer);
        
        foreach (Collider collider in hitColliders)
        {
            IDamageable hitEnemy = collider.GetComponent<IDamageable>();
            if (hitEnemy != null)
            {
                Vector3 hitPos = collider.ClosestPoint(worldCenter); // 또는 collider.transform.position
                PoolManager.Instance.ReuseComponent<ParticleEffect>(
                    hitEffect.gameObject,
                    hitPos,
                    Quaternion.identity
                ).gameObject.SetActive(true);
                hitEnemy.TakeDamage(damage);
            }
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (equippedTool != ToolType.Sword) return;
        
        Gizmos.color = new Color(1, 0, 0, 0.6f);
        Matrix4x4 matrix = Matrix4x4.TRS(attackPoint.position + attackPoint.TransformVector(attackOffset), attackPoint.rotation, Vector3.one);

        Gizmos.matrix = matrix;

        Gizmos.DrawWireCube(Vector3.zero, attackRange);
    }
#endif
}
