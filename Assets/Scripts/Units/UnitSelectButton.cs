using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class UnitSelectButton : MonoBehaviour
{
    [Header("Prefab a colocar")]
    public GameObject unitPrefab;

    [Header("Colores de estado")]
    public Color normalColor    = Color.white;
    public Color selectedColor  = new Color(0.4f, 1f, 0.4f); // verde claro

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    public void SetSelected(bool selected)
    {
        sr.color = selected ? selectedColor : normalColor;
    }
}
