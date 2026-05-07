using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Necesario para Listas

[System.Serializable]
public class GrupoEnemigos
{
    public string nombreGrupo;      // Ej: "Grupo de Teclas"
    public GameObject prefab;       // El enemigo (Tecla, Mouse, etc.)
    public int cantidad;            // Cuántos de este tipo
    public float intervaloSpawn;    // Tiempo entre cada enemigo de este grupo
}

[System.Serializable]
public class Oleada
{
    public string nombreOleada;     // Ej: "Oleada 1: El ataque periférico"
    public List<GrupoEnemigos> grupos; // Lista de diferentes tipos de enemigos en esta oleada
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Configuración de Oleadas")]
    [SerializeField] private List<Oleada> oleadas;
    [SerializeField] private float pausaEntreOleadas = 3f;

    private int oleadaActualIndex = 0;

    void Start()
    {
        Time.timeScale = 1f; // Aseguramos que el tiempo esté normal al iniciar
        if (oleadas.Count > 0)
        {
            StartCoroutine(GestionarOleadas());
        }
    }

    IEnumerator GestionarOleadas()
    {
        // Mientras queden oleadas por procesar
        while (oleadaActualIndex < oleadas.Count)
        {
            Oleada oleadaActual = oleadas[oleadaActualIndex];
            Debug.Log("--- Iniciando " + oleadaActual.nombreOleada + " ---");

            // 1. Spawneamos todos los grupos de la oleada actual
            foreach (GrupoEnemigos grupo in oleadaActual.grupos)
            {
                for (int i = 0; i < grupo.cantidad; i++)
                {
                    // Si la roca ha sido destruida mientras spawneamos, salimos de la corrutina
                    if (RocaHandler.Instance != null && RocaHandler.Instance.rocaHP <= 0)
                        yield break;

                    Instantiate(grupo.prefab, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(grupo.intervaloSpawn);
                }
            }

            // 2. Espera de seguridad para que el último enemigo instanciado aparezca en los cálculos
            yield return new WaitForSeconds(1f);

            // 3. BLOQUEO: Esperar hasta que no haya objetos con el tag "Enemy"
            // Solo pasará de línea cuando el contador de enemigos sea 0
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

            // 4. Verificación extra: si al limpiar la oleada resulta que hemos perdido, no seguimos
            if (RocaHandler.Instance != null && RocaHandler.Instance.rocaHP <= 0)
                yield break;

            Debug.Log("Oleada Limpia. Siguiente en " + pausaEntreOleadas + " segundos...");

            // 5. Pausa de descanso antes de la siguiente oleada
            yield return new WaitForSeconds(pausaEntreOleadas);

            oleadaActualIndex++;
        }

        // --- GRAN FINAL ---
        // Si el bucle termina, significa que todas las oleadas se han completado y limpiado
        Debug.Log("¡Felicidades! Todas las oleadas superadas.");

        if (RocaHandler.Instance != null)
        {
            RocaHandler.Instance.ShowVictory();
        }
    }
}