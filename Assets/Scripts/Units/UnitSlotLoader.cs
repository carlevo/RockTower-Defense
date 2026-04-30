using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlotLoader : MonoBehaviour
{
    void Start()
    {
        List<GameObject> prefabs = ValoresInventario.getInventoryPrefabs();
        if (prefabs.Count == 0)
        {
            Debug.LogWarning("[UnitSlotLoader] ValoresInventario esta vacio. No se asignaron prefabs a los slots.");
            return;
        }

        UnitSelectButton[] slots = FindObjectsByType<UnitSelectButton>(FindObjectsSortMode.None)
            .OrderBy(b => b.name)
            .ToArray();

        for (int i = 0; i < slots.Length && i < prefabs.Count; i++)
        {
            int cost = slots[i].unitData != null ? slots[i].unitData.cost : 100;
            Sprite sprite = slots[i].unitData != null ? slots[i].unitData.unitSprite : null;

            UnitData newData = ScriptableObject.CreateInstance<UnitData>();
            newData.unitPrefab = prefabs[i];
            newData.cost = cost;
            newData.unitSprite = sprite;
            slots[i].unitData = newData;

            Image img = slots[i].GetComponentInChildren<Image>();
            if (img != null && sprite != null)
                img.sprite = sprite;
        }

        Debug.Log($"[UnitSlotLoader] {Mathf.Min(slots.Length, prefabs.Count)} slots asignados desde inventario.");
    }
}
