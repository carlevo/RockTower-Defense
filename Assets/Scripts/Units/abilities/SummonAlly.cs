using System.Collections.Generic;
using UnityEngine;

public class AllyInvoker : MonoBehaviour
{
    public float summonCooldown = 15f;
    public float summonDelay = 5f;

    [Header("Summon Details")]
    public List<GameObject> prefabsToSummon;

    void Start()
    {
        if (prefabsToSummon == null || prefabsToSummon.Count == 0)
        {
            Debug.LogWarning("[AllyInvoker] No hay prefabs en prefabsToSummon.");
            return;
        }
        InvokeRepeating(nameof(SeleccionarSummon), summonDelay, summonCooldown);
    }

    void Update() { }

    private void SeleccionarSummon()
    {
        Route route = FindObjectOfType<Route>();
        Vector3 spawnPos = (route != null && route.waypoints.Length > 0)
            ? route.waypoints[route.waypoints.Length - 1].position
            : transform.position;

        int index = Random.Range(0, prefabsToSummon.Count);
        Instantiate(prefabsToSummon[index], spawnPos, Quaternion.identity);
        Debug.Log("[AllyInvoker] Invocado: " + prefabsToSummon[index].name);
    }
}
