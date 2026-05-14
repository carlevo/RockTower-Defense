using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GrupoEnemigos {
    public string nombreGrupo;
    public GameObject prefab;
    public int cantidad;
    public float intervaloSpawn;
}

[System.Serializable]
public class Oleada {
    public string nombreOleada;
    public List<GrupoEnemigos> grupos;
}
public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    // --- EVENTO PARA ALEX Y OTROS GENERADORES ---
    public delegate void OleadaFinalizada();
    public static event OleadaFinalizada OnOleadaFinalizada;

    [Header("Configuración")]
    [SerializeField] private List<Oleada> oleadas;
    [SerializeField] private float pausaEntreOleadas = 3f;
    [SerializeField] private int recompensaNivel = 150;

    private int totalEnemigosNivel = 0;
    private int enemigosMuertos = 0;
    private int oleadaActualIndex = 0;

    void Awake() { Instance = this; }

    void Start()
    {
        // Contamos total de enemigos
        totalEnemigosNivel = 0;
        foreach (var oleada in oleadas) {
            foreach (var grupo in oleada.grupos) {
                totalEnemigosNivel += grupo.cantidad;
            }
        }
        
        if (oleadas.Count > 0) StartCoroutine(GestionarOleadas());
    }

    public void RegistrarMuerteEnemigo()
    {
        enemigosMuertos++;
        Debug.Log($"Enemigo fuera. {enemigosMuertos} / {totalEnemigosNivel}");

        if (enemigosMuertos >= totalEnemigosNivel)
        {
            StartCoroutine(EsperarVictoria());
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
                    if (RocaHandler.Instance != null && RocaHandler.Instance.rocaHP <= 0) yield break;
                    Instantiate(grupo.prefab, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(grupo.intervaloSpawn);
                }
            }

            // --- AVISAR A ALEX ---
            // Cuando terminan de salir los enemigos de la oleada actual, lanzamos el evento
            Debug.Log("Spawning de oleada terminado, avisando a generadores...");
            OnOleadaFinalizada?.Invoke();

            oleadaActualIndex++;
            yield return new WaitForSeconds(pausaEntreOleadas);
        }
    }

    IEnumerator EsperarVictoria()
    {
        // Esperamos 2 segundos para que el jugador vea morir al último antes del menú
        yield return new WaitForSeconds(2f);
        FinalizarNivel();
    }

    private void FinalizarNivel()
    {
        if (DineroGlobal.Instance != null) DineroGlobal.Instance.SumarDinero(recompensaNivel);
        if (RocaHandler.Instance != null) RocaHandler.Instance.ShowVictory();
    }
}