using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Josue : MonoBehaviour
{
    [Header("Estadísticas Josue")]
    [SerializeField] private float InvokeCooldown = 3f;
    [SerializeField] private float josueIsGoneCooldown = 3f; //3 Segundos que dura la desaparición

    [Header("Summon Details")]
    public GameObject JosuePrefab;

    private SpriteRenderer[] renderers;
    private bool enCiclo = false;

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        StartCoroutine(CicloJosue());
    }

    void Update() { }

    private IEnumerator CicloJosue()
    {
        while (true)
        {
            yield return new WaitForSeconds(InvokeCooldown);
            InstanciateJosue();
            yield return new WaitUntil(() => !enCiclo);
        }
    }

    private void InstanciateJosue()
    {
        if (JosuePrefab == null)
        {
            Debug.LogWarning("[Josue] JosuePrefab no asignado.");
            return;
        }

        Route route = FindObjectOfType<Route>();
        if (route == null || route.waypoints.Length == 0)
        {
            Debug.LogWarning("[Josue] No se encontró Route con waypoints en la escena.");
            return;
        }

        enCiclo = true;
        SetVisible(false);

        Vector3 spawnPos = route.waypoints[route.waypoints.Length - 1].position;
        GameObject instancia = Instantiate(JosuePrefab, spawnPos, Quaternion.identity);

        // Pasamos la ruta directamente para no depender del FindObjectOfType del prefab
        AllyBehavior ally = instancia.GetComponent<AllyBehavior>();
        if (ally != null)
            ally.InicializarRuta(route.waypoints);
        else
            Debug.LogWarning("[Josue] El JosuePrefab no tiene AllyBehavior — no se moverá por el path.");

        StartCoroutine(EsperarRetorno(instancia));
    }

    private IEnumerator EsperarRetorno(GameObject instancia)
    {
        // Mínimo josueIsGoneCooldown segundos invisible
        yield return new WaitForSeconds(josueIsGoneCooldown);

        // Esperar además hasta que el prefab muera
        yield return new WaitUntil(() => instancia == null);

        SetVisible(true);
        enCiclo = false;
    }

    private void SetVisible(bool visible)
    {
        foreach (SpriteRenderer sr in renderers)
            sr.enabled = visible;
    }
}
