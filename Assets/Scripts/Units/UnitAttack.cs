using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    [Header("Referencia de Datos")]
    public UnitData unitData; 

    [Header("Ajustes de Proyectil (Solo para DPS)")]
    [SerializeField] private GameObject proyectilPrefab; 
    [SerializeField] private Sprite spriteProyectil;    

    private float cooldownTimer = 0f;
    private Animator animator;
    private readonly Collider2D[] results = new Collider2D[20];

    void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    // MÉTODO CLAVE: Llamado por UnitPlacer justo al instanciar
    public void InicializarUnidad(UnitData nuevosDatos)
    {
        unitData = nuevosDatos;
        // Reiniciamos el cooldown para que no dispare al milisegundo de nacer
        cooldownTimer = unitData.attackCooldown;
        Debug.Log($"[UnitAttack] {gameObject.name} inicializado con éxito. Rango: {unitData.attackRange}");
    }

    void Update()
    {
        // Si aún no tenemos datos, no hacemos nada
        if (unitData == null) return;

        cooldownTimer -= Time.deltaTime;

        // Buscamos al enemigo más cercano
        IDamageable nearest = GetNearestEnemy();
        if (nearest == null) return;

        // Atacar solo cuando el cooldown llegue a 0
        if (cooldownTimer <= 0f)
        {
            EjecutarLogicaAtaque(nearest);
            cooldownTimer = unitData.attackCooldown;
        }
    }

    private void EjecutarLogicaAtaque(IDamageable objetivo)
    {
        // Animación
        if (!string.IsNullOrEmpty(unitData.attackAnimationTrigger))
            animator?.SetTrigger(unitData.attackAnimationTrigger);

        // Lógica según TAG
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
        else if (gameObject.CompareTag("Debuffer"))
        {
            objetivo.TakeDamage(unitData.attackDamage);
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
        else
        {
            // Círculo rojo de aviso si no hay datos
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}