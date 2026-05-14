using UnityEngine;

public class Alex : MonoBehaviour
{
    [Header("Configuración de Ganancia")]
    [SerializeField] private int dineroPorOleada = 100;

    private void OnEnable()
    {
        // Nos suscribimos al evento: "Cuando termine la oleada, ejecuta mi función DarDinero"
        WaveSpawner.OnOleadaFinalizada += DarDinero;
    }

    private void OnDisable()
    {
        // Es muy importante desuscribirse al morir o quitar la unidad
        WaveSpawner.OnOleadaFinalizada -= DarDinero;
    }

    private void DarDinero()
    {
        if (Coins.Instance != null)
        {
            Debug.Log(gameObject.name + " está generando dinero...");
            Coins.Instance.AddCoins(dineroPorOleada); 
        }
    }
}