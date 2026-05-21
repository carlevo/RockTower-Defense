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
        
        string NivelName = Niveles.ObtenerNombreNivelDesdeEscena(sceneName);
        Debug.Log($"[Button_Scene_Changer] Nombre de nivel mapeado: '{NivelName}'");
        
        // Validar si se puede jugar este nivel
        if (!Niveles.PuedoJugarNivel(NivelName))
        {
            Debug.LogWarning($"[Button_Scene_Changer] ❌ NO puedes jugar el nivel '{NivelName}' aún. Completa el nivel anterior primero.");
            MostrarMensajeBloqueo(NivelName);
            return;
        }

        Debug.Log($"[Button_Scene_Changer] ✓ Acceso permitido a '{NivelName}'. Cambiando escena...");
        canvas.GetComponent<LobbySceneManager>().cambioEscena(sceneName);
    }

    private void MostrarMensajeBloqueo(string NivelName)
    {
        // Buscar si existe un mensaje de bloqueo en la escena
        GameObject messageGO = GameObject.Find("NivelBlockedMessage");
        if (messageGO != null && messageGO.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = 1;
            messageGO.SetActive(true);
            
            // Desactivar después de 3 segundos
            Invoke(nameof(OcultarMensajeBloqueo), 3f);
        }
        else
        {
            Debug.Log($"Para desbloquear '{NivelName}', primero completa el nivel anterior.");
        }
    }

    private void OcultarMensajeBloqueo()
    {
        GameObject messageGO = GameObject.Find("NivelBlockedMessage");
        if (messageGO != null)
        {
            messageGO.SetActive(false);
        }
    }
}

