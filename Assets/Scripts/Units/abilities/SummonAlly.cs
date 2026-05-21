using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyInvoker : MonoBehaviour
{
    public float summonCooldown = 15f;
    public float summonDelay = 5f;

    [Header("Animación")]
    [SerializeField] private string summonAnimationTrigger = "Attack";
    [SerializeField] private float delayTrasAnimacion = 1f;

    [Header("Summon Details")]
    public List<GameObject> prefabsToSummon;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

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
        StartCoroutine(AnimarYInvocar());
    }

    private IEnumerator AnimarYInvocar()
    {
        if (!string.IsNullOrEmpty(summonAnimationTrigger))
            animator?.SetTrigger(summonAnimationTrigger);

        yield return new WaitForSeconds(delayTrasAnimacion);

        Route route = FindObjectOfType<Route>();
        Vector3 spawnPos = (route != null && route.waypoints.Length > 0)
            ? route.waypoints[route.waypoints.Length - 1].position
            : transform.position;

        int index = Random.Range(0, prefabsToSummon.Count);
        Instantiate(prefabsToSummon[index], spawnPos, Quaternion.identity);
    }
}
