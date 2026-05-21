using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
            CompletarNivelDebug("Tutorial");
        }

        // Presiona 1-5 para marcar niveles
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CompletarNivelDebug("Nivel1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CompletarNivelDebug("Nivel2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CompletarNivelDebug("Nivel3");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CompletarNivelDebug("Nivel4");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CompletarNivelDebug("Nivel5");
        }

        // Presiona U para desbloquear todos
        if (Input.GetKeyDown(KeyCode.U))
        {
            DesbloquearTodos();
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
        text += $"Escena actual: {SceneManager.GetActiveScene().name}\n";
        text += $"Progreso: {Niveles.ObtenerPorcentajeProgreso()}%\n\n";

        foreach (string NivelName in Niveles.NIVEL_ORDEN)
        {
            bool completado = Niveles.EsNivelCompletado(NivelName);
            bool puedoJugar = Niveles.PuedoJugarNivel(NivelName);
            
            string estado = completado ? "✓ COMPLETADO" : "✗ NO COMPLETADO";
            string acceso = puedoJugar ? "✓ DISPONIBLE" : "✗ BLOQUEADO";
            
            text += $"{NivelName}\n";
            text += $"  Estado: {estado}\n";
            text += $"  Acceso: {acceso}\n\n";
        }

        text += "CONTROLES:\n";
        text += "D = Toggle Debug\n";
        text += "T = Tutorial Completado\n";
        text += "1 = Nivel1 Completado\n";
        text += "2 = Nivel2 Completado\n";
        text += "3 = Nivel3 Completado\n";
        text += "4 = Nivel4 Completado\n";
        text += "5 = Nivel5 Completado\n";
        text += "U = Desbloquear Todos\n";
        text += "R = Resetear Progreso";

        debugText.text = text;
    }

    private void CompletarNivelDebug(string nivelName)
    {
        Niveles.CompletarNivel(nivelName);
        ActualizarDebug();
    }

    private void DesbloquearTodos()
    {
        for (int i = 0; i < Niveles.NIVEL_ORDEN.Length; i++)
        {
            Niveles.CompletarNivel(Niveles.NIVEL_ORDEN[i]);
        }

        ActualizarDebug();
        Debug.Log("Todos los niveles marcados como completados");
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
        CompletarNivelDebug("Tutorial");
        Debug.Log("Tutorial marcado como completado");
    }

    public void BotonCompletarNivel1()
    {
        CompletarNivelDebug("Nivel1");
        Debug.Log("Nivel1 marcado como completado");
    }

    public void BotonCompletarNivel2()
    {
        CompletarNivelDebug("Nivel2");
        Debug.Log("Nivel2 marcado como completado");
    }

    public void BotonCompletarNivel3()
    {
        CompletarNivelDebug("Nivel3");
        Debug.Log("Nivel3 marcado como completado");
    }

    public void BotonCompletarNivel4()
    {
        CompletarNivelDebug("Nivel4");
        Debug.Log("Nivel4 marcado como completado");
    }

    public void BotonCompletarNivel5()
    {
        CompletarNivelDebug("Nivel5");
        Debug.Log("Nivel5 marcado como completado");
    }

    public void BotonDesbloquearTodos()
    {
        DesbloquearTodos();
    }

    public void BotonReiniciarProgreso()
    {
        Niveles.ReiniciarProgreso();
        ActualizarDebug();
        Debug.Log("Progreso reiniciado");
    }
}
