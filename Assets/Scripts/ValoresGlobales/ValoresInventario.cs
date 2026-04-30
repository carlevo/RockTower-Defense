using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class ValoresInventario : MonoBehaviour
{
 List <GameObject> inventorySlots =new List<GameObject>();

    public List <GameObject> getInventoryPrefabs()
    {
        return inventorySlots;
    }

    public void setInventoryPrefabs(GameObject prefabToAdd)
    {
        if(inventorySlots.Capacity <6){ 
            inventorySlots.Add(prefabToAdd);
        }
        else
        {
            Debug.Log("MaxRANGE");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
