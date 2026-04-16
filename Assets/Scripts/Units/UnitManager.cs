using UnityEngine;
using System.Collections.Generic;

public class UnitPlacer : MonoBehaviour
{
    public static UnitPlacer Instance { get; private set; }

    private GameObject selectedPrefab;
    private UnitSelectButton selectedButton;

    private Transform pathTileMapParent;
    private HashSet<GameObject> occupiedTiles = new HashSet<GameObject>();

    void Awake()
    {
        Instance = this;

        GameObject ptm = GameObject.Find("PathTileMap");
        if (ptm != null)
            pathTileMapParent = ptm.transform;
        else
            Debug.LogError("UnitPlacer: No se encontró 'PathTileMap' en la escena.");
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // Primero comprobamos si el click es sobre un botón de selección
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.TryGetComponent<UnitSelectButton>(out var btn))
        {
            SelectUnit(btn);
            return;
        }

        PlaceUnit();
    }

    public void SelectUnit(UnitSelectButton button)
    {
        // Deselecciona el anterior
        if (selectedButton != null)
            selectedButton.SetSelected(false);

        selectedButton = button;
        selectedPrefab = button.unitPrefab;
        button.SetSelected(true);

        Debug.Log($"Unidad seleccionada: {(selectedPrefab != null ? selectedPrefab.name : "ninguna")}");
    }

    void PlaceUnit()
    {
        if (pathTileMapParent == null || selectedPrefab == null) return;

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
            Instantiate(selectedPrefab, closestTile.transform.position, Quaternion.identity);
            occupiedTiles.Add(closestTile);
            Debug.Log($"Torre colocada en: {closestTile.transform.position}");
        }
    }
}
