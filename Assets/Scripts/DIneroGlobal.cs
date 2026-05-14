using UnityEngine;

public class DineroGlobal : MonoBehaviour
{
    public static DineroGlobal Instance { get; private set; }

    [SerializeField] private int defaultDinero = 5000000;
    public int dineroGlobal;

    private const string SaveKeyMoney = "InventoryMoney";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey(SaveKeyMoney))
        {
            dineroGlobal = PlayerPrefs.GetInt(SaveKeyMoney);
        }
        else
        {
            dineroGlobal = defaultDinero;
        }
    }

    private void OnApplicationQuit()
    {
        SaveMoney();
    }

    private void OnDisable()
    {
        // In editor, stopping Play Mode can skip or reorder lifecycle calls.
        SaveMoney();
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt(SaveKeyMoney, dineroGlobal);
        PlayerPrefs.Save();
    }

    public void SumarDinero(int cantidad)
    {
        dineroGlobal += cantidad;
        SaveMoney(); // Guardamos inmediatamente para evitar pérdidas si el juego crashea
        Debug.Log("Dinero actualizado: " + dineroGlobal);
    }
}
