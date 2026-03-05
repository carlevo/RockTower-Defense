using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform inventoryContainer;

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

            ItemSlotUI newItemSlotUI = newItemSlot.GetComponent<ItemSlotUi>();

            itemSlotUI.itemIconImage.sprite = item.itemData.itemIcon;
            itemSlotUI.itemNameText.text = item.itemData.itemName;
            itemSlotUI.itemQuantityText.text = "x" + item.itemQuantity.ToString();
        }
    }
}
