using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemQuantityText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;
    public GameObject shadowOverlay;
    public GameObject equippedIndicator;
    public GameObject equippedTick;
    public Vector2 equippedIndicatorPosition = new Vector2(58.2865f, 14.2365f);
    public Vector2 equippedIndicatorSize = new Vector2(50f, 44f);
    private bool uiIndicatorBuilt;
    private bool isPurchased;
    private bool isEquipped;

    private Item boundItem;

    private void Awake()
    {
        if (buyButton == null)
        {
            Transform buttonTransform = transform.Find("Button");
            if (buttonTransform != null)
            {
                buyButton = buttonTransform.GetComponent<Button>();
            }
        }

        if (shadowOverlay == null)
        {
            Transform shadowTransform = transform.Find("Sombreado");
            if (shadowTransform != null)
            {
                shadowOverlay = shadowTransform.gameObject;
            }
        }

        if (equippedIndicator == null)
        {
            Transform equippedTransform = transform.Find("Equipado");
            if (equippedTransform != null)
            {
                equippedIndicator = equippedTransform.gameObject;
            }
        }

        if (equippedTick == null && equippedIndicator != null)
        {
            Transform tickTransform = equippedIndicator.transform.Find("tickTransparente_0");
            if (tickTransform != null)
            {
                equippedTick = tickTransform.gameObject;
            }
        }

        EnsureUIEquippedIndicator();

        SetPurchasedState(false);
    }

    private void EnsureUIEquippedIndicator()
    {
        if (uiIndicatorBuilt)
        {
            return;
        }

        uiIndicatorBuilt = true;

        if (equippedIndicator == null)
        {
            return;
        }

        // If the indicator is already a UI object, keep using it directly.
        if (equippedIndicator.GetComponent<RectTransform>() != null && equippedIndicator.GetComponent<Image>() != null)
        {
            ApplyEquippedRect(equippedIndicator.GetComponent<RectTransform>());
            return;
        }

        SpriteRenderer indicatorSpriteRenderer = equippedIndicator.GetComponent<SpriteRenderer>();
        if (indicatorSpriteRenderer == null)
        {
            return;
        }

        RectTransform slotRect = transform as RectTransform;
        if (slotRect == null)
        {
            return;
        }

        GameObject uiIndicator = new GameObject("EquipadoUI", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        RectTransform uiIndicatorRect = uiIndicator.GetComponent<RectTransform>();
        uiIndicatorRect.SetParent(slotRect, false);
        uiIndicatorRect.SetAsLastSibling();

        Image uiIndicatorImage = uiIndicator.GetComponent<Image>();
        uiIndicatorImage.sprite = indicatorSpriteRenderer.sprite;
        uiIndicatorImage.color = indicatorSpriteRenderer.color;
        uiIndicatorImage.preserveAspect = true;
        uiIndicatorImage.raycastTarget = false;

        Bounds indicatorBounds = indicatorSpriteRenderer.bounds;
        if (indicatorBounds.size.sqrMagnitude > 0f)
        {
            equippedIndicatorSize = new Vector2(
                Mathf.Abs(indicatorBounds.size.x),
                Mathf.Abs(indicatorBounds.size.y)
            );
        }
        ApplyEquippedRect(uiIndicatorRect);

        GameObject uiTick = new GameObject("tickTransparente_0_UI", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        RectTransform uiTickRect = uiTick.GetComponent<RectTransform>();
        uiTickRect.SetParent(uiIndicatorRect, false);
        uiTickRect.anchorMin = new Vector2(0.5f, 0.5f);
        uiTickRect.anchorMax = new Vector2(0.5f, 0.5f);
        uiTickRect.pivot = new Vector2(0.5f, 0.5f);
        uiTickRect.anchoredPosition = Vector2.zero;
        uiTickRect.sizeDelta = new Vector2(18f, 18f);

        Image uiTickImage = uiTick.GetComponent<Image>();
        SpriteRenderer tickSpriteRenderer = equippedTick != null ? equippedTick.GetComponent<SpriteRenderer>() : null;
        if (tickSpriteRenderer != null)
        {
            uiTickImage.sprite = tickSpriteRenderer.sprite;
            uiTickImage.color = tickSpriteRenderer.color;
        }
        uiTickImage.preserveAspect = true;
        uiTickImage.raycastTarget = false;

        equippedIndicator = uiIndicator;
        equippedTick = uiTick;
    }

    private void ApplyEquippedRect(RectTransform rectTransform)
    {
        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = equippedIndicatorPosition;
        rectTransform.sizeDelta = equippedIndicatorSize;
    }

    public void OnEquipToggle()
    {
        if (!isPurchased)
        {
            return;
        }

        InventoryManagerUI inventoryUI = GetComponentInParent<InventoryManagerUI>();
        if (inventoryUI == null)
        {
            inventoryUI = FindFirstObjectByType<InventoryManagerUI>();
        }

        if (inventoryUI != null)
        {
            inventoryUI.TryToggleEquip(boundItem, this);
            return;
        }

        // Fallback if manager is not found.
        isEquipped = !isEquipped;
        SetEquippedVisual(isEquipped);
    }

    public void SetEquippedState(bool equipped)
    {
        isEquipped = equipped;
        SetEquippedVisual(isEquipped);
    }

    private void SetEquippedVisual(bool equipped)
    {
        if (equippedTick != null)
        {
            equippedTick.SetActive(equipped);
        }
    }

    public void BindItem(Item item)
    {
        boundItem = item;
    }

    public Item GetBoundItem()
    {
        return boundItem;
    }

    public void SetPurchasedState(bool purchased)
    {
        bool wasNotPurchased = !isPurchased;
        isPurchased = purchased;

        if (shadowOverlay != null)
        {
            shadowOverlay.SetActive(!purchased);
        }

        if (equippedIndicator != null)
        {
            equippedIndicator.SetActive(true);
        }

        if (!purchased)
        {
            isEquipped = false;
        }

        SetEquippedState(isEquipped);

        // When transitioning from unpurchased to purchased (i.e. just bought),
        // swap the button listener so subsequent clicks act as equip toggle.
        if (purchased && wasNotPurchased && buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnEquipToggle);
        }
    }

}
