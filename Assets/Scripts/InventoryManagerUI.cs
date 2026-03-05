using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform inventoryContainer;

    private void Start()
    {
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        // Clear existing UI
        foreach (Transform t in inventoryContainer)
        {
            Destroy(t.gameObject);
        }

        // Create new UI elements for each item in the inventory
        foreach (Item item in InventoryManager.instance.inventory)
        {
            GameObject newItemSlot = Instantiate(itemSlotPrefab, inventoryContainer);

            
        }
    }
}
