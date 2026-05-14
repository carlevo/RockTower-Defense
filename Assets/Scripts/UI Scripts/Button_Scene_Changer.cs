using System;
using UnityEngine;

public class Button_Scene_Changer : MonoBehaviour
{
    private GameObject canvas; 
    public String sceneName = "";
    void Start()
    {
        // Buscamos el Canvas al iniciar
        canvas = GameObject.Find("Canvas");
        
        if (canvas == null) {
            Debug.LogError("No se encontró un GameObject llamado 'Canvas' en la escena.");
        }
    }

public void onClickEvent()
    {
        Debug.Log($"[Button_Scene_Changer] Click en botón. Scene a cargar: '{sceneName}'");
        
        string levelName = Niveles.ObtenerNombreNivelDesdeEscena(sceneName);
        Debug.Log($"[Button_Scene_Changer] Nombre de nivel mapeado: '{levelName}'");
        
        // Validar si se puede jugar este nivel
        if (!Niveles.PuedoJugarNivel(levelName))
        {
            Debug.LogWarning($"[Button_Scene_Changer] ❌ NO puedes jugar el nivel '{levelName}' aún. Completa el nivel anterior primero.");
            MostrarMensajeBloqueo(levelName);
            return;
        }

        Debug.Log($"[Button_Scene_Changer] ✓ Acceso permitido a '{levelName}'. Cambiando escena...");
        canvas.GetComponent<LobbySceneManager>().cambioEscena(sceneName);
    }

    private void MostrarMensajeBloqueo(string levelName)
    {
        // Buscar si existe un mensaje de bloqueo en la escena
        GameObject messageGO = GameObject.Find("LevelBlockedMessage");
        if (messageGO != null && messageGO.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = 1;
            messageGO.SetActive(true);
            
            // Desactivar después de 3 segundos
            Invoke(nameof(OcultarMensajeBloqueo), 3f);
        }
        else
        {
            Debug.Log($"Para desbloquear '{levelName}', primero completa el nivel anterior.");
        }
    }

    private void OcultarMensajeBloqueo()
    {
        GameObject messageGO = GameObject.Find("LevelBlockedMessage");
        if (messageGO != null)
        {
            messageGO.SetActive(false);
        }
    }
}

