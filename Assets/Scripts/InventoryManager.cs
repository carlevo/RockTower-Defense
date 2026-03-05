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
        AddItem(testItemData, 5);
        AddItem(testItemData2, 1);
    }

    public void AddItem(ItemData a, int b)
    {
        inventory.Add(new Item { itemData = a, itemQuantity = b});
    }
}
