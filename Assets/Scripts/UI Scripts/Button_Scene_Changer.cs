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
    canvas.GetComponent<LobbySceneManager>().cambioEscena(sceneName);
    }
}
