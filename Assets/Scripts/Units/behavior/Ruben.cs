using System.Collections;
using UnityEngine;

public class Ruben : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private string attackAnimationTrigger = "Attack";

    [Header("Slow")]
    [SerializeField] private float slowDuration = 3f;
    // Factor de velocidad aplicado: 0.25 = 25% de la vel original
    [SerializeField] private float slowFactor = 0.25f;

    [Header("Visual Candado")]
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private float lockYOffset = 0.5f;
    [SerializeField] private int lockSortingOrder = 10;

    private Animator animator;
    private float cooldownTimer;
    private readonly Collider2D[] results = new Collider2D[20];

    private ISlowable currentTarget;
    private GameObject currentLockObj;
    private Coroutine slowCoroutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cooldownTimer = attackCooldown;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        ISlowable nearest = GetNearestSlowableEnemy();
        if (nearest == null)
        {
            Debug.LogWarning($"[Ruben] Timer llegó a 0 pero no encontró enemigo ISlowable en rango {attackRange}");
            cooldownTimer = attackCooldown;
            return;
        }

        EjecutarDebuff(nearest);
        cooldownTimer = attackCooldown;
    }

    private void EjecutarDebuff(ISlowable objetivo)
    {
        if (!string.IsNullOrEmpty(attackAnimationTrigger))
            animator?.SetTrigger(attackAnimationTrigger);

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        QuitarSlowActual();

        currentTarget = objetivo;
        slowCoroutine = StartCoroutine(AplicarSlowConCandado(objetivo));
    }

    private IEnumerator AplicarSlowConCandado(ISlowable objetivo)
    {
        Component comp2 = objetivo as Component;
        Debug.Log($"[Ruben] Aplicando slow x{slowFactor} a {comp2?.gameObject.name}");
        objetivo.ApplySlow(slowFactor);

        Component comp = objetivo as Component;
        if (comp != null && lockSprite != null)
        {
            currentLockObj = new GameObject("LockVisual");
            currentLockObj.transform.SetParent(comp.transform);
            currentLockObj.transform.localPosition = new Vector3(0f, lockYOffset, 0f);
            currentLockObj.transform.localScale = Vector3.one;

            SpriteRenderer sr = currentLockObj.AddComponent<SpriteRenderer>();
            sr.sprite = lockSprite;
            sr.sortingOrder = lockSortingOrder;
        }

        yield return new WaitForSeconds(slowDuration);

        QuitarSlowActual();
    }

    private void QuitarSlowActual()
    {
        if (currentTarget != null)
        {
            Component comp = currentTarget as Component;
            if (comp != null)
                currentTarget.RemoveSlow();
            currentTarget = null;
        }

        if (currentLockObj != null)
        {
            Destroy(currentLockObj);
            currentLockObj = null;
        }
    }

    private ISlowable GetNearestSlowableEnemy()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, results);
        Debug.Log($"[Ruben] OverlapCircle encontró {count} colliders en rango {attackRange}");

        ISlowable nearest = null;
        float nearestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Debug.Log($"[Ruben] Collider[{i}]: {results[i].gameObject.name} | tag: '{results[i].tag}'");

            if (!results[i].CompareTag("Enemy")) continue;

            ISlowable slowable = results[i].GetComponent<ISlowable>();
            if (slowable == null)
            {
                Debug.LogWarning($"[Ruben] {results[i].gameObject.name} tiene tag Enemy pero NO tiene ISlowable");
                continue;
            }

            float dist = Vector2.Distance(transform.position, results[i].transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = slowable;
            }
        }
        return nearest;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
