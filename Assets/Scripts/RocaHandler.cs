using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RocaHandler : MonoBehaviour
{
    public static RocaHandler Instance { get; private set; }

    [SerializeField] public SpriteRenderer colorHp;

    private const float maxHP = 100f;
    private const float recoveryDelay = 5f;
    private const float regenPerSecond = 10f;

    private float rocaHP = maxHP;
    private float timeSinceLastDamage = 0f;

    [SerializeField] public TextMeshProUGUI textoHp;

    [SerializeField] public GameObject menu;



    void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        textoHp.text =rocaHP.ToString()+"/"+ maxHP.ToString();
        menu.SetActive(false);
    }

    void Update()
    {
        
        if (rocaHP >= maxHP) return;

        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= recoveryDelay)
        {
            rocaHP = Mathf.Min(rocaHP + regenPerSecond * Time.deltaTime, maxHP);
            UpdateAlpha();
        }
        if(rocaHP <= 0)
        {
            Time.timeScale=0f;
        }
    }

    public void TakeDamage(float damage)
    {
        rocaHP = Mathf.Max(rocaHP - damage, 0f);
        Debug.Log(rocaHP);
        timeSinceLastDamage = 0f;
        UpdateAlpha();
        textoHp.text=UpdateHpText();
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
        String v = rocaHP.ToString()+"/"+ maxHP.ToString(); 
        return  v;
    }
    private void hpVisualFeedBack()
    {
        if(rocaHP <=50f && rocaHP > 25f)
        {
            textoHp.color=Color.orange;
        }
        if (rocaHP <= 25f)
        {
            textoHp.color=Color.red;
        }
        
    }

    private void CheckRocaLife()
    {
       if(rocaHP <= 0)
        {
            showMenu(true,"Defeat");
        } 
    }

    private void showMenu(bool _value, String _result)
    {
        menu.SetActive(_value);
        //Busca el gameobject llamado result en la escena y le pilla el textmeshpro
        TextMeshProUGUI _textFromMenu= GameObject.Find("Result").GetComponent<TextMeshProUGUI>();
        _textFromMenu.text=_result;

    }
}
