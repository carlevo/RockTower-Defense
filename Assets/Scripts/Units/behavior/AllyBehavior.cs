using UnityEngine;

public class AllyBehavior : MonoBehaviour, IDamageable
{
    public float HP = 50f;
    public float DMG = 10f;
    [SerializeField] private float vel = 2f;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private Animator anim;
    private Vector3 fixedScale;

    void Start()
    {
        anim = GetComponent<Animator>();
        fixedScale = transform.localScale;

        Route route = FindObjectOfType<Route>();
        if (route != null && route.waypoints.Length > 0)
        {
            waypoints = route.waypoints;
            currentWaypointIndex = 0;
        }
        else
        {
            Debug.LogWarning("[AllyBehavior] No se encontro Route en la escena.");
        }
    }

    void Update()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Length) return;
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetPos = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, vel * Time.deltaTime);

        UpdateAnimation(targetPos);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                ReachPlayerBase();
        }
    }

    private void UpdateAnimation(Vector3 target)
    {
        if (anim == null) return;

        Vector3 direction = (target - transform.position).normalized;
        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);

        if (direction.x > 0.1f) transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x < -0.1f) transform.localScale = new Vector3(1, 1, 1);
    }

    void LateUpdate()
    {
        transform.localScale = new Vector3(
            Mathf.Sign(transform.localScale.x) * Mathf.Abs(fixedScale.x),
            fixedScale.y,
            fixedScale.z
        );
    }

    private void ReachPlayerBase()
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
