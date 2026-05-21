# 🔧 TROUBLESHOOTING - NIVELES NO SE DESBLOQUEAN

## ¿QUÉ SUCEDE?

Completas un nivel → Victory → Vuelves al Lobby → **El siguiente nivel SIGUE BLOQUEADO**

---

## 🧪 DIAGNÓSTICO PASO A PASO

### PASO 1: Verificar que se está guardando correctamente

1. **En la escena del Lobby**:
   - Crea un GameObject vacío
   - Adjúntale el script **NivelesDiagnostico.cs**
   
2. **En el Inspector**:
   - Click en "DIAGNOSTICO COMPLETO"
   - Mira la consola

3. **Qué buscar en la consola**:
   ```
   Marcando 'Tutorial' como completado...
   Tutorial completado: True
   Nivel1 disponible: True
   ```
   
   Si ves esto → **Guardado funciona ✓**

---

### PASO 2: Verificar que los botones se actualizan cuando vuelves

1. **En Play Mode**:
   - Completa el Tutorial (mata todos los enemigos)
   - Mira la consola cuando se muestre "Victory"
   - Deberías ver:
     ```
     [Niveles] ✅ ¡NIVEL COMPLETADO! Tutorial
     [Niveles]    Disparando evento OnNivelCompletado...
     [NivelButtonManager] 📢 Evento recibido: Nivel completado = Tutorial
     [NivelButtonManager] 🔄 ACTUALIZANDO ESTADO DE BOTONES
     ```

2. **Presiona Next Level (o vuelve al Lobby)**
   
3. **Cuando aparece el Lobby**:
   - Deberías ver en la consola:
     ```
     [LobbySceneManager] OnEnable ejecutado
     [NivelButtonManager] OnEnable - actualizando botones
     ```

---

## 🔴 PROBLEMAS COMUNES Y SOLUCIONES

### Problema 1: "No veo el evento OnNivelCompletado"

**Síntomas**:
- Cuando completas un nivel, no ves "Evento recibido"
- Los botones no cambian de color

**Causas posibles**:
- Niveles.Instance es null
- El evento no está siendo suscrito correctamente

**Solución**:
1. En Hierarchy, busca "NivelesManager"
2. Verifica que tiene el script **Niveles.cs** adjunto
3. Si no existe, crea uno nuevo

---

### Problema 2: "Los botones se actualizan en pantalla pero NO cuando vuelvo"

**Síntomas**:
- Si presionas D varias veces en el Lobby, los botones cambian
- Pero cuando vuelves de un nivel, no cambian

**Causas posibles**:
- OnEnable no se está ejecutando
- NivelButtonManager se destruyó
- La referencia está perdida

**Solución**:
1. Verifica que NivelButtonManager está en el Canvas
2. Asegúrate de que NO está dentro de un Panel que se desactiva
3. En el Inspector, selecciona NivelButtonManager y mira que tiene botones asignados

---

### Problema 3: "Los datos se guardaron pero desaparecieron"

**Síntomas**:
- Usas el Diagnostico y ves "Tutorial completado: True"
- Pero después de cerrar y abrir el juego, desaparece

**Causas posibles**:
- PlayerPrefs no se está guardando
- OnApplicationQuit no se ejecuta

**Solución**:
1. En el diagnóstico, presiona "DESBLOQUEAR TODOS LOS NIVELES"
2. Presiona Ctrl+S en Unity (Save)
3. Cierra el juego
4. Reabre y comprueba

---

## 🔧 REPARACIÓN RÁPIDA

Si nada funciona, haz esto en este orden:

1. **Borra todo** (en NivelesDiagnostico):
   - Click en "BORRAR TODOS LOS DATOS"

2. **Presiona Play**

3. **En la consola**:
   - Presiona **T** (marca Tutorial completado)
   - Presiona **U** (marca Nivel1 completado)
   - Mira en consola si todo se actualiza

4. **Si funciona aquí pero NO en el juego**:
   - El problema está en tu WaveSpawner/RocaHandler
   - Verifica que Niveles.CompletarNivel se llama al ganar

5. **Si NO funciona**:
   - Ejecuta NivelesValidador
   - La consola te dirá exactamente qué falta

---

## 📊 FLUJO CORRECTO

```
JUEGA NIVEL
    ↓
GANAS (mata todos los enemigos)
    ↓
WaveSpawner.FinalizarNivel() se ejecuta
    ↓
Niveles.CompletarNivel("NombreDelNivel") se llama
    ↓
PlayerPrefs se guarda ✓
    ↓
Evento OnNivelCompletado se dispara
    ↓
NivelButtonManager recibe el evento
    ↓
ActualizarEstadoBotones() se ejecuta
    ↓
Los botones cambien de color EN EL ACTO
    ↓
PRESIONAS NEXT LEVEL
    ↓
Se carga el Lobby
    ↓
LobbySceneManager.OnEnable() se ejecuta
    ↓
NivelButtonManager.OnEnable() se ejecuta
    ↓
ActualizarEstadoBotones() se ejecuta
    ↓
Los botones están DESBLOQUEADOS ✓
```

Si falla en algún punto, los logs te lo dirán.

---

## 🚀 DEBUG RÁPIDO

Presiona estas teclas en Play Mode:

- **D**: Actualizar debug en pantalla
- **T**: Marcar Tutorial completado
- **U**: Marcar Nivel1 completado
- **R**: Reiniciar TODO

Mira los logs:
```
[Niveles]           → Sistema de niveles
[NivelButtonManager] → Actualización de botones
[LobbySceneManager]  → Cambio de escenas
```

---

## 📞 SI AÚN NO FUNCIONA

1. Abre **NivelesDiagnostico** → "DIAGNOSTICO COMPLETO"
2. Copia TODO lo que sale en la consola
3. Verifica:
   - ¿Aparece "Event OnNivelCompletado tiene X suscriptores"?
   - ¿Dice "Tutorial completado: True"?
   - ¿Dice "Nivel1 disponible: True"?

Si TODO está bien aquí pero no funciona en juego → **El problema está en WaveSpawner/RocaHandler**
