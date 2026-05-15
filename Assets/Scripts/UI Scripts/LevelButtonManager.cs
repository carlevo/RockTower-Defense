using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons; // Arrastra los botones aquí en orden (Tutorial, Level1, Level2, etc)
    [SerializeField] private Color buttonDisabledColor = new Color(0.5f, 0.5f, 0.5f, 0.6f); // Gris semi-transparente
    [SerializeField] private Color buttonEnabledColor = Color.white;

    private void Start()
    {
        ActualizarEstadoBotones();
    }

    private void OnEnable()
    {
        // Actualizar cada vez que la escena se carga
        ActualizarEstadoBotones();
    }

    public void ActualizarEstadoBotones()
    {
        if (levelButtons == null || levelButtons.Length == 0)
        {
            Debug.LogWarning("[LevelButtonManager] No se han asignado botones en el Inspector");
            return;
        }

        for (int i = 0; i < levelButtons.Length && i < Niveles.NIVEL_ORDEN.Length; i++)
        {
            Button btn = levelButtons[i];
            if (btn == null)
            {
                continue;
            }

            string levelName = Niveles.NIVEL_ORDEN[i];
            bool puedeJugar = Niveles.PuedoJugarNivel(levelName);

            // Actualizar el estado del botón
            btn.interactable = puedeJugar;

            // Cambiar color visual
            Image btnImage = btn.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.color = puedeJugar ? buttonEnabledColor : buttonDisabledColor;
            }

            // Cambiar color del texto
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.color = puedeJugar ? Color.white : Color.gray;
            }

            Debug.Log($"[LevelButtonManager] {levelName}: {(puedeJugar ? "✓ HABILITADO" : "✗ BLOQUEADO")}");
        }
    }
}
