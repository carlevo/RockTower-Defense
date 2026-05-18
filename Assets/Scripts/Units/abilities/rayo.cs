using UnityEngine;

public class Rayo : MonoBehaviour
{
    private Transform target;
    private Transform origenTorre;
    private Animator rayoAnimator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rayoAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Ahora el método también acepta el sprite estático
    public void ConfigurarRayo(Transform torreTransform, Transform enemigoTransform, RuntimeAnimatorController controladorAnimacion, Sprite spriteEstatico)
    {
        origenTorre = torreTransform;
        target = enemigoTransform;

        // 1. SI TIENE ANIMACIÓN: La asignamos y nos aseguramos de que el Animator esté encendido
        if (controladorAnimacion != null)
        {
            if (rayoAnimator != null)
            {
                rayoAnimator.enabled = true;
                rayoAnimator.runtimeAnimatorController = controladorAnimacion;
            }
        }
        // 2. SI SOLO TIENE IMAGEN FIJA: Apagamos el Animator para que no interfiera y cambiamos el Sprite
        else if (spriteEstatico != null)
        {
            if (rayoAnimator != null) 
            {
                rayoAnimator.enabled = false; // Apagar el animator evita conflictos con sprites estáticos
            }
            
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = spriteEstatico;
            }
        }
        
        ActualizarPosicionYRotacion();
    }

    void Update()
    {
        ActualizarPosicionYRotacion();
    }

    private void ActualizarPosicionYRotacion()
    {
        if (origenTorre == null || target == null) return;

        transform.position = origenTorre.position;

        Vector3 direccion = target.position - origenTorre.position;
        float distancia = Vector2.Distance(origenTorre.position, target.position);

        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        transform.localScale = new Vector3(distancia, 1f, 1f);
    }
}