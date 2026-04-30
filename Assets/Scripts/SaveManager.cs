using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string SaveKeyMoney    = "InventoryMoney";
    private const string SaveKeyUnlocked = "InventoryUnlocked";
    private const string SaveKeyEquipped = "InventoryEquipped";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        InventoryManagerUI ui = FindFirstObjectByType<InventoryManagerUI>();
        if (ui != null)
        {
            Save(ui.currentMoney, ui.unlockedItems, ui.equippedItems);
        }
    }

    public void Save(int money, HashSet<string> unlocked, HashSet<string> equipped)
    {
        SaveToPrefs(money, unlocked, equipped);
    }

    public static void SaveToPrefs(int money, HashSet<string> unlocked, HashSet<string> equipped)
    {
        PlayerPrefs.SetInt(SaveKeyMoney, money);
        PlayerPrefs.SetString(SaveKeyUnlocked, string.Join(",", unlocked ?? new HashSet<string>()));
        PlayerPrefs.SetString(SaveKeyEquipped, string.Join(",", equipped ?? new HashSet<string>()));
        PlayerPrefs.Save();
    }

    public bool Load(out int money, out HashSet<string> unlocked, out HashSet<string> equipped)
    {
        return LoadFromPrefs(out money, out unlocked, out equipped);
    }

    public static bool LoadFromPrefs(out int money, out HashSet<string> unlocked, out HashSet<string> equipped)
    {
        unlocked = new HashSet<string>();
        equipped = new HashSet<string>();
        money    = -1;

        if (!PlayerPrefs.HasKey(SaveKeyMoney))
        {
            return false;
        }

        money = PlayerPrefs.GetInt(SaveKeyMoney);

        string rawUnlocked = PlayerPrefs.GetString(SaveKeyUnlocked, "");
        foreach (string key in rawUnlocked.Split(','))
        {
            if (!string.IsNullOrEmpty(key)) unlocked.Add(key);
        }

        string rawEquipped = PlayerPrefs.GetString(SaveKeyEquipped, "");
        foreach (string key in rawEquipped.Split(','))
        {
            if (!string.IsNullOrEmpty(key)) equipped.Add(key);
        }

        return true;
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKeyMoney);
        PlayerPrefs.DeleteKey(SaveKeyUnlocked);
        PlayerPrefs.DeleteKey(SaveKeyEquipped);
        PlayerPrefs.Save();
    }

    [ContextMenu("Borrar guardado")]
    public void DeleteSaveEditor()
    {
        DeleteSave();
        Debug.Log("Guardado borrado.");
    }
}
