using UnityEngine;
using TMPro;

public class NivelesDebugger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        ActualizarDebug();
    }

    private void Update()
    {
        // Presiona D para alternar debug
        if (Input.GetKeyDown(KeyCode.D))
        {
            ToggleDebug();
        }

        // Presiona T para marcar Tutorial como completado
        if (Input.GetKeyDown(KeyCode.T))
        {
            Niveles.CompletarNivel("Tutorial");
            ActualizarDebug();
        }

        // Presiona L para marcar Level1 como completado
        if (Input.GetKeyDown(KeyCode.L))
        {
            Niveles.CompletarNivel("Level1");
            ActualizarDebug();
        }

        // Presiona R para resetear progreso
        if (Input.GetKeyDown(KeyCode.R))
        {
            Niveles.ReiniciarProgreso();
            ActualizarDebug();
        }
    }

    public void ActualizarDebug()
    {
        if (debugText == null) return;

        string text = "=== NIVELES DEBUG ===\n\n";
        text += $"Progreso: {Niveles.ObtenerPorcentajeProgreso()}%\n\n";

        foreach (string levelName in Niveles.NIVEL_ORDEN)
        {
            bool completado = Niveles.EsNivelCompletado(levelName);
            bool puedoJugar = Niveles.PuedoJugarNivel(levelName);
            
            string estado = completado ? "✓ COMPLETADO" : "✗ NO COMPLETADO";
            string acceso = puedoJugar ? "✓ DISPONIBLE" : "✗ BLOQUEADO";
            
            text += $"{levelName}\n";
            text += $"  Estado: {estado}\n";
            text += $"  Acceso: {acceso}\n\n";
        }

        text += "CONTROLES:\n";
        text += "D = Toggle Debug\n";
        text += "T = Tutorial Completado\n";
        text += "L = Level1 Completado\n";
        text += "R = Resetear Progreso";

        debugText.text = text;
    }

    public void ToggleDebug()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = canvasGroup.alpha > 0.5f ? 0 : 1;
        }
    }

    // Botones públicos para UI
    public void BotonCompletarTutorial()
    {
        Niveles.CompletarNivel("Tutorial");
        ActualizarDebug();
        Debug.Log("Tutorial marcado como completado");
    }

    public void BotonCompletarLevel1()
    {
        Niveles.CompletarNivel("Level1");
        ActualizarDebug();
        Debug.Log("Level1 marcado como completado");
    }

    public void BotonReiniciarProgreso()
    {
        Niveles.ReiniciarProgreso();
        ActualizarDebug();
        Debug.Log("Progreso reiniciado");
    }
}
