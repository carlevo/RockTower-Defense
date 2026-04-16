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
            Debug.LogError("[UnitPlacer] No se encontró 'PathTileMap' en la escena.");
    }

    // Los botones llaman a este método para seleccionar qué torre colocar
    public void SelectUnit(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("[UnitPlacer] El prefab recibido es null.");
            return;
        }
        selectedPrefab = prefab;
        Debug.Log($"[UnitPlacer] Seleccionado: {prefab.name}");
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (pathTileMapParent == null) return;

        if (selectedPrefab == null)
        {
            Debug.LogWarning("[UnitPlacer] Ninguna unidad seleccionada. Clica primero un botón.");
            return;
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorld.x, mouseWorld.y);
        PlaceUnit(mousePos2D);
    }

    void PlaceUnit(Vector2 mousePos2D)
    {
        GameObject closestTile = null;
        float closestDist = 0.6f;

        foreach (Transform child in pathTileMapParent)
        {
            if (child.name != "RegularTile") continue;
            float dist = Vector2.Distance(child.position, mousePos2D);
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
            Debug.Log($"[UnitPlacer] Torre '{selectedPrefab.name}' colocada en: {closestTile.transform.position}");
        }
        else if (closestTile != null && occupiedTiles.Contains(closestTile))
        {
            Debug.LogWarning("[UnitPlacer] Este tile ya está ocupado.");
        }
    }
}
