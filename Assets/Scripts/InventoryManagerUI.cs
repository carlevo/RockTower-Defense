using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public RectTransform inventoryContainer;

    private void Awake()
    {
        EnsureReferences();
    }

    private void EnsureReferences()
    {
        if (itemSlotPrefab == null)
        {
            Debug.LogWarning("InventoryManagerUI: itemSlotPrefab not assigned in inspector. Assign 'PF_InventorySlot' prefab from Assets/Prefabs/ to see inventory UI.");
        }

        if (inventoryContainer != null && inventoryContainer.gameObject == null)
        {
            inventoryContainer = null;
        }

        if (inventoryContainer == null)
        {
            Canvas rootCanvas = GetComponentInParent<Canvas>();
            if (rootCanvas == null)
            {
                rootCanvas = FindObjectOfType<Canvas>();
            }

            if (rootCanvas != null)
            {
                RectTransform existingContainer = FindInventoryPanel(rootCanvas);
                if (existingContainer != null)
                {
                    inventoryContainer = existingContainer;
                    EnsureContainerLayout(inventoryContainer);
                    return;
                }

                GameObject containerGO = new GameObject("InventoryContainer", typeof(RectTransform));
                RectTransform rectTransform = containerGO.GetComponent<RectTransform>();
                rectTransform.SetParent(rootCanvas.transform, false);
                rectTransform.anchorMin = new Vector2(0.03f, 0.97f);
                rectTransform.anchorMax = new Vector2(0.03f, 0.97f);
                rectTransform.pivot = new Vector2(0f, 1f);
                rectTransform.anchoredPosition = new Vector2(20f, -20f);
                rectTransform.sizeDelta = new Vector2(180f, 280f);

                var image = containerGO.AddComponent<UnityEngine.UI.Image>();
                image.color = new Color(0f, 0f, 0f, 0.4f);

                EnsureContainerLayout(rectTransform);
                inventoryContainer = rectTransform;
            }
            else
            {
                Debug.LogWarning("InventoryManagerUI: No Canvas found in scene. Create a Canvas or assign an inventoryContainer manually.");
            }
        }
        else
        {
            EnsureContainerLayout(inventoryContainer);
        }
    }

    private RectTransform FindInventoryPanel(Canvas rootCanvas)
    {
        RectTransform[] rects = rootCanvas.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform rect in rects)
        {
            string lowerName = rect.gameObject.name.ToLower();
            if (rect == rootCanvas.transform) continue;
            if (lowerName.Contains("inventory") || lowerName.Contains("inventario") || lowerName.Contains("panel") || lowerName.Contains("frame"))
            {
                return rect;
            }
        }
        return null;
    }

    private void EnsureContainerLayout(RectTransform rect)
    {
        if (rect == null || rect.gameObject == null)
        {
            Debug.LogWarning("InventoryManagerUI: Cannot ensure layout because the assigned inventoryContainer has no RectTransform or has been destroyed.");
            return;
        }
        GameObject target = rect.gameObject;
        if (target.GetComponent<VerticalLayoutGroup>() == null)
        {
            var layout = target.AddComponent<VerticalLayoutGroup>();
            if (layout == null)
            {
                Debug.LogWarning("InventoryManagerUI: Failed to add VerticalLayoutGroup to inventoryContainer.");
                return;
            }
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.spacing = 8f;
            layout.padding = new RectOffset(8, 8, 8, 8);
        }
        if (rect.GetComponent<ContentSizeFitter>() == null)
        {
            var fitter = rect.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }

    public void RefreshInventoryUI()
    {
        EnsureReferences();
        
        if (inventoryContainer == null)
        {
            Debug.LogWarning("InventoryManagerUI: inventoryContainer is null, cannot refresh UI.");
            return;
        }
        
        // Clear existing UI
        foreach (Transform t in inventoryContainer)
        {
            Destroy(t.gameObject);
        }

        // Create new UI elements for each item in the inventory
        if (itemSlotPrefab == null)
        {
            Debug.LogWarning("InventoryManagerUI: itemSlotPrefab not assigned. Items added to inventory but not displayed.");
            return;
        }

        foreach (Item item in InventoryManager.Instance.inventory)
        {
            GameObject newItemSlot = Instantiate(itemSlotPrefab, inventoryContainer, false);
            newItemSlot.transform.localScale = Vector3.one;
            RectTransform slotRect = newItemSlot.GetComponent<RectTransform>();
            if (slotRect != null)
            {
                slotRect.anchoredPosition = Vector2.zero;
                slotRect.localScale = Vector3.one;
            }

            ItemSlotUI newItemSlotUI = newItemSlot.GetComponent<ItemSlotUI>();
            if (newItemSlotUI == null) continue;

            if (newItemSlotUI.itemIconImage != null)
                newItemSlotUI.itemIconImage.sprite = item.itemData.itemIcon;
            if (newItemSlotUI.itemNameText != null)
                newItemSlotUI.itemNameText.text = item.itemData.itemName;
            if (newItemSlotUI.itemQuantityText != null)
                newItemSlotUI.itemQuantityText.text = "x" + item.itemQuantity.ToString();
        }
    }
}
