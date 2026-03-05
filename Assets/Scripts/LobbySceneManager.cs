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
        SceneManager.LoadScene(nameScene);
    }
}
