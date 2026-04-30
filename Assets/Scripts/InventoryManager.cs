using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<InventoryManager>();
                if (instance == null)
                {
                    InventoryManager[] managers = Resources.FindObjectsOfTypeAll<InventoryManager>();
                    if (managers != null && managers.Length > 0)
                    {
                        instance = managers[0];
                    }
                }
                
                if (instance == null)
                {
                    EnsureCreated();
                }
            }
            return instance;
        }
    }

    private static void EnsureCreated()
    {
        GameObject go = new GameObject("InventoryManager");
        instance = go.AddComponent<InventoryManager>();
        InventoryManagerUI ui = go.AddComponent<InventoryManagerUI>();
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            go.transform.SetParent(canvas.transform, false);
        }
        else
        {
            GameObject canvasGO = new GameObject("Canvas");
            Canvas newCanvas = canvasGO.AddComponent<Canvas>();
            newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            
            go.transform.SetParent(canvasGO.transform, false);
            canvas = newCanvas;
        }
        
        GameObject containerGO = new GameObject("InventoryContainer");
        RectTransform containerRect = containerGO.AddComponent<RectTransform>();
        containerRect.anchorMin = Vector2.zero;
        containerRect.anchorMax = Vector2.one;
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;
        containerGO.transform.SetParent(canvas.transform, false);
        
        ui.inventoryContainer = containerRect;
        
        Debug.Log("InventoryManager created automatically in scene.");
    }

    public List<Item> inventory = new List<Item>();


    [Header("Debug")]
    public ItemData testItemData;
    public ItemData testItemData2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        InventoryManagerUI ui = GetComponent<InventoryManagerUI>();
        if (ui != null)
        {
            ui.RefreshInventoryUI();
        }
    }

    public void AddItem(ItemData a, int b)
    {
        foreach (Item item in inventory)
        {
            if (item.itemData.itemName == a.itemName)
            {
                item.itemQuantity += b;
                RefreshUI();
                return;
            }
        }
        inventory.Add(new Item { itemData = a, itemQuantity = b });
        RefreshUI();
    }

    private void RefreshUI()
    {
        InventoryManagerUI ui = GetComponent<InventoryManagerUI>();
        if (ui == null)
        {
            ui = FindFirstObjectByType<InventoryManagerUI>();
        }
        if (ui == null)
        {
            InventoryManagerUI[] uis = Resources.FindObjectsOfTypeAll<InventoryManagerUI>();
            if (uis != null && uis.Length > 0)
            {
                ui = uis[0];
            }
        }
        if (ui != null)
        {
            ui.RefreshInventoryUI();
        }
    }

}
