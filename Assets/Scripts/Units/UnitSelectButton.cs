using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class UnitSelectButton : MonoBehaviour
{
    [Header("Datos de la unidad")]
    public UnitData unitData;

//Titulo de la seccion
    [Header("Colores de estado")]
    //Colores. Aqui no se arrastra si no que es un campo aqui solo se asignan los valores
    public Color normalColor    = Color.white;
    public Color selectedColor  = new Color(0.4f, 1f, 0.4f); // verde claro


    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    void OnMouseDown()
    {
        if (unitData == null)
        {
            Debug.LogError($"[UnitSelectButton] '{name}' no tiene UnitData asignado.");
            return;
        }
        UnitPlacer.Instance.SelectUnit(this);
    }

    public void SetSelected(bool selected)
    {
        sr.color = selected ? selectedColor : normalColor;
    }
}
