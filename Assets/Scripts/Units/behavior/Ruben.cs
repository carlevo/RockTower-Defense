using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruben : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private string attackAnimationTrigger = "Attack";

    [Header("Slow")]
    [SerializeField] private float slowDuration = 3f;
    [SerializeField] private float slowFactor = 0.25f;

    [Header("Visual Candado")]
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private float lockYOffset = 0.5f;
    [SerializeField] private int lockSortingOrder = 10;

    private Animator animator;
    private float cooldownTimer;
    private readonly Collider2D[] results = new Collider2D[20];
    private readonly Dictionary<ISlowable, GameObject> slowedEnemies = new Dictionary<ISlowable, GameObject>();
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

        EjecutarDebuffAOE();
        cooldownTimer = attackCooldown;
    }

    private void EjecutarDebuffAOE()
    {
        if (!string.IsNullOrEmpty(attackAnimationTrigger))
            animator?.SetTrigger(attackAnimationTrigger);

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        QuitarTodosLosSlows();

        int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, results);

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
            if (comp != null)
                kvp.Key.RemoveSlow();

            if (kvp.Value != null)
                Destroy(kvp.Value);
        }
        slowedEnemies.Clear();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
