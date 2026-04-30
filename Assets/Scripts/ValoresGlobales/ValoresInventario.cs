using System.Collections.Generic;
using UnityEngine;

public static class ValoresInventario
{
    static readonly List<UnitData> inventorySlots = new List<UnitData>();

    public static List<UnitData> getInventorySlots()
    {
        return inventorySlots;
    }

    public static void addUnit(UnitData unit)
    {
        if (inventorySlots.Count < 6)
            inventorySlots.Add(unit);
        else
            Debug.Log("MaxRANGE");
    }

    public static void removeUnit(UnitData unit)
    {
        inventorySlots.Remove(unit);
    }

    public static void Clear()
    {
        inventorySlots.Clear();
    }
}
