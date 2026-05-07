using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Units/UnitData")]
public class UnitData : ScriptableObject
{
    public GameObject unitPrefab;
    public Sprite unitSprite;
    public int cost = 100;
}
