using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NivelButtonManager : MonoBehaviour
{
    [SerializeField] private Button[] NivelButtons; // Arrastra los botones aquí en orden (Tutorial, Nivel1, Nivel2, etc)
    [SerializeField] private Color buttonDisabledColor = new Color(0.5f, 0.5f, 0.5f, 0.6f); // Gris semi-transparente
    [SerializeField] private Color buttonEnabledColor = Color.white;
    [SerializeField] private Color buttonDisabledTextColor = Color.gray;
    [SerializeField] private Color buttonEnabledTextColor = Color.white;

    private void Start()
    {
        ValidarYMostrarAdvertencia();
        ActualizarEstadoBotones();
        
        // Suscribirse a eventos de nivel completado
        if (Niveles.Instance != null)
        {
            Niveles.OnNivelCompletado += ActualizarEstadoBotones;
            Debug.Log("[NivelButtonManager] ✓ Suscrito a evento OnNivelCompletado");
        }
        else
        {
            Debug.LogError("[NivelButtonManager] ✗ Niveles.Instance es null");
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse para evitar memory leaks
        if (Niveles.Instance != null)
        {
            Niveles.OnNivelCompletado -= ActualizarEstadoBotones;
        }
    }

    private void OnEnable()
    {
        // Actualizar SIEMPRE que la escena se muestra, incluso si es la primera vez
        Debug.Log("[NivelButtonManager] OnEnable - actualizando botones");
        
        if (NivelButtons != null && NivelButtons.Length > 0)
        {
            ActualizarEstadoBotones();
        }
        else
        {
            Debug.LogError("[NivelButtonManager] OnEnable - NivelButtons vacío");
        }
    }

    private void OnBecameVisible()
    {
        // Este método se ejecuta cuando el componente se hace visible
        Debug.Log("[NivelButtonManager] OnBecameVisible - actualizando botones");
        ActualizarEstadoBotones();
    }

    private void ValidarYMostrarAdvertencia()
    {
        if (NivelButtons == null || NivelButtons.Length == 0)
        {
            Debug.LogError("[NivelButtonManager] ✗ CRÍTICO: No se han asignado botones en el Inspector.");
            Debug.LogError("[NivelButtonManager] Debes asignar los botones en orden:");
            for (int i = 0; i < Niveles.NIVEL_ORDEN.Length; i++)
            {
                Debug.LogError($"  {i}. {Niveles.NIVEL_ORDEN[i]}");
            }
            return;
        }

        if (NivelButtons.Length < Niveles.NIVEL_ORDEN.Length)
        {
            Debug.LogWarning($"[NivelButtonManager] ⚠️ Solo hay {NivelButtons.Length} botones pero hay {Niveles.NIVEL_ORDEN.Length} niveles");
        }
    }

    public void ActualizarEstadoBotones(string nivelCompletado = "")
    {
        if (NivelButtons == null || NivelButtons.Length == 0)
        {
            Debug.LogError("[NivelButtonManager] ✗ No se pueden actualizar botones: array vacío o null");
            return;
        }

        if (!string.IsNullOrEmpty(nivelCompletado))
        {
            Debug.Log($"[NivelButtonManager] 📢 Evento recibido: Nivel completado = {nivelCompletado}");
        }

        Debug.Log("\n════════════════════════════════════════════════════════════");
        Debug.Log("[NivelButtonManager] 🔄 ACTUALIZANDO ESTADO DE BOTONES");
        Debug.Log("════════════════════════════════════════════════════════════");

        for (int i = 0; i < NivelButtons.Length && i < Niveles.NIVEL_ORDEN.Length; i++)
        {
            Button btn = NivelButtons[i];
            if (btn == null)
            {
                Debug.LogWarning($"[NivelButtonManager] ⚠️ El botón en índice {i} es NULL");
                continue;
            }

            string NivelName = Niveles.NIVEL_ORDEN[i];
            bool puedeJugar = Niveles.PuedoJugarNivel(NivelName);
            bool esCompletado = Niveles.EsNivelCompletado(NivelName);

            // PASO 1: Desactivar/Activar el botón
            btn.interactable = puedeJugar;
            Debug.Log($"[NivelButtonManager] Bot {i} ({NivelName}): interactable = {puedeJugar}");

            // PASO 2: Cambiar color visual del botón (Image)
            Image btnImage = btn.GetComponent<Image>();
            if (btnImage != null)
            {
                Color newColor = puedeJugar ? buttonEnabledColor : buttonDisabledColor;
                btnImage.color = newColor;
                Debug.Log($"[NivelButtonManager] Bot {i} Image color: {(puedeJugar ? "ENABLED" : "DISABLED")}");
            }
            else
            {
                Debug.LogWarning($"[NivelButtonManager] ⚠️ Bot {i} no tiene componente Image");
            }

            // PASO 3: Cambiar color del texto (TextMeshProUGUI)
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.color = puedeJugar ? buttonEnabledTextColor : buttonDisabledTextColor;
                Debug.Log($"[NivelButtonManager] Bot {i} Text color: {(puedeJugar ? "WHITE" : "GRAY")}");
            }
            else
            {
                Debug.LogWarning($"[NivelButtonManager] ⚠️ Bot {i} no tiene componente TextMeshProUGUI");
            }

            // PASO 4: Debug info
            string estado = esCompletado ? "✓ COMPLETADO" : "✗ NO COMPLETADO";
            string acceso = puedeJugar ? "✓ HABILITADO" : "✗ BLOQUEADO";
            Debug.Log($"[{i}] {NivelName:10} | {estado:15} | {acceso}");
        }
        
        Debug.Log("═══════════════════════════════════════════════════════════");
    }
}
