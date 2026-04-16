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
    }

    public void BindItem(Item item)
    {
        boundItem = item;
    }

    public Item GetBoundItem()
    {
        return boundItem;
    }

    public void SetPurchasedState(bool isPurchased)
    {
        if (shadowOverlay != null)
        {
            shadowOverlay.SetActive(!isPurchased);
        }

        if (buyButton != null)
        {
            buyButton.interactable = !isPurchased;
        }
    }

}
