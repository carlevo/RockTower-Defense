using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RocaHandler : MonoBehaviour
{
    public static RocaHandler Instance { get; private set; }

    [SerializeField] public SpriteRenderer colorHp;
    [SerializeField] private int enemiesToKill = 10;

    private const float maxHP = 100f;
    //private const float recoveryDelay = 5f;
    //private const float regenPerSecond = 10f;

    public float rocaHP = maxHP;
    private float timeSinceLastDamage = 0f;
    private int enemiesKilled = 0;

    [SerializeField] public TextMeshProUGUI textoHp;

    [SerializeField] public GameObject menu;
    [SerializeField] private GameObject btnNextNivel; // Arrastra el botón aquí
    [SerializeField] private TextMeshProUGUI resultText; // Arrastra el texto "Result" aquí
    [SerializeField] private GameObject txtNextNivel;



    void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        textoHp.text = rocaHP.ToString() + "/" + maxHP.ToString();
        menu.SetActive(false);
    }

    void Update()
    {

        if (rocaHP >= maxHP) return;

        timeSinceLastDamage += Time.deltaTime;

        //if (timeSinceLastDamage >= recoveryDelay)
        //{
        //    rocaHP = Mathf.Min(rocaHP + regenPerSecond * Time.deltaTime, maxHP);
        //    UpdateAlpha();
        //}
        if (rocaHP <= 0)
        {
            Time.timeScale = 0f; //Ponerlo en el victory
        }
    }

    public void TakeDamage(float damage)
    {
        rocaHP = Mathf.Max(rocaHP - damage, 0f);
        Debug.Log(rocaHP);
        timeSinceLastDamage = 0f;
        UpdateAlpha();
        textoHp.text = UpdateHpText();
        hpVisualFeedBack();
        CheckRocaLife();
    }

    private void UpdateAlpha()
    {
        // 0 vida = alpha 1 (negro total), 100 vida = alpha 0 (transparente)
        float alpha = 1f - (rocaHP / maxHP);
        Color c = colorHp.color;
        c.a = alpha;
        colorHp.color = c;
    }

    private String UpdateHpText()
    {
        String v = rocaHP.ToString() + "/" + maxHP.ToString();
        return v;
    }
    private void hpVisualFeedBack()
    {
        if (rocaHP <= 50f && rocaHP > 25f)
        {
            textoHp.color = Color.orange;
        }
        if (rocaHP <= 25f)
        {
            textoHp.color = Color.red;
        }

    }

    private void CheckRocaLife()
    {
        if (rocaHP <= 0)
        {
            showMenu(true, "Defeat");
        }
    }

    // Añade esta función dentro de RocaHandler
    private void LimpiarEnemigos()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigos)
        {
            Destroy(enemigo);
        }
    }

    // Modifica tu función showMenu para manejar el botón
    private void showMenu(bool _value, string _result)
{
    menu.SetActive(_value);
    
    if (resultText != null) 
        resultText.text = _result;

    // Controlamos el botón y su texto por separado si es necesario
    if (btnNextNivel != null)
    {
        bool esVictoria = (_result == "Victory");
        
        btnNextNivel.SetActive(esVictoria);
        
        // Si el texto no es hijo del botón, lo apagamos aquí también
        if (txtNextNivel != null) 
        {
            txtNextNivel.SetActive(esVictoria);
        }
    }

    if (_result == "Defeat")
    {
        LimpiarEnemigos();
    }
}

    public void ShowVictory()
    {
        // Solo mostramos victoria si no hemos perdido ya
        if (rocaHP > 0)
        {
            showMenu(true, "Victory");
            
            // NOTA: CompletarNivel se llama desde WaveSpawner.FinalizarNivel()
            // No duplicamos la llamada aquí para evitar conflictos
        }
    }

    public void ReiniciarEscena()
    {
        // Asegúrate de devolver el tiempo a la normalidad antes de cargar la escena
        Time.timeScale = 1f; 
        
        // Carga la escena que está activa actualmente
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
