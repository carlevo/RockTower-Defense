using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float vel = 2f;
    public float hp = 100f;
    public float damage = 10f;

    private Transform[] targetWaypoints;
    private int currentWaypointIndex = 0;
    private Animator anim; // Para las animaciones de giro

    void Start()
    {
        anim = GetComponent<Animator>();
        
        // Buscamos la ruta en la escena (puedes pasarla por el Spawner también)
        Route route = FindObjectOfType<Route>();
        if (route != null)
        {
            targetWaypoints = route.waypoints;
        }
    }

    void Update()
    {
        if (targetWaypoints == null || currentWaypointIndex >= targetWaypoints.Length) return;

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        // 1. Moverse hacia el punto actual
        Vector3 targetPos = targetWaypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, vel * Time.deltaTime);

        // 2. Cambiar de animación según la dirección (Opcional)
        UpdateAnimation(targetPos);

        // 3. ¿Hemos llegado al punto?
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentWaypointIndex++;
            
            // Si era el último punto, daña a la roca
            if (currentWaypointIndex >= targetWaypoints.Length)
            {
                ReachEnd();
            }
        }
    }

    private void UpdateAnimation(Vector3 target)
    {
        if (anim == null) return;

        Vector3 direction = (target - transform.position).normalized;
        
        // Ejemplo simple: Cambiar parámetros del Animator
        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);
    }

    private void ReachEnd()
    {
        RocaHandler.Instance.TakeDamage(damage);
        // Aquí podrías activar el efecto visual "hitPLayerGm" que tenías
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            RocaHandler.Instance.RegisterKill();
            Destroy(gameObject);
        }
    }
}