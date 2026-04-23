using UnityEngine;

public class Goku : MonoBehaviour
{
    public GameObject kamekamehaPrefab;
    public GameObject chargeAttackPrefab;
    public ParticleSystem attackParticle;
    public float range = 3f;
    public float chargeTime = 0.3f;
    public float laserScaleSpeed = 8f;
    public float damage = 0.2f;
    public float incrementDamagebyTime = 0.6f;
    float maxDamage = 6f;

    enum State { Idle, Charging, Attacking }
    State state = State.Idle;

    GameObject enemyTarget;
    GameObject chargeInstance;
    GameObject laserInstance;
    float chargeTimer;

    private readonly Collider2D[] detectionResults = new Collider2D[10];

    void Start()
    {
        if (attackParticle != null)
            attackParticle.gameObject.SetActive(false);
    }

    void Update()
    {
        EnemyDetectionPoint();

        switch (state)
        {
            case State.Charging: UpdateCharging(); break;
            case State.Attacking: UpdateAttacking(); break;
        }
    }

    void EnemyDetectionPoint()
    {
        GameObject closest = FindClosestEnemy();

        if (closest == null)
        {
            if (state != State.Idle)
            {
                Debug.Log("[Goku] Sin enemigos en rango, perdiendo target.");
                enemyTarget = null;
                StopAll();
            }
            return;
        }

        if (closest != enemyTarget)
        {
            enemyTarget = closest;
            float dist = Vector2.Distance(transform.position, enemyTarget.transform.position);
            Debug.Log($"[Goku] Enemigo detectado: {enemyTarget.name} a {dist:F2} unidades.");
            StartCharging();
        }
    }

    GameObject FindClosestEnemy()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, range, detectionResults);
        GameObject closest = null;
        float closestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            if (detectionResults[i] == null) continue;
            if (detectionResults[i].GetComponent<Teclaenemy>() == null) continue;
            float dist = Vector2.Distance(transform.position, detectionResults[i].transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = detectionResults[i].gameObject;
            }
        }
        return closest;
    }

    void StartCharging()
    {
        StopAll();
        state = State.Charging;
        chargeTimer = chargeTime;

        if (chargeAttackPrefab != null)
            chargeInstance = Instantiate(chargeAttackPrefab, transform.position, Quaternion.identity, transform);

        if (attackParticle != null)
            attackParticle.gameObject.SetActive(false);
    }

    void UpdateCharging()
    {
        if (enemyTarget == null) { StopAll(); return; }

        chargeTimer -= Time.deltaTime;
        if (chargeTimer <= 0f)
            StartAttacking();
    }

    void StartAttacking()
    {
        if (chargeInstance != null) { Destroy(chargeInstance); chargeInstance = null; }
        if (enemyTarget == null) { state = State.Idle; return; }

        state = State.Attacking;

        // El sprite del láser debe tener el pivot en la base (Y=0 local) apuntando hacia arriba
        Vector3 dir = (enemyTarget.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        laserInstance = Instantiate(kamekamehaPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
        laserInstance.transform.localScale = new Vector3(laserInstance.transform.localScale.x, 0f, laserInstance.transform.localScale.z);

        if (attackParticle != null)
            attackParticle.gameObject.SetActive(true);
    }

    void UpdateAttacking()
    {
        if (enemyTarget == null || laserInstance == null)
        {
            StopAll();
            return;
        }

        // Mantener posición y dirección hacia el enemigo
        laserInstance.transform.position = transform.position;
        Vector3 dir = (enemyTarget.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        laserInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Escalar Y hasta alcanzar la distancia al enemigo (colisión implícita)
        float dist = Vector2.Distance(transform.position, enemyTarget.transform.position);
        float currentY = laserInstance.transform.localScale.y;

        if (currentY < dist)
        {
            float newY = Mathf.MoveTowards(currentY, dist, laserScaleSpeed * Time.deltaTime);
            laserInstance.transform.localScale = new Vector3(laserInstance.transform.localScale.x, newY, laserInstance.transform.localScale.z);
        }
    }

    void StopAll()
    {
        if (chargeInstance != null) { Destroy(chargeInstance); chargeInstance = null; }
        if (laserInstance != null) { Destroy(laserInstance); laserInstance = null; }

        if (attackParticle != null)
            attackParticle.gameObject.SetActive(false);

        state = State.Idle;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
