using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UnitPlacer : MonoBehaviour
{
    //void Start()
    //{
    //    // Restaurar la selección de unidad si existe
    //    string unidadSeleccionada = PlayerPrefs.GetString("UnidadSeleccionada", "");
    //    if (!string.IsNullOrEmpty(unidadSeleccionada))
    //    {
    //        foreach (var btn in FindObjectsOfType<UnitSelectButton>())
    //        {
    //            if (btn.unitData != null && btn.unitData.name == unidadSeleccionada)
    //            {
    //                SelectUnit(btn);
    //                break;
    //            }
    //        }
    //    }
    //}

    //Declaración para que otros scripts lo puedan invocar desde cualquier sitio sin tener que buscarlo
    public static UnitPlacer Instance { get; private set; }
    //Esta es la variable que guardará la unitdata que se selecciona
    private UnitData selectedUnitData;

    //variable que guardará el boton seleccionado
    private UnitSelectButton selectedButton;

    //Guardamos todas las "casillas" Tilemaps del hierarchy
    private Transform pathTileMapParent;
    //Diccionario que solo contiene las keys para guardar que tiles ya están ocupadas (no contiene el valor)
    private HashSet<GameObject> occupiedTiles = new HashSet<GameObject>();

    private SpriteRenderer hoveredTileSR;
    private static readonly Color highlightColor = new Color(0f, 1f, 0f, 0.5f);
    private static readonly Color normalTileColor = Color.white;

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

    public void SelectUnit(UnitSelectButton button)
    {
        if (button == null)
        {
            Debug.LogWarning("[UnitPlacer] SelectUnit recibió un botón null.");
            return;
        }

        if (button.unitData == null)
        {
            Debug.LogWarning($"[UnitPlacer] El botón '{button.name}' no tiene UnitData asignado.");
            return;
        }

        if (button.unitData.unitPrefab == null)
        {
            Debug.LogWarning($"[UnitPlacer] UnitData '{button.unitData.name}' no tiene unitPrefab asignado.");
            return;
        }

        if (selectedButton != null)
            selectedButton.SetSelected(false);

        selectedButton = button;
        selectedUnitData = button.unitData;
        button.SetSelected(true);

        // Guardar la selección en PlayerPrefs
        if (selectedUnitData != null)
        {
            PlayerPrefs.SetString("UnidadSeleccionada", selectedUnitData.name);
            PlayerPrefs.Save();
        }

        Debug.Log($"[UnitPlacer] Seleccionado: {selectedUnitData.unitPrefab.name} (coste: {selectedUnitData.cost})");
    }

    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mouseWorld.x, mouseWorld.y);

        if (selectedUnitData != null)
            UpdateHoverHighlight(mousePos2D);

        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        PlaceUnit(mousePos2D);
    }

    void UpdateHoverHighlight(Vector2 mousePos2D)
    {
        GameObject closestFree = null;
        float closestDist = float.MaxValue;

        foreach (Transform child in pathTileMapParent)
        {
            if (child.name != "RegularTile") continue;
            if (occupiedTiles.Contains(child.gameObject)) continue;
            float dist = Vector2.Distance(child.position, mousePos2D);
            if (dist < closestDist) { closestDist = dist; closestFree = child.gameObject; }
        }

        SpriteRenderer newSR = (closestFree != null && closestDist <= 0.6f)
            ? closestFree.GetComponent<SpriteRenderer>()
            : null;

        if (newSR == hoveredTileSR) return;

        if (hoveredTileSR != null) hoveredTileSR.color = normalTileColor;
        hoveredTileSR = newSR;
        if (hoveredTileSR != null) hoveredTileSR.color = highlightColor;
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

        if (selectedUnitData.unitPrefab == null)
        {
            Debug.LogWarning($"[UnitPlacer] La unidad seleccionada '{selectedUnitData.name}' no tiene prefab asignado.");
            return;
        }

        if (Coins.Instance == null)
        {
            Debug.LogError("[UnitPlacer] Coins.Instance es null.");
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
                Alex scriptDinero = torreInstanciada.GetComponent<Alex>();
                Ruben scriptRuben = torreInstanciada.GetComponent<Ruben>();
                if (scriptDinero == null && scriptRuben == null)
                    Debug.LogWarning("La unidad colocada no tiene script de ataque ni de dinero.");
            }

            occupiedTiles.Add(closestTile);
            hoveredTileSR = null;
            Debug.Log($"[UnitPlacer] {selectedUnitData.name} colocada con éxito.");
        }
    }

}
