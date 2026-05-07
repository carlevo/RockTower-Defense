using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Units/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("Inventario")]
    public GameObject unitPrefab;
    public Sprite unitSprite;
    public int cost = 100;

    [Header("Ataque")]
    public float attackRange = 2f;
    public float attackDamage = 20f;
    public float attackCooldown = 1f;
    public string attackAnimationTrigger = "Attack";
}
