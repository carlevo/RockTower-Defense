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
    [Header("Recompensa")]
    [SerializeField] private int recompensaNivel = 150;

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
    while (oleadaActualIndex < oleadas.Count)
    {
        Oleada oleadaActual = oleadas[oleadaActualIndex];
        foreach (GrupoEnemigos grupo in oleadaActual.grupos)
        {
            for (int i = 0; i < grupo.cantidad; i++)
            {
                if (RocaHandler.Instance != null && RocaHandler.Instance.rocaHP <= 0)
                    yield break;

                Instantiate(grupo.prefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(grupo.intervaloSpawn);
            }
        }

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

        if (RocaHandler.Instance != null && RocaHandler.Instance.rocaHP <= 0)
            yield break;

        yield return new WaitForSeconds(pausaEntreOleadas);
        oleadaActualIndex++;
    }

    // Al terminar todas las oleadas de forma normal:
    FinalizarNivel();
}

    void Update()
{
    // --- CÓDIGO DE TESTEO ---
    // Si presionas la V, saltamos a la victoria inmediatamente
    if (Input.GetKeyDown(KeyCode.V))
    {
        Debug.Log("Cheat: Forzando Victoria");
        StopAllCoroutines(); // Detiene el spawn de enemigos actual
        FinalizarNivel();    // Llama a la función de dar dinero y mostrar menú
    }
    // ------------------------
}

// He sacado esto a una función aparte para poder llamarla desde el cheat y desde la corrutina
private void FinalizarNivel()
{
    if (DineroGlobal.Instance != null)
    {
        // Usamos la recompensa que configuraste en el Inspector
        DineroGlobal.Instance.SumarDinero(recompensaNivel);
    }

    if (RocaHandler.Instance != null)
    {
        RocaHandler.Instance.ShowVictory();
    }
}
}