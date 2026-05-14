using UnityEngine;
using System.Collections.Generic;

public class UnitPlacer : MonoBehaviour
{

    //Declaración para que otros scripts lo puedan invocar desde cualquier sitio sin tener que buscarlo
    public static UnitPlacer Instance { get; private set; }
    //Esta es la variable que guardará la unitdata que se selecciona
    private UnitData selectedUnitData;

    //variable que guardará el boton seleccionado
    private UnitSelectButton selectedButton;
    private bool buttonClickedThisFrame = false;

    //Guardamos todas las "casillas" Tilemaps del hierarchy
    private Transform pathTileMapParent;
    //Diccionario que solo contiene las keys para guardar que tiles ya están ocupadas (no contiene el valor)
    private HashSet<GameObject> occupiedTiles = new HashSet<GameObject>();

    //Awake se utiliza para inicializar los valores al empezar el juego
    void Awake()
    {
        Instance = this;
        //Busca el pathtilemap
        GameObject ptm = GameObject.Find("PathTileMap");
        //Si lo encuentra guarda el transform rotacion, escala del objeto si no lo encuentra tira error
        if (ptm != null)
            pathTileMapParent = ptm.transform;
        else
            Debug.LogError("UnitPlacer: No se encontró 'PathTileMap' en la escena.");
    }

    // Llamado desde OnMouseDown de UnitSelectButton, que dispara ANTES de Update
    public void SelectUnit(UnitSelectButton button)
    {
        buttonClickedThisFrame = true;

        if (selectedButton != null)
            selectedButton.SetSelected(false);

        selectedButton = button;
        selectedUnitData = button.unitData;
        button.SetSelected(true);

        Debug.Log($"[UnitPlacer] Seleccionado: {selectedUnitData.unitPrefab.name} (coste: {selectedUnitData.cost})");
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
            Debug.LogError("[UnitPlacer] pathTileMapParent es null.");
            return;
        }
        if (selectedUnitData == null)
        {
            Debug.LogWarning("[UnitPlacer] Ninguna unidad seleccionada.");
            return;
        }

        if (!Coins.Instance.CanAfford(selectedUnitData.cost))
        {
            Debug.LogWarning($"[UnitPlacer] Sin monedas suficientes.");
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

        // Si encontramos un tile válido y no está ocupado
        if (closestTile != null && closestDist <= 0.6f && !occupiedTiles.Contains(closestTile))
        {
            Coins.Instance.SpendCoins(selectedUnitData.cost);
            
            // Posición de spawn (Z = -1 para estar frente al fondo)
            Vector3 spawnPos = new Vector3(closestTile.transform.position.x, closestTile.transform.position.y, -1f);
            
            // INSTANCIAR
            GameObject torreInstanciada = Instantiate(selectedUnitData.unitPrefab, spawnPos, Quaternion.identity);
            
            // CONFIGURAR DATOS
            UnitAttack scriptAtaque = torreInstanciada.GetComponent<UnitAttack>();
            if (scriptAtaque != null)
            {
                scriptAtaque.InicializarUnidad(selectedUnitData);
            }
            else 
            {
                // Si es la unidad generadora de dinero, buscamos su script específico
                Alex scriptDinero = torreInstanciada.GetComponent<Alex>();
                if(scriptDinero == null) Debug.LogWarning("La unidad colocada no tiene script de ataque ni de dinero.");
            }

            occupiedTiles.Add(closestTile);
            Debug.Log($"[UnitPlacer] {selectedUnitData.name} colocada con éxito.");
        }
    }
}
