using System.Collections.Generic;
using UnityEngine;

public static class ValoresInventario
{
    static readonly List<GameObject> inventorySlots = new List<GameObject>();

    public static List<GameObject> getInventoryPrefabs()
    {
        return inventorySlots;
    }

    public static void setInventoryPrefabs(GameObject prefabToAdd)
    {
        if (inventorySlots.Count < 6)
            inventorySlots.Add(prefabToAdd);
        else
            Debug.Log("MaxRANGE");
    }

    public static void removeInventoryPrefab(GameObject prefabToRemove)
    {
        inventorySlots.Remove(prefabToRemove);
    }

    public static void Clear()
    {
        inventorySlots.Clear();
    }
}
