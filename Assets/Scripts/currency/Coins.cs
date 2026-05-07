using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public static Coins Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI coins;
    private int currency;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currency = 3000;
    }

    void Update()
    {
        coins.text = currency.ToString();
    }

    public bool CanAfford(int amount) => currency >= amount;

    public bool SpendCoins(int amount)
    {
        if (!CanAfford(amount)) return false;
        currency -= amount;
        return true;
    }

    public void AddCoins(int amount)
    {
        currency += amount;
    }
}
