using UnityEngine;
using System.Collections.Generic;

public class UnitPlacer : MonoBehaviour
{
    public GameObject unitPrefab;

    private Transform pathTileMapParent;
    private HashSet<GameObject> occupiedTiles = new HashSet<GameObject>();

    void Awake()
    {
        GameObject ptm = GameObject.Find("PathTileMap");
        if (ptm != null)
            pathTileMapParent = ptm.transform;
        else
            Debug.LogError("UnitPlacer: No se encontró 'PathTileMap' en la escena.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlaceUnit();
    }

    void PlaceUnit()
    {
        if (pathTileMapParent == null || unitPrefab == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Busca el RegularTile más cercano al click
        GameObject closestTile = null;
        float closestDist = 0.6f; // radio máximo (mitad de tile de 1 unidad)

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
            Instantiate(unitPrefab, closestTile.transform.position, Quaternion.identity);
            occupiedTiles.Add(closestTile);
            Debug.Log($"Torre colocada en: {closestTile.transform.position}");
        }
    }
}
