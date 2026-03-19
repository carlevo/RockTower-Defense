using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gacha : MonoBehaviour
{


    public cardInfo card;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI name;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (card != null)
        {
            img.sprite = card.image;
            name.txt = card.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Start();
    }
}
