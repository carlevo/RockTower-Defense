using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NivelesDebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private NivelButtonManager nivelButtonManager;

    private void Start()
    {
        if (debugText == null)
        {
            debugText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (nivelButtonManager == null)
        {
            nivelButtonManager = FindFirstObjectByType<NivelButtonManager>();
        }

        ActualizarDebug();
    }

    private void Update()
    {
        // Presiona D para ver el debug
        if (Input.GetKeyDown(KeyCode.D))
        {
            ActualizarDebug();
        }

        // Presiona T para marcar Tutorial completado
        if (Input.GetKeyDown(KeyCode.T))
        {
            Niveles.CompletarNivel("Tutorial");
            ActualizarDebug();
        }

        // Presiona U para marcar Nivel1 completado
        if (Input.GetKeyDown(KeyCode.U))
        {
            Niveles.CompletarNivel("Nivel1");
            ActualizarDebug();
        }

        // Presiona R para reiniciar
        if (Input.GetKeyDown(KeyCode.R))
        {
            Niveles.ReiniciarProgreso();
            ActualizarDebug();
        }
    }

    private void ActualizarDebug()
    {
        if (debugText == null) return;

        string text = "=== DEBUG NIVELES ===\n\n";

        // Estado de Niveles.cs
        if (Niveles.Instance != null)
        {
            text += "OK Niveles.Instance encontrado\n";
        }
        else
        {
            text += "ERROR Niveles.Instance NULL\n";
        }

        // Estado de NivelButtonManager
        if (nivelButtonManager != null)
        {
            text += "OK NivelButtonManager encontrado\n\n";
        }
        else
        {
            text += "ERROR NivelButtonManager NULL\n\n";
        }

        // Estado de cada nivel
        text += "ESTADO DE NIVELES:\n";
        for (int i = 0; i < Niveles.NIVEL_ORDEN.Length; i++)
        {
            string nivelName = Niveles.NIVEL_ORDEN[i];
            bool completado = Niveles.EsNivelCompletado(nivelName);
            bool puedeJugar = Niveles.PuedoJugarNivel(nivelName);

            string estado = completado ? "HECHO" : "NO";
            string acceso = puedeJugar ? "DESBLOQUEADO" : "BLOQUEADO";

            text += i + ". " + nivelName + " | " + estado + " | " + acceso + "\n";
        }

        text += "\nCONTROLES:\n";
        text += "T = Marcar Tutorial completado\n";
        text += "U = Marcar Nivel1 completado\n";
        text += "R = Reiniciar progreso\n";
        text += "D = Actualizar debug";

        debugText.text = text;
    }
}
