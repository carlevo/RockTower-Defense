using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "new card", menuName = "Character")]
public class cardInfo : ItemData
{/*
    [FormerlySerializedAs("name")]
    [SerializeField] private string legacyName;

    [FormerlySerializedAs("image")]
    [SerializeField] private Sprite legacyImage;

    public string name
    {
        get => itemName;
        set => itemName = value;
    }

    public Sprite image
    {
        get => itemIcon;
        set => itemIcon = value;
    }

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(legacyName) && string.IsNullOrEmpty(itemName))
        {
            itemName = legacyName;
        }
        if (legacyImage != null && itemIcon == null)
        {
            itemIcon = legacyImage;
        }
    }*/
}
