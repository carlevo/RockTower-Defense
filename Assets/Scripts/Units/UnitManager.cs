using UnityEngine;
using System.Collections.Generic;

public class UnitPlacer : MonoBehaviour
{
    public static UnitPlacer instance;

    private GameObject selectedPrefab = null;
    private Transform pathTileMapParent;
    private HashSet<GameObject> occupiedTiles = new HashSet<GameObject>();

    void Awake()
    {
        instance = this;

        GameObject ptm = GameObject.Find("PathTileMap");
        if (ptm != null)
            pathTileMapParent = ptm.transform;
        else
            Debug.LogError("UnitPlacer: No se encontró 'PathTileMap' en la escena.");
    }

    // Los botones llaman a este método para seleccionar qué torre colocar
    public void SelectUnit(GameObject prefab)
    {
        selectedPrefab = prefab;
        Debug.Log($"Unidad seleccionada: {prefab.name}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlaceUnit();
    }

    void PlaceUnit()
    {
        if (pathTileMapParent == null) return;

        if (selectedPrefab == null)
        {
            Debug.Log("Ninguna unidad seleccionada. Clica primero un botón.");
            return;
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Busca el RegularTile más cercano al click
        GameObject closestTile = null;
        float closestDist = 0.6f;

        foreach (Transform child in pathTileMapParent)
        {
            if (child.name != "RegularTile") continue;

            float dist = Vector2.Distance(child.position, mouseWorld);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestTile = child.gameObject;
            }
        }

        if (closestTile != null && !occupiedTiles.Contains(closestTile))
        {
            Instantiate(selectedPrefab, closestTile.transform.position, Quaternion.identity);
            occupiedTiles.Add(closestTile);
            Debug.Log($"Torre '{selectedPrefab.name}' colocada en: {closestTile.transform.position}");
        }
        else if (closestTile != null && occupiedTiles.Contains(closestTile))
        {
            Debug.Log("Este tile ya está ocupado.");
        }
    }
}
