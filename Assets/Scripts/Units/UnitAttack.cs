using UnityEngine;
using System.Collections;

public class UnitAttack : MonoBehaviour
{
    [Header("Referencia de Datos")]
    public UnitData unitData; 

    [Header("Ajustes de Proyectil (Solo para DPS)")]
    [SerializeField] private GameObject proyectilPrefab; 
    [SerializeField] private Sprite spriteProyectil;    

    [Header("Ajustes de Rayo (Solo para CD)")]
    [SerializeField] private GameObject rayoPrefab; // El prefab base del rayo (el mismo para todas)
    [SerializeField] private RuntimeAnimatorController animacionRayo;
    [SerializeField] private Sprite spriteRayo; // ¡NUEVO: Para rayos de una sola imagen!

    [Header("Ajustes de Slow (Solo para debuffers)")]
    [SerializeField] private Sprite spriteStun; //El sprite del stuneo

    private float cooldownTimer = 0f;
    private Animator animator;
    private readonly Collider2D[] results = new Collider2D[20];

    void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    public void InicializarUnidad(UnitData nuevosDatos)
    {
        unitData = nuevosDatos;
        cooldownTimer = unitData.attackCooldown;
        Debug.Log($"[UnitAttack] {gameObject.name} inicializado. Rango: {unitData.attackRange}");
    }

    void Update()
    {
        if (unitData == null) return;

        cooldownTimer -= Time.deltaTime;

        IDamageable nearest = GetNearestEnemy();
        if (nearest == null) return;

        if (cooldownTimer <= 0f)
        {
            EjecutarLogicaAtaque(nearest);
            cooldownTimer = unitData.attackCooldown;
        }
    }

    private void EjecutarLogicaAtaque(IDamageable objetivo)
    {
        if (!string.IsNullOrEmpty(unitData.attackAnimationTrigger))
            animator?.SetTrigger(unitData.attackAnimationTrigger);

        // --- LÓGICA DPS ---
        if (gameObject.CompareTag("DPS"))
        {
            if (proyectilPrefab != null)
            {
                GameObject obj = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
                Proyectil p = obj.GetComponent<Proyectil>();
                Component targetComp = objetivo as Component;

                if (p != null && targetComp != null)
                {
                    p.Configurar(targetComp.transform, unitData.attackDamage, spriteProyectil);
                }
            }
        }
        // --- LÓGICA DEBUFFER ---
        else if (gameObject.CompareTag("Debuffer"))
        {
            objetivo.TakeDamage(unitData.attackDamage);
        }
        // --- LÓGICA CD MODIFICADA ---
        else if (gameObject.CompareTag("CD"))
        {
            StartCoroutine(CanalizarRayo(objetivo));
        }
    }

   IEnumerator CanalizarRayo(IDamageable objetivo)
    {
        float duracionRayo = 2.5f; 
        float tiempoEntreTicks = 0.25f; 
        float tiempoPasado = 0f;

        Component targetComp = objetivo as Component;
        GameObject rayoVisual = null;

        if (rayoPrefab != null && targetComp != null)
        {
            rayoVisual = Instantiate(rayoPrefab, transform.position, Quaternion.identity);
            
            Rayo scriptRayo = rayoVisual.GetComponent<Rayo>();
            if (scriptRayo != null)
            {
                scriptRayo.ConfigurarRayo(transform, targetComp.transform, animacionRayo, spriteRayo);
                Debug.Log("[CD] Rayo instanciado y configurado correctamente.");
            }
            else
            {
                Debug.LogError("[CD] ¡ERROR! El rayoPrefab no tiene el script 'RayoKamehameha' pegado.");
            }
        }

        while (tiempoPasado < duracionRayo)
        {
            if (targetComp == null || targetComp.gameObject == null) 
            {
                Debug.LogWarning("[CD] El rayo se apagó porque el enemigo murió o desapareció.");
                break;
            }

            float distanciaActual = Vector2.Distance(transform.position, targetComp.transform.position);
            
            // Si esto salta, el rango está mal calculado en el espacio 2D
            if (distanciaActual > unitData.attackRange) 
            {
                Debug.LogWarning($"[CD] El rayo se apagó porque la distancia ({distanciaActual}) es mayor que el rango ({unitData.attackRange}).");
                break; 
            }

            objetivo.TakeDamage(unitData.attackDamage);

            yield return new WaitForSeconds(tiempoEntreTicks);
            tiempoPasado += tiempoEntreTicks;
        }

        if (rayoVisual != null) 
        {
            Debug.Log("[CD] Fin de la canalización. Destruyendo rayo visual.");
            Destroy(rayoVisual);
        }
    }

    private IDamageable GetNearestEnemy()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, unitData.attackRange, results);
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

    void OnDrawGizmos()
    {
        if (unitData != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, unitData.attackRange);
        }
    }
}