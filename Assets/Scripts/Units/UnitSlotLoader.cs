using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSlotLoader : MonoBehaviour
{
    private const string SaveKeyEquipped = "InventoryEquipped";

    void Start()
    {
        List<UnitData> units = ValoresInventario.getInventorySlots();
        RemoveNullUnits(units);

        if (units == null || units.Count == 0)
        {
            RebuildSlotsFromEquippedItems();
            units = ValoresInventario.getInventorySlots();
            RemoveNullUnits(units);
        }

        if (units == null || units.Count == 0)
        {
            RebuildSlotsFromSavedEquippedPrefs();
            units = ValoresInventario.getInventorySlots();
            RemoveNullUnits(units);
        }

        if (units == null || units.Count == 0)
        {
            RebuildSlotsFromSavedSelection();
            units = ValoresInventario.getInventorySlots();
            RemoveNullUnits(units);
        }

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

        for (int i = 0; i < slots.Length; i++)
        {
            if (i >= units.Count)
            {
                slots[i].unitData = null;
                Collider2D emptyCollider = slots[i].GetComponent<Collider2D>();
                if (emptyCollider != null) emptyCollider.enabled = false;

                // Si el slot está vacío, borramos el texto del coste
                TextMeshProUGUI emptyText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
                if (emptyText != null) emptyText.text = "";

                continue;
            }

            slots[i].unitData = units[i];
            Collider2D filledCollider = slots[i].GetComponent<Collider2D>();
            if (filledCollider != null) filledCollider.enabled = true;

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

            // --- NUEVA LÓGICA PARA ASIGNAR EL COSTE ---
            // Busca automáticamente cualquier componente TextMeshProUGUI dentro del slot actual
            TextMeshProUGUI textoCoste = slots[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textoCoste != null)
            {
                // NOTA: Revisa si en tu UnitData se llama 'cost'. Si se llama 'coste', cambia units[i].cost por units[i].coste
                textoCoste.text = $"${units[i].cost}";
                Debug.Log($"[UnitSlotLoader] Coste asignado a {slots[i].name}: {units[i].cost}");
            }
            else
            {
                Debug.LogWarning($"[UnitSlotLoader] No se encontró ningún componente de texto TextMeshProUGUI hijo en {slots[i].name}");
            }
        }
        // Restaurar selección de unidad después de asignar los datos
        string unidadSeleccionada = PlayerPrefs.GetString("UnidadSeleccionada", "");
        bool restaurada = false;
        if (!string.IsNullOrEmpty(unidadSeleccionada))
        {
            foreach (var btn in FindObjectsOfType<UnitSelectButton>())
            {
                if (btn.unitData != null && btn.unitData.name == unidadSeleccionada)
                {
                    if (UnitPlacer.Instance != null)
                    {
                        UnitPlacer.Instance.SelectUnit(btn);
                        restaurada = true;
                    }
                    break;
                }
            }
        }

        if (!restaurada && UnitPlacer.Instance != null)
        {
            UnitSelectButton primerBotonValido = FindObjectsOfType<UnitSelectButton>()
                .FirstOrDefault(b => b != null && b.unitData != null && b.unitData.unitPrefab != null);
            if (primerBotonValido != null)
            {
                UnitPlacer.Instance.SelectUnit(primerBotonValido);
            }
        }
    }

    private void RebuildSlotsFromEquippedItems()
    {
        InventoryManagerUI ui = FindFirstObjectByType<InventoryManagerUI>();
        if (ui == null || ui.equippedItems == null || ui.equippedItems.Count == 0)
        {
            return;
        }

        InventoryManager manager = InventoryManager.Instance;
        if (manager == null || manager.inventory == null || manager.inventory.Count == 0)
        {
            return;
        }

        foreach (Item item in manager.inventory)
        {
            if (item == null || item.itemData == null || item.itemData.unitData == null)
            {
                continue;
            }

            string key = string.IsNullOrEmpty(item.itemData.itemName) ? item.itemData.name : item.itemData.itemName;
            if (!ui.equippedItems.Contains(key))
            {
                continue;
            }

            List<UnitData> slots = ValoresInventario.getInventorySlots();
            if (!slots.Contains(item.itemData.unitData))
            {
                ValoresInventario.addUnit(item.itemData.unitData);
            }
        }

        Debug.Log($"[UnitSlotLoader] Slots reconstruidos desde equippedItems. Total: {ValoresInventario.getInventorySlots().Count}");
    }

    private void RebuildSlotsFromSavedEquippedPrefs()
    {
        string rawEquipped = PlayerPrefs.GetString(SaveKeyEquipped, "");
        if (string.IsNullOrEmpty(rawEquipped))
        {
            return;
        }

        string[] equippedKeys = rawEquipped
            .Split(',')
            .Select(k => k.Trim())
            .Where(k => !string.IsNullOrEmpty(k))
            .Distinct()
            .ToArray();

        if (equippedKeys.Length == 0)
        {
            return;
        }

        ItemData[] allItemData = Resources.FindObjectsOfTypeAll<ItemData>();
        UnitData[] allUnitData = Resources.FindObjectsOfTypeAll<UnitData>();
        int added = 0;

        foreach (string equippedKey in equippedKeys)
        {
            UnitData unit = null;

            ItemData itemMatch = allItemData.FirstOrDefault(i =>
                i != null &&
                (string.Equals(i.itemName, equippedKey, StringComparison.Ordinal) ||
                 string.Equals(i.name, equippedKey, StringComparison.Ordinal)));

            if (itemMatch != null)
            {
                unit = itemMatch.unitData;
            }

            if (unit == null)
            {
                unit = allUnitData.FirstOrDefault(u =>
                    u != null && string.Equals(u.name, equippedKey, StringComparison.Ordinal));
            }

            if (unit == null)
            {
                Debug.LogWarning($"[UnitSlotLoader] Equipped '{equippedKey}' no pudo mapearse a UnitData.");
                continue;
            }

            List<UnitData> slots = ValoresInventario.getInventorySlots();
            if (!slots.Contains(unit))
            {
                ValoresInventario.addUnit(unit);
                added++;
            }
        }

        if (added > 0)
        {
            Debug.Log($"[UnitSlotLoader] Slots reconstruidos desde PlayerPrefs InventoryEquipped. Aniadidos: {added}");
        }
    }

    private void RebuildSlotsFromSavedSelection()
    {
        string unidadSeleccionada = PlayerPrefs.GetString("UnidadSeleccionada", "");
        if (string.IsNullOrEmpty(unidadSeleccionada))
        {
            return;
        }

        UnitData[] allUnitData = Resources.FindObjectsOfTypeAll<UnitData>();
        UnitData match = allUnitData.FirstOrDefault(u =>
            u != null && string.Equals(u.name, unidadSeleccionada, StringComparison.Ordinal));

        if (match == null)
        {
            Debug.LogWarning($"[UnitSlotLoader] No se encontro UnitData para la selección guardada: {unidadSeleccionada}");
            return;
        }

        List<UnitData> slots = ValoresInventario.getInventorySlots();
        if (!slots.Contains(match))
        {
            ValoresInventario.addUnit(match);
            Debug.Log($"[UnitSlotLoader] Slot reconstruido desde UnidadSeleccionada: {match.name}");
        }
    }

    private void RemoveNullUnits(List<UnitData> units)
    {
        if (units == null) return;

        for (int i = units.Count - 1; i >= 0; i--)
        {
            if (units[i] == null)
            {
                units.RemoveAt(i);
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
