using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlotLoader : MonoBehaviour
{
    void Start()
    {
        List<UnitData> units = ValoresInventario.getInventorySlots();

        Debug.Log($"[UnitSlotLoader] Personajes en ValoresInventario: {units.Count}");
        foreach (UnitData u in units)
            Debug.Log($"  - {u?.name}");

        if (units.Count == 0)
        {
            Debug.LogWarning("[UnitSlotLoader] ValoresInventario esta vacio.");
            return;
        }

        UnitSelectButton[] slots = FindObjectsByType<UnitSelectButton>(FindObjectsSortMode.None)
            .OrderBy(b => b.name)
            .ToArray();

        Debug.Log($"[UnitSlotLoader] Slots encontrados: {slots.Length}");

        for (int i = 0; i < slots.Length && i < units.Count; i++)
        {
            slots[i].unitData = units[i];

            Transform pjImage = slots[i].transform.Find("PJ_Image");
            if (pjImage == null)
                pjImage = FindDeep(slots[i].transform, "PJ_Image");

            if (pjImage != null)
            {
                Image img = pjImage.GetComponent<Image>();
                if (img != null && units[i].unitSprite != null)
                {
                    img.sprite = units[i].unitSprite;
                    Debug.Log($"[UnitSlotLoader] Sprite asignado a {slots[i].name}: {units[i].unitSprite.name}");
                }
                else
                {
                    Debug.LogWarning($"[UnitSlotLoader] {slots[i].name}: Image={img != null}, Sprite={units[i].unitSprite != null}");
                }
            }
            else
            {
                Debug.LogWarning($"[UnitSlotLoader] No se encontro PJ_Image en {slots[i].name}");
            }
        }
    }

    Transform FindDeep(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName) return child;
            Transform found = FindDeep(child, childName);
            if (found != null) return found;
        }
        return null;
    }
}
