using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{
//Funcion para cambiar escena

public void cambioEscena( String nameScene)
    {
        //Redirección a la página del gacha
        if(nameScene == "InventoryScene")
        {
            Debug.Log("Cambiando a la escena de inventario...");
            GameObject inventoryManagerGO = GameObject.Find("InventoryManager");
            if (inventoryManagerGO == null)
            {
                Debug.LogWarning("No se encontró el objeto InventoryManager. Se intentará reasignar al cargar la escena InventoryScene.");
            }
            else
            {
                InventoryManagerUI iMU = inventoryManagerGO.GetComponent<InventoryManagerUI>();
                if (iMU == null)
                {
                    Debug.LogWarning("No se encontró el componente InventoryManagerUI en el objeto InventoryManager. Se intentará reasignar al cargar la escena InventoryScene.");
                }
            }
        }
        SceneManager.LoadScene(nameScene);
    }
}
