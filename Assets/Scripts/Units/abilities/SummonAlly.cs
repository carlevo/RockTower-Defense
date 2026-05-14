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
        int index = Random.Range(0, prefabsToSummon.Count);
        GameObject prefab = prefabsToSummon[index];
        Instantiate(prefab, Vector3.zero, Quaternion.identity);
        Debug.Log("[AllyInvoker] Invocado: " + prefab.name);
    }
}
