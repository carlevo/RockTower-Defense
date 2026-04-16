using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManagerUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public RectTransform inventoryContainer;
    int currentMoney = 300;
    public TextMeshProUGUI moneyText;
    private readonly HashSet<string> unlockedItems = new HashSet<string>();

    private void Awake()
    {
        EnsureReferences();
        ResolveMoneyTextReference();
        UpdateMoneyText();
    }

    private void Start()
    {
        ResolveMoneyTextReference();
        UpdateMoneyText();
    }

    private void OnEnable()
    {
        ResolveMoneyTextReference();
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text =currentMoney.ToString();
        }
    }

    private void ResolveMoneyTextReference()
    {
        if (moneyText != null)
        {
            return;
        }

        Canvas rootCanvas = GetComponentInParent<Canvas>();
        if (rootCanvas == null)
        {
            rootCanvas = FindObjectOfType<Canvas>();
        }

        if (rootCanvas != null)
        {
            TextMeshProUGUI[] allTexts = rootCanvas.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI text in allTexts)
            {
                string lowerName = text.gameObject.name.ToLower();
                if (lowerName.Contains("money") || lowerName.Contains("dinero") || lowerName.Contains("coin"))
                {
                    moneyText = text;
                    return;
                }
            }
        }

        moneyText = GetComponentInChildren<TextMeshProUGUI>(true);
        if (moneyText == null)
        {
            Debug.LogWarning("InventoryManagerUI: No se encontro un TextMeshProUGUI para mostrar el dinero. Asigna 'moneyText' en el inspector.");
        }
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
        UpdateMoneyText();
        
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
            if (newItemSlotUI.itemPriceText != null)
                newItemSlotUI.itemPriceText.text = item.itemPrice.ToString();

            newItemSlotUI.BindItem(item);
            bool alreadyPurchased = IsItemPurchased(item);
            newItemSlotUI.SetPurchasedState(alreadyPurchased);

            if (newItemSlotUI.buyButton != null)
            {
                newItemSlotUI.buyButton.onClick.RemoveAllListeners();
                newItemSlotUI.buyButton.onClick.AddListener(() => TryBuyItem(item, newItemSlotUI));
            }
        }
    }

    private string GetItemKey(Item item)
    {
        if (item == null)
        {
            return string.Empty;
        }

        if (item.itemData != null && !string.IsNullOrEmpty(item.itemData.itemName))
        {
            return item.itemData.itemName;
        }

        return item.GetHashCode().ToString();
    }

    private bool IsItemPurchased(Item item)
    {
        string key = GetItemKey(item);
        return !string.IsNullOrEmpty(key) && unlockedItems.Contains(key);
    }

    public bool TryBuyItem(Item item, ItemSlotUI slotUI = null)
    {
        if (item == null)
        {
            Debug.LogWarning("InventoryManagerUI: No item received for purchase.");
            return false;
        }

        string itemName = item.itemData != null ? item.itemData.itemName : "Unknown Item";
        int totalPrice = Mathf.Max(0, item.itemPrice);

        if (IsItemPurchased(item))
        {
            if (slotUI != null)
            {
                slotUI.SetPurchasedState(true);
            }
            return true;
        }

        if (currentMoney < totalPrice)
        {
            Debug.Log("Not enough money to buy " + itemName);
            return false;
        }

        currentMoney -= totalPrice;
        UpdateMoneyText();

        string key = GetItemKey(item);
        if (!string.IsNullOrEmpty(key))
        {
            unlockedItems.Add(key);
        }

        if (slotUI != null)
        {
            slotUI.SetPurchasedState(true);
        }

        Debug.Log("Purchased: " + itemName);
        return true;
    }

    
}
