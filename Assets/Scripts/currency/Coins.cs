using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins;
    private int currency;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currency += 500;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        coins.text = currency.ToString();
    }
}
