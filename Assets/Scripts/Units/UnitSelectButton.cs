using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class UnitSelectButton : MonoBehaviour
{
    //Esto es el "titulo del campo"
    [Header("Prefab a colocar")]
    //Campo para arrastrar el gameobject con la "descripcion" unitprefab (le damos un nombre relacionado a lo que es)
    public GameObject unitPrefab;

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
        UnitPlacer.Instance.SelectUnit(this);
    }

    public void SetSelected(bool selected)
    {
        sr.color = selected ? selectedColor : normalColor;
    }
}
