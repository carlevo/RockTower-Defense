using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

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
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GetComponent<InventoryManagerUI>().RefreshInventoryUI();
    }

    public void AddItem(ItemData a, int b)
    {

        foreach (Item item in inventory)
        {
            if (item.itemData.itemName == a.itemName)
            {
                item.itemQuantity += b;
                return;
            }
        }
        inventory.Add(new Item { itemData = a, itemQuantity = b });
    }
}
