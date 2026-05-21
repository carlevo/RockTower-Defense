using UnityEngine;

public class NivelesDiagnostico : MonoBehaviour
{
    [ContextMenu("DIAGNOSTICO COMPLETO")]
    public void DiagnosticoCompleto()
    {
        Debug.Log("\n" + new string('=', 70));
        Debug.Log("DIAGNOSTICO COMPLETO DEL SISTEMA DE NIVELES");
        Debug.Log(new string('=', 70));

        // 1. Verificar PlayerPrefs
        Debug.Log("\n1. VERIFICAR PLAYERPREFS:");
        for (int i = 0; i < Niveles.NIVEL_ORDEN.Length; i++)
        {
            string nivelName = Niveles.NIVEL_ORDEN[i];
            bool completado = Niveles.EsNivelCompletado(nivelName);
            string key = "NivelCompleted_" + nivelName;
            int valor = PlayerPrefs.GetInt(key, 0);
            
            Debug.Log($"   {i}. {nivelName}: EsNivelCompletado={completado} | PlayerPrefs.GetInt={valor}");
        }

        // 2. Verificar que PuedoJugarNivel devuelve lo correcto
        Debug.Log("\n2. VERIFICAR ACCESO A NIVELES:");
        for (int i = 0; i < Niveles.NIVEL_ORDEN.Length; i++)
        {
            string nivelName = Niveles.NIVEL_ORDEN[i];
            bool puedeJugar = Niveles.PuedoJugarNivel(nivelName);
            Debug.Log($"   {i}. {nivelName}: PuedoJugarNivel={puedeJugar}");
        }

        // 3. Ver si el evento está siendo escuchado
        Debug.Log("\n3. ESTADO DEL EVENTO:");
        Debug.Log("   (El evento se verificará cuando se dispare)");

        // 4. Verificar NivelButtonManager
        Debug.Log("\n4. ESTADO DE NivelButtonManager:");
        NivelButtonManager nbm = FindFirstObjectByType<NivelButtonManager>();
        if (nbm != null)
        {
            Debug.Log("   ✓ NivelButtonManager encontrado en la escena");
        }
        else
        {
            Debug.LogWarning("   ✗ NivelButtonManager NO encontrado en la escena");
        }

        // 5. Simular completion
        Debug.Log("\n5. PRUEBA DE COMPLETION:");
        Debug.Log("   Marcando 'Tutorial' como completado...");
        Niveles.CompletarNivel("Tutorial");
        Debug.Log($"   Tutorial completado: {Niveles.EsNivelCompletado("Tutorial")}");
        Debug.Log($"   Nivel1 disponible: {Niveles.PuedoJugarNivel("Nivel1")}");

        Debug.Log("\n" + new string('=', 70) + "\n");
    }

    [ContextMenu("BORRAR TODOS LOS DATOS")]
    public void BorrarTodos()
    {
        Niveles.ReiniciarProgreso();
        Debug.Log("✓ Todos los datos borrados");
    }

    [ContextMenu("DESBLOQUEAR TODOS LOS NIVELES")]
    public void DesbloquearTodos()
    {
        for (int i = 0; i < Niveles.NIVEL_ORDEN.Length; i++)
        {
            Niveles.CompletarNivel(Niveles.NIVEL_ORDEN[i]);
        }
        Debug.Log("✓ Todos los niveles desbloqueados");
    }
}
