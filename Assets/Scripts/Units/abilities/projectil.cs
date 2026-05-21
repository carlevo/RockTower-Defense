using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Transform objetivo;
    private float daño;
    private float velocidad = 8f;
    private SpriteRenderer sRenderer;

    void Awake()
    {
        // Buscamos el componente en el mismo objeto
        sRenderer = GetComponent<SpriteRenderer>();
    }

    public void Configurar(Transform _objetivo, float _daño, Sprite _sprite)
    {
        objetivo = _objetivo;
        daño = _daño;
        
        // Aquí es donde ocurre la magia: le ponemos el sprite que nos da la torre
        if (sRenderer != null && _sprite != null)
        {
            sRenderer.sprite = _sprite;
        }
    }

    void Update()
    {
        if (objetivo == null) { Destroy(gameObject); return; }

        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.position += direccion * velocidad * Time.deltaTime;

        // Rotar para que el objeto "mire" a donde vuela
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        if (Vector2.Distance(transform.position, objetivo.position) < 0.2f)
        {
            objetivo.GetComponentInParent<IDamageable>()?.TakeDamage(daño);
            Destroy(gameObject);
        }
    }
}