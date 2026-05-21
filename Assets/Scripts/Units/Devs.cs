using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devs : MonoBehaviour
{
    // ── LANZADOR ──────────────────────────────────────────────────────────────
    [Header("Lanzador")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Sprite spriteProyectil;
    [SerializeField] private float launchRange = 5f;
    [SerializeField] private float launchCooldown = 2.5f;
    [SerializeField] private float launchDamage = 15f;

    // ── DEBUFF AOE ────────────────────────────────────────────────────────────
    [Header("Debuff AOE")]
    [SerializeField] private float debuffRange = 4f;
    [SerializeField] private float debuffCooldown = 6f;
    [SerializeField] private float slowFactor = 0.4f;
    [SerializeField] private float slowDuration = 3f;
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private float lockYOffset = 0.5f;
    [SerializeField] private int lockSortingOrder = 10;

    // ── SUMMON ────────────────────────────────────────────────────────────────
    [Header("Summon")]
    [SerializeField] private List<GameObject> prefabsToSummon;
    [SerializeField] private float summonCooldown = 15f;
    [SerializeField] private float summonDelay = 5f;
    [SerializeField] private float delayTrasAnimacion = 0.5f;

    // ── INTERNO ───────────────────────────────────────────────────────────────
    private Animator animator;
    private float launchTimer;
    private float debuffTimer;
    private readonly Collider2D[] results = new Collider2D[20];
    private readonly Dictionary<ISlowable, GameObject> slowedEnemies = new Dictionary<ISlowable, GameObject>();
    private Coroutine slowCoroutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        launchTimer = launchCooldown;
        debuffTimer = debuffCooldown;
    }

    void Start()
    {
        if (prefabsToSummon != null && prefabsToSummon.Count > 0)
            InvokeRepeating(nameof(TriggerSummon), summonDelay, summonCooldown);
        else
            Debug.LogWarning("[Devs] No hay prefabs en prefabsToSummon.");
    }

    void Update()
    {
        launchTimer -= Time.deltaTime;
        debuffTimer -= Time.deltaTime;

        if (launchTimer <= 0f)
        {
            IDamageable target = GetNearestEnemy(launchRange);
            if (target != null)
            {
                LaunchProjectile(target);
                launchTimer = launchCooldown;
            }
        }

        if (debuffTimer <= 0f)
        {
            EjecutarDebuffAOE();
            debuffTimer = debuffCooldown;
        }
    }

    // ── LANZADOR ──────────────────────────────────────────────────────────────
    private void LaunchProjectile(IDamageable target)
    {
        animator?.SetTrigger("Attack");

        if (proyectilPrefab == null) return;

        Component comp = target as Component;
        if (comp == null) return;

        GameObject obj = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
        Proyectil p = obj.GetComponent<Proyectil>();
        p?.Configurar(comp.transform, launchDamage, spriteProyectil);
    }

    // ── DEBUFF AOE ────────────────────────────────────────────────────────────
    private void EjecutarDebuffAOE()
    {
        animator?.SetTrigger("Attack");

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        QuitarTodosLosSlows();

        int count = Physics2D.OverlapCircleNonAlloc(transform.position, debuffRange, results);
        for (int i = 0; i < count; i++)
        {
            if (!results[i].CompareTag("Enemy")) continue;

            ISlowable slowable = results[i].GetComponent<ISlowable>();
            if (slowable == null) continue;

            slowable.ApplySlow(slowFactor);

            GameObject lockObj = null;
            if (lockSprite != null)
            {
                lockObj = new GameObject("LockVisual");
                lockObj.transform.SetParent(results[i].transform);
                lockObj.transform.localPosition = new Vector3(0f, lockYOffset, 0f);
                lockObj.transform.localScale = Vector3.one;
                SpriteRenderer sr = lockObj.AddComponent<SpriteRenderer>();
                sr.sprite = lockSprite;
                sr.sortingOrder = lockSortingOrder;
            }
            slowedEnemies[slowable] = lockObj;
        }

        if (slowedEnemies.Count > 0)
            slowCoroutine = StartCoroutine(QuitarSlowDespuesDe(slowDuration));
    }

    private IEnumerator QuitarSlowDespuesDe(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        QuitarTodosLosSlows();
    }

    private void QuitarTodosLosSlows()
    {
        foreach (var kvp in slowedEnemies)
        {
            Component comp = kvp.Key as Component;
            if (comp != null) kvp.Key.RemoveSlow();
            if (kvp.Value != null) Destroy(kvp.Value);
        }
        slowedEnemies.Clear();
    }

    // ── SUMMON ────────────────────────────────────────────────────────────────
    private void TriggerSummon()
    {
        StartCoroutine(AnimarYInvocar());
    }

    private IEnumerator AnimarYInvocar()
    {
        animator?.SetTrigger("Attack");
        yield return new WaitForSeconds(delayTrasAnimacion);

        Route route = FindObjectOfType<Route>();
        Vector3 spawnPos = (route != null && route.waypoints.Length > 0)
            ? route.waypoints[route.waypoints.Length - 1].position
            : transform.position;

        int index = Random.Range(0, prefabsToSummon.Count);
        GameObject instancia = Instantiate(prefabsToSummon[index], spawnPos, Quaternion.identity);

        AllyBehavior ally = instancia.GetComponent<AllyBehavior>();
        if (ally != null && route != null)
            ally.InicializarRuta(route.waypoints);
    }

    // ── UTILIDADES ────────────────────────────────────────────────────────────
    private IDamageable GetNearestEnemy(float range)
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, range, results);
        IDamageable nearest = null;
        float nearestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            IDamageable enemy = results[i].GetComponentInParent<IDamageable>();
            if (enemy == null) continue;

            Component comp = enemy as Component;
            bool hasTag = results[i].CompareTag("Enemy") || (comp != null && comp.CompareTag("Enemy"));
            if (!hasTag) continue;

            float dist = Vector2.Distance(transform.position, comp.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, launchRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, debuffRange);
    }
}
