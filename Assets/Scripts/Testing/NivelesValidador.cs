using UnityEngine;
using UnityEngine.UI;

public class NivelesValidador : MonoBehaviour
{
    [ContextMenu("Validar Setup Completo")]
    public void ValidarSetup()
    {
        Debug.Log("\n════════════════════════════════════════════════════════════");
        Debug.Log("🔍 VALIDADOR DE SETUP DE NIVELES");
        Debug.Log("════════════════════════════════════════════════════════════\n");

        int errores = 0;
        int advertencias = 0;

        // 1. Validar Niveles.cs
        Debug.Log("📋 PASO 1: Validar Niveles.Instance");
        if (Niveles.Instance != null)
        {
            Debug.Log("  ✓ Niveles.Instance encontrado\n");
        }
        else
        {
            Debug.LogError("  ✗ Niveles.Instance es NULL. Crea un GameObject 'NivelesManager' con Niveles.cs\n");
            errores++;
        }

        // 2. Validar NivelButtonManager
        Debug.Log("📋 PASO 2: Validar NivelButtonManager");
        NivelButtonManager buttonManager = FindFirstObjectByType<NivelButtonManager>();
        if (buttonManager != null)
        {
            Debug.Log("  ✓ NivelButtonManager encontrado");
            
            // Validar que tiene botones asignados
            Button[] botones = buttonManager.GetComponent<NivelButtonManager>()
                ?.GetType()
                .GetField("NivelButtons", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(buttonManager) as Button[];

            if (botones != null && botones.Length > 0)
            {
                Debug.Log($"  ✓ Tiene {botones.Length} botones asignados\n");
            }
            else
            {
                Debug.LogError("  ✗ No tiene botones asignados en el Inspector (NivelButtons array vacío)\n");
                errores++;
            }
        }
        else
        {
            Debug.LogError("  ✗ NivelButtonManager no encontrado. Crea un GameObject con este script\n");
            errores++;
        }

        // 3. Validar Botones de niveles
        Debug.Log("📋 PASO 3: Validar Botones");
        Button[] nivelButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        int botonesValidos = 0;

        foreach (Button btn in nivelButtons)
        {
            Button_Scene_Changer sceneChanger = btn.GetComponent<Button_Scene_Changer>();
            if (sceneChanger != null)
            {
                botonesValidos++;
                Debug.Log($"  ✓ Botón encontrado con Button_Scene_Changer");
            }
        }

        if (botonesValidos >= 6)
        {
            Debug.Log($"  ✓ Encontrados {botonesValidos} botones con Button_Scene_Changer\n");
        }
        else
        {
            Debug.LogWarning($"  ⚠️ Solo hay {botonesValidos} botones (esperado 6)\n");
            advertencias++;
        }

        // 4. Probar lógica de niveles
        Debug.Log("📋 PASO 4: Probar Lógica de Niveles");
        Debug.Log($"  Tutorial disponible: {Niveles.PuedoJugarNivel("Tutorial")}");
        Debug.Log($"  Nivel1 disponible: {Niveles.PuedoJugarNivel("Nivel1")}");
        Debug.Log($"  Tutorial completado: {Niveles.EsNivelCompletado("Tutorial")}\n");

        // RESUMEN
        Debug.Log("════════════════════════════════════════════════════════════");
        if (errores == 0 && advertencias == 0)
        {
            Debug.Log("✅ TODO ESTÁ CORRECTO");
        }
        else
        {
            Debug.Log($"❌ {errores} ERRORES | ⚠️ {advertencias} ADVERTENCIAS");
        }
        Debug.Log("════════════════════════════════════════════════════════════\n");
    }
}
