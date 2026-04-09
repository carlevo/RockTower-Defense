using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackCooldown = 1f;

    private float cooldownTimer = 0f;
    private Animator animator;
    private readonly Collider2D[] results = new Collider2D[10];

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, results);
        for (int i = 0; i < count; i++)
        {
            Teclaenemy enemy = results[i].GetComponent<Teclaenemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                animator?.SetTrigger("Attack");
                cooldownTimer = attackCooldown;
                break; // un golpe por swing
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
