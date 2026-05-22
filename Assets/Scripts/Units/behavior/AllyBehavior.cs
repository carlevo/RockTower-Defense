using UnityEngine;

public class AllyBehavior : MonoBehaviour, IDamageable
{
    public float HP = 50f;
    [SerializeField] private float vel = 2f;

    [Header("Attack")]
    [SerializeField] public float attackDamage = 200f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 0.5f;

    private float cooldownTimer = 0f;
    private readonly Collider2D[] attackResults = new Collider2D[10];

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private Animator anim;
    private Vector3 fixedScale;
    private SpriteRenderer[] renderers;

    void Awake()
    {
        fixedScale = transform.localScale;
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var r in renderers) r.enabled = false;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        foreach (var r in renderers) r.enabled = true;

        if (waypoints == null || waypoints.Length == 0)
        {
            Route route = FindObjectOfType<Route>();
            if (route != null && route.waypoints.Length > 0)
                InicializarRuta(route.waypoints);
            else
                Debug.LogWarning("[AllyBehavior] No se encontro Route en la escena.");
        }
    }

    public void InicializarRuta(Transform[] puntos)
    {
        waypoints = puntos;
        currentWaypointIndex = waypoints.Length - 1;
        transform.position = waypoints[currentWaypointIndex].position;
    }

    void Update()
    {
        if (waypoints == null || currentWaypointIndex < 0) return;
        MoveTowardsTarget();
        HandleAttack();
    }

    private void HandleAttack()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, attackResults);
        for (int i = 0; i < count; i++)
        {
            IDamageable enemy = attackResults[i].GetComponentInParent<IDamageable>();
            if (enemy == null) continue;
            Component comp = enemy as Component;
            bool hasTag = attackResults[i].CompareTag("Enemy") || (comp != null && comp.CompareTag("Enemy"));
            if (!hasTag) continue;
            enemy.TakeDamage(attackDamage);
            cooldownTimer = attackCooldown;
            break;
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetPos = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, vel * Time.deltaTime);

        UpdateAnimation(targetPos);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
                ReachDestination();
        }
    }

    private void UpdateAnimation(Vector3 target)
    {
        if (anim == null) return;

        Vector3 direction = (target - transform.position).normalized;
        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);
    }

    void LateUpdate()
    {
        transform.localScale = fixedScale;
    }

    private void ReachDestination()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
            Destroy(gameObject);
    }
}
