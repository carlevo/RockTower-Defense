using UnityEngine;
using System.Collections.Generic;

public class Niveles : MonoBehaviour
{
    public static Niveles Instance { get; private set; }

    // Evento que se dispara cuando se completa un nivel
    public delegate void NivelCompletadoDelegate(string nombreNivel);
    public static event NivelCompletadoDelegate OnNivelCompletado;

    // Definir los nombres de los niveles en orden progresivo
    public static readonly string[] NIVEL_ORDEN = new string[]
    {
        "Tutorial",      // Índice 0 - Siempre disponible
        "Nivel1",        // Indice 1 - Requiere Tutorial completado
        "Nivel2",        // Indice 2 - Requiere Nivel1 completado
        "Nivel3",        // Indice 3 - Requiere Nivel2 completado
        "Nivel4",        // Indice 4 - Requiere Nivel3 completado
        "Nivel5",        // Indice 5 - Requiere Nivel4 completado
        // Añade más niveles según sea necesario
    };

    private const string PREFS_PREFIX = "NivelCompleted_";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[Niveles] Sistema de progresión inicializado");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Marca un nivel como completado
    /// </summary>
    public static void CompletarNivel(string NivelName)
    {
        // Si ya estaba completado, no hacer nada
        if (EsNivelCompletado(NivelName))
        {
            Debug.Log($"[Niveles] ℹ️ Nivel ya estaba completado: {NivelName}");
            return;
        }

        string key = PREFS_PREFIX + NivelName;
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        
        Debug.Log($"[Niveles] ✅ ¡NIVEL COMPLETADO! {NivelName}");
        Debug.Log($"[Niveles]    Guardado en PlayerPrefs: {key} = 1");
        Debug.Log($"[Niveles]    Disparando evento OnNivelCompletado...");
        
        // Disparar evento
        OnNivelCompletado?.Invoke(NivelName);
        
        Debug.Log($"[Niveles] ✅ Evento disparado correctamente");
    }

    /// <summary>
    /// Verifica si un nivel está completado
    /// </summary>
    public static bool EsNivelCompletado(string NivelName)
    {
        string key = PREFS_PREFIX + NivelName;
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    /// <summary>
    /// Verifica si un nivel puede ser jugado
    /// El Tutorial siempre está disponible
    /// Los demás niveles requieren completar el anterior
    /// </summary>
    public static bool PuedoJugarNivel(string NivelName)
    {
        // El Tutorial siempre está disponible
        if (NivelName == "Tutorial")
        {
            Debug.Log($"[Niveles] PuedoJugarNivel('{NivelName}') = TRUE (Tutorial siempre disponible)");
            return true;
        }

        // Buscar la posición del nivel en el orden
        int nivelIndex = -1;
        for (int i = 0; i < NIVEL_ORDEN.Length; i++)
        {
            if (NIVEL_ORDEN[i] == NivelName)
            {
                nivelIndex = i;
                break;
            }
        }

        // Si no existe el nivel, no se puede jugar
        if (nivelIndex <= 0)
        {
            Debug.LogWarning($"[Niveles] PuedoJugarNivel('{NivelName}') = FALSE (Nivel no encontrado o es Tutorial)");
            return false;
        }

        // Verificar si el nivel anterior está completado
        string nivelAnterior = NIVEL_ORDEN[nivelIndex - 1];
        bool puedeJugar = EsNivelCompletado(nivelAnterior);

        Debug.Log($"[Niveles] PuedoJugarNivel('{NivelName}') = {puedeJugar} | Requiere: '{nivelAnterior}' completado={EsNivelCompletado(nivelAnterior)}");

        return puedeJugar;
    }

    /// <summary>
    /// Obtiene el nombre del nivel actual según el nombre de la escena
    /// </summary>
    public static string ObtenerNombreNivelDesdeEscena(string sceneName)
    {
        // Mapeo de nombres de escena a nombres de nivel
        // Ajusta según tus nombres de escenas
        switch (sceneName.ToLower())
        {
            case "tutorial":
            case "tutorialscene":
            case "maptestscene":
            case "maptest":
                return "Tutorial";

            case "nivel1":
            case "nivel1scene":
                return "Nivel1";

            case "nivel2":
            case "nivel2scene":
                return "Nivel2";

            case "nivel3":
            case "nivel3scene":
                return "Nivel3";

            case "nivel4":
            case "nivel4scene":
                return "Nivel4";

            case "nivel5":
            case "nivel5scene":
                return "Nivel5";

            default:
                return sceneName;
        }
    }

    /// <summary>
    /// Reinicia el progreso (solo para testing)
    /// </summary>
    public static void ReiniciarProgreso()
    {
        foreach (string NivelName in NIVEL_ORDEN)
        {
            string key = PREFS_PREFIX + NivelName;
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();
        Debug.Log("[Niveles] Progreso reiniciado");
    }

    [ContextMenu("Niveles/Reiniciar progreso")]
    private void ReiniciarProgresoDesdeInspector()
    {
        ReiniciarProgreso();
    }

    // Metodo no estatico para poder enlazarlo desde un boton UI en el Inspector.
    public void ReiniciarProgresoDesdeBoton()
    {
        ReiniciarProgreso();
    }

    /// <summary>
    /// Obtiene el porcentaje de progreso (0-100)
    /// </summary>
    public static int ObtenerPorcentajeProgreso()
    {
        int nivelesCompletados = 0;
        foreach (string NivelName in NIVEL_ORDEN)
        {
            if (EsNivelCompletado(NivelName))
            {
                nivelesCompletados++;
            }
        }
        return (nivelesCompletados * 100) / NIVEL_ORDEN.Length;
    }
}
