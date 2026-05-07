using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    [SerializeField] public UnitData unitData;

    private float attackRange;
    private float attackDamage;
    private float attackCooldown;
    private string attackAnimTrigger;

    private float cooldownTimer = 0f;
    private Animator animator;
    private readonly Collider2D[] results = new Collider2D[20];

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (unitData != null)
        {
            attackRange = unitData.attackRange;
            attackDamage = unitData.attackDamage;
            attackCooldown = unitData.attackCooldown;
            attackAnimTrigger = unitData.attackAnimationTrigger;
        }
        else
        {
            attackRange = 2f;
            attackDamage = 20f;
            attackCooldown = 1f;
            attackAnimTrigger = "Attack";
            Debug.LogWarning($"[UnitAttack] {gameObject.name} no tiene UnitData asignado.");
        }
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        IDamageable nearest = GetNearestEnemy();
        if (nearest == null) return;

        nearest.TakeDamage(attackDamage);
        if (!string.IsNullOrEmpty(attackAnimTrigger))
            animator?.SetTrigger(attackAnimTrigger);
        cooldownTimer = attackCooldown;
    }

    private IDamageable GetNearestEnemy()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, results);
        IDamageable nearest = null;
        float nearestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            if (!results[i].CompareTag("Enemy")) continue;

            IDamageable enemy = results[i].GetComponent<IDamageable>();
            if (enemy == null) continue;

            float dist = Vector2.Distance(transform.position, results[i].transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void OnDrawGizmosSelected()
    {
        float range = (unitData != null) ? unitData.attackRange : 2f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
