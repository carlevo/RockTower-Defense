using UnityEngine;
using System.Collections.Generic;

public class UnitPlacer : MonoBehaviour
{
    public static UnitPlacer Instance { get; private set; }

    private GameObject selectedPrefab;
    private UnitSelectButton selectedButton;
    private bool buttonClickedThisFrame = false;

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

    // Llamado desde OnMouseDown de UnitSelectButton, que dispara ANTES de Update
    public void SelectUnit(UnitSelectButton button)
    {
        buttonClickedThisFrame = true;

        if (button.unitPrefab == null)
        {
            Debug.LogError($"[UnitPlacer] '{button.name}' no tiene prefab asignado en UnitSelectButton.");
            return;
        }

        if (selectedButton != null)
            selectedButton.SetSelected(false);

        selectedButton = button;
        selectedPrefab = button.unitPrefab;
        button.SetSelected(true);

        Debug.Log($"[UnitPlacer] Seleccionado: {selectedPrefab.name}");
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // Si este frame se clicó un botón, no colocar unidad
        if (buttonClickedThisFrame)
        {
            buttonClickedThisFrame = false;
            return;
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorld.x, mouseWorld.y);
        PlaceUnit(mousePos2D);
    }

    void PlaceUnit(Vector2 mousePos2D)
    {
        if (pathTileMapParent == null)
        {
            Debug.LogError("[UnitPlacer] pathTileMapParent es null. ¿Existe 'PathTileMap' en la escena?");
            return;
        }
        if (selectedPrefab == null)
        {
            Debug.LogWarning("[UnitPlacer] Ninguna unidad seleccionada. Clica primero un botón.");
            return;
        }

        GameObject closestTile = null;
        float closestDist = float.MaxValue;

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

        if (closestTile != null && closestDist <= 0.6f && !occupiedTiles.Contains(closestTile))
        {
            Vector3 spawnPos = new Vector3(closestTile.transform.position.x, closestTile.transform.position.y, -1f);
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
            occupiedTiles.Add(closestTile);
            Debug.Log($"[UnitPlacer] Torre colocada en: {closestTile.transform.position}");
        }
        else if (closestDist > 0.6f)
        {
            Debug.LogWarning($"[UnitPlacer] Demasiado lejos del tile más cercano (dist: {closestDist:F3}). Clica más cerca del centro.");
        }
    }
}
