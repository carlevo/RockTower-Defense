using UnityEngine;

public class EnemyMovement : MonoBehaviour, IDamageable
{
    [Header("Configuración")]
    [SerializeField] private float vel = 2f;
    public float hp = 100f;
    public float damage = 10f;

    private Transform[] targetWaypoints;
    private int currentWaypointIndex = 0;
    private Animator anim; // Para las animaciones de giro

    private Vector3 fixedScale;


    void Start()
    {
        anim = GetComponent<Animator>();

        fixedScale = transform.localScale;
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

        // Calculamos la dirección (destino - posición actual)
        Vector3 direction = (target - transform.position).normalized;

        // Enviamos los valores al Blend Tree
        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);

        // mire a la derecha/izquierda haciendo espejo:
        if (direction.x > 0.1f) transform.localScale = new Vector3(-1, 1, 1); // Derecha
        else if (direction.x < -0.1f) transform.localScale = new Vector3(1, 1, 1); // Izquierda
        Debug.Log(direction);
    }

    void LateUpdate()
    {
        // Esto obliga al enemigo a mantener su tamaño original 
        // ignorando lo que diga el clip de animación "Read Only"

        float directionSign = transform.localScale.x > 0 ? 1f : -1f;

        transform.localScale = new Vector3(
            fixedScale.x,
            fixedScale.y,
            fixedScale.z
        );
    }

    // Dentro de tu EnemyMovement.cs

    public void TakeDamage(float amount)
{
    hp -= amount;
    if (hp <= 0)
    {
        // ESTA ES LA LÍNEA IMPORTANTE
        if(WaveSpawner.Instance != null) 
            WaveSpawner.Instance.RegistrarMuerteEnemigo();
        
        Destroy(gameObject);
    }
}

    private void ReachEnd()
    {
        RocaHandler.Instance.TakeDamage(damage);

        // AUNQUE NO MUERA POR TORRE, CUENTA COMO ENEMIGO QUE "SALIÓ" DE LA ESCENA
        if (WaveSpawner.Instance != null) WaveSpawner.Instance.RegistrarMuerteEnemigo();

        Destroy(gameObject);
    }


}