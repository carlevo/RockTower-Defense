using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public GameObject towerPrefab;

    // Asigna este método al evento OnClick del botón en el Inspector
    public void OnButtonClick()
    {
        if (towerPrefab == null)
        {
            Debug.LogError($"TowerButton '{gameObject.name}': No tiene ningún prefab asignado.");
            return;
        }

        UnitPlacer.instance.SelectUnit(towerPrefab);
    }
}
