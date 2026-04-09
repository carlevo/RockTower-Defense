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



    void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        textoHp.text =rocaHP.ToString()+"/"+ maxHP.ToString();
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
}
