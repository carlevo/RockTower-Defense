using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{
    private NivelButtonManager nivelButtonManager;

    private void Start()
    {
        Debug.Log("[LobbySceneManager] Iniciando...");
        
        // Buscar el NivelButtonManager en la escena
        nivelButtonManager = FindFirstObjectByType<NivelButtonManager>();
        if (nivelButtonManager != null)
        {
            Debug.Log("[LobbySceneManager] ✓ NivelButtonManager encontrado");
            
            // FORÇA actualización inmediata
            nivelButtonManager.ActualizarEstadoBotones();
        }
        else
        {
            Debug.LogWarning("[LobbySceneManager] ⚠️ NivelButtonManager no encontrado en la escena. Los botones no se actualizarán.");
        }
    }

    private void OnEnable()
    {
        Debug.Log("[LobbySceneManager] OnEnable ejecutado");
        
        // Cuando la escena se carga (reload), actualizar botones
        if (nivelButtonManager != null)
        {
            nivelButtonManager.ActualizarEstadoBotones();
        }
    }

    //Funcion para cambiar escena
    public void cambioEscena(String nameScene)
    {
        Debug.Log($"[LobbySceneManager] Cambio de escena a: {nameScene}");
        
        //Redirección a la página del gacha
        if(nameScene == "InventoryScene")
        {
            Debug.Log("[LobbySceneManager] Cambiando a escena de inventario...");
            GameObject inventoryManagerGO = GameObject.Find("InventoryManager");
            if (inventoryManagerGO == null)
            {
                Debug.LogWarning("[LobbySceneManager] No se encontró InventoryManager.");
            }
            else
            {
                InventoryManagerUI iMU = inventoryManagerGO.GetComponent<InventoryManagerUI>();
                if (iMU == null)
                {
                    Debug.LogWarning("[LobbySceneManager] No se encontró InventoryManagerUI.");
                }
            }
        }
        
        Debug.Log($"[LobbySceneManager] Cargando escena: {nameScene}");
        SceneManager.LoadScene(nameScene);
    }

    // Método público para forzar actualización de botones
    public void ForzarActualizacionBotones()
    {
        if (nivelButtonManager != null)
        {
            Debug.Log("[LobbySceneManager] ForzarActualizacionBotones ejecutado");
            nivelButtonManager.ActualizarEstadoBotones();
        }
        else
        {
            Debug.LogError("[LobbySceneManager] No se puede actualizar: NivelButtonManager es null");
        }
    }
}
