using System.Collections.Generic;
using UnityEngine;

public class AllyInvoker : MonoBehaviour
{
    public float summonCooldown;
   
    [Header("Summon Details")]
    //Lista con los prefabs de las invocaciones
    public List<GameObject> prefabsToSummon;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        InvokeRepeating("seleccionarSummon",5,15);
        
    }
//Seleccionamos un prefab aleatorio
    private void seleccionarSummon()
    {
        int summonSeleccionada = Random.Range(0, prefabsToSummon.Count);
        GameObject prefabDeLaUnidad = prefabsToSummon[summonSeleccionada];
        Debug.Log(summonSeleccionada + " con el nombre " +prefabDeLaUnidad.name);
       // prefabDeLaUnidad(0);
    }
}
