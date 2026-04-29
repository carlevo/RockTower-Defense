using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cards : MonoBehaviour
{
    [SerializeField] private cardInfo _card;
    public cardInfo card
    {
        get => _card;
        set
        {
            _card = value;
            RefreshCard();
            AddCardToInventory();
        }
    }

    [SerializeField] private Image img;
    [SerializeField] private TMP_Text nameText;

    void Start()
    {
        EnsureReferences();
        RefreshCard();
    }

    private void EnsureReferences()
    {
        if (img == null)
        {
            img = GetComponentInChildren<Image>(true);
        }
        if (nameText == null)
        {
            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
            if (texts != null && texts.Length > 0)
            {
                if (texts.Length == 1)
                {
                    nameText = texts[0];
                }
                else
                {
                    foreach (var txt in texts)
                    {
                        string lowerName = txt.gameObject.name.ToLower();
                        if (lowerName.Contains("name") || lowerName.Contains("title"))
                        {
                            nameText = txt;
                            break;
                        }
                    }
                    if (nameText == null)
                    {
                        nameText = texts[0];
                    }
                }
            }
        }
    }

    private void AddCardToInventory()
    {
        if (_card == null) return;

        var inventory = InventoryManager.Instance;
        if (inventory != null)
        {
            inventory.AddItem(_card, 1);
            Debug.Log("✓ Added to inventory: " + _card.itemName + " | Total items: " + inventory.inventory.Count);
        }
        else
        {
            Debug.LogWarning("Cards: failed to get or create InventoryManager.");
        }
    }

    private void RefreshCard()
    {
        if (_card == null) return;

        EnsureReferences();

        if (img != null)
        {
            if (_card.itemIcon != null)
            {
                img.sprite = _card.itemIcon;
            }
            else
            {
                Debug.LogWarning("Cards: reward card has no itemIcon assigned: " + _card.itemName);
            }
        }
        else
        {
            Debug.LogWarning("Cards: Image reference is not assigned.");
        }

        if (nameText != null)
        {
            nameText.text = _card.itemName;
        }
    }
}
