using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class UnitPlacer : MonoBehaviour
{
    public Tilemap groundTilemap;  // SOLO el Tilemap donde se puede colocar
    public Tilemap pathTilemap;    // Tilemap con caminos
    public GameObject unitPrefab;

    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    void Awake()
    {
        if (groundTilemap == null)
        {
            GameObject go = GameObject.Find("RegularTile");
            if (go != null) groundTilemap = go.GetComponent<Tilemap>();
        }
        if (pathTilemap == null)
        {
            GameObject go = GameObject.Find("RegularPath");
            if (go != null) pathTilemap = go.GetComponent<Tilemap>();
        }

        if (groundTilemap == null)
            Debug.LogError("UnitPlacer: No se encontró el Tilemap 'RegularTile'. Asígnalo en el Inspector o renombra el GameObject.");
        if (pathTilemap == null)
            Debug.LogError("UnitPlacer: No se encontró el Tilemap 'RegularPath'. Asígnalo en el Inspector o renombra el GameObject.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceUnit();
        }
    }

    void PlaceUnit()
    {
        if (groundTilemap == null || pathTilemap == null) return;

        // Obtener posición del mouse en el mundo
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Convertir a celda del grid
        Vector3Int cellPosition = groundTilemap.WorldToCell(mouseWorld);

        // Comprobar tiles
        bool hasGround = groundTilemap.HasTile(cellPosition); // solo GroundTilemap
        bool hasPath = pathTilemap.HasTile(cellPosition);     // solo PathTilemap

        Debug.Log($"Cell: {cellPosition} Ground:{hasGround} Path:{hasPath}");

        // Colocar unidad SOLO si hay suelo, NO hay camino y no está ocupada
        if (hasGround && !hasPath && !occupiedCells.Contains(cellPosition))
        {
            Vector3 spawnPosition = groundTilemap.GetCellCenterWorld(cellPosition);

            Instantiate(unitPrefab, spawnPosition, Quaternion.identity);

            occupiedCells.Add(cellPosition);
        }
    }
}