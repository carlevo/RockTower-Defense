using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/New Item")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Equipment,
        Currency,
    }

    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
    public ItemType itemType;
}
