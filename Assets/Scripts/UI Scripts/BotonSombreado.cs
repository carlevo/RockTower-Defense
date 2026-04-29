using UnityEngine;

public class BotonSombreado : MonoBehaviour
{
    private InventoryManagerUI GetInventoryUI()
    {
        InventoryManagerUI inventoryUI = GetComponentInParent<InventoryManagerUI>();
        if (inventoryUI != null)
        {
            return inventoryUI;
        }

        if (InventoryManager.Instance != null)
        {
            inventoryUI = InventoryManager.Instance.GetComponent<InventoryManagerUI>();
        }

        if (inventoryUI == null)
        {
            inventoryUI = FindFirstObjectByType<InventoryManagerUI>();
        }

        return inventoryUI;
    }

    public void SetShadowActive(bool active)
    {
        ItemSlotUI slotUI = GetComponentInParent<ItemSlotUI>();
        Transform root = slotUI != null ? slotUI.transform : transform;
        Transform sombreadoTransform = root.Find("Sombreado");

        if (sombreadoTransform == null)
        {
            Transform[] allChildren = root.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name == "Sombreado")
                {
                    sombreadoTransform = child;
                    break;
                }
            }
        }

        if (sombreadoTransform == null)
        {
            Debug.LogWarning("BotonSombreado: No se encontró el hijo 'Sombreado'.");
            return;
        }

        sombreadoTransform.gameObject.SetActive(active);
    }

    public void DisableShadowing()
    {
        ItemSlotUI slotUI = GetComponentInParent<ItemSlotUI>();
        if (slotUI == null)
        {
            Debug.LogWarning("BotonSombreado: No se encontró ItemSlotUI en el padre del botón.");
            return;
        }

        InventoryManagerUI inventoryUI = GetInventoryUI();
        if (inventoryUI == null)
        {
            Debug.LogWarning("BotonSombreado: No se encontró InventoryManagerUI.");
            return;
        }

        bool purchased = inventoryUI.TryBuyItem(slotUI.GetBoundItem(), slotUI);
        if (purchased)
        {
            SetShadowActive(false);
        }
    }

    public void EnableShadowing()
    {
        SetShadowActive(true);
    }
}
