# 🎮 SISTEMA DE NIVELES - GUÍA DE CONFIGURACIÓN

## ¿Qué cambié?

### 1. **niveles.cs** 
- ✅ Añadido evento `OnNivelCompletado` que se dispara cuando completas un nivel
- ✅ Mejor logging para debug

### 2. **LevelButtonManager.cs**
- ✅ Ahora se suscribe al evento de niveles completados
- ✅ Se actualiza automáticamente cuando completas un nivel
- ✅ Validación mejorada con mensajes de error claros
- ✅ Mejor logging para debug

### 3. **LobbySceneManager.cs**
- ✅ Busca automáticamente el NivelButtonManager
- ✅ Puede forzar actualización de botones si es necesario

---

## 📋 SETUP EN INSPECTOR (IMPORTANTE!)

### PASO 1: Escena del Lobby

Necesitas estos GameObjects en tu escena del Lobby:

```
Canvas (el existente)
├── NivelButtonManager (nuevo GameObject con NivelButtonManager.cs)
├── [0] TutorialButton  ← Button para Tutorial
├── [1] Nivel1Button    ← Button para Nivel1
├── [2] Nivel2Button    ← Button para Nivel2
├── [3] Nivel3Button    ← Button para Nivel3
├── [4] Nivel4Button    ← Button para Nivel4
└── [5] Nivel5Button    ← Button para Nivel5

NivelesManager (nuevo GameObject con Niveles.cs) ← IMPORTANTE: DontDestroyOnLoad
```

### PASO 2: Configurar NivelButtonManager

1. Selecciona el GameObject **NivelButtonManager** en el Inspector
2. En el componente **NivelButtonManager**:
   - **Array "Nivel Buttons" Size**: 6
   - Arrastra los botones en ORDEN:
     - Element 0 = Tutorial Button
     - Element 1 = Nivel1 Button
     - Element 2 = Nivel2 Button
     - Element 3 = Nivel3 Button
     - Element 4 = Nivel4 Button
     - Element 5 = Nivel5 Button

### PASO 3: Configurar cada Button

Cada botón debe tener:
- ✅ Componente **Button** (UI)
- ✅ Componente **Image**
- ✅ Hijo **Text** (TextMeshPro)
- ✅ En el evento OnClick(), conecta a **Canvas → Button_Scene_Changer → onClickEvent()**
- ✅ En Button_Scene_Changer, asigna el nombre de la escena del nivel

### PASO 4: Crear NivelesManager

1. En la escena del Lobby, crea un GameObject vacío
2. Nómbralo **"NivelesManager"**
3. Adjunta el script **Niveles.cs**
4. ✅ El script automáticamente hará DontDestroyOnLoad

---

## ✅ VERIFICAR QUE FUNCIONA

### En Editor (Play Mode)

1. Abre la **Console** de Unity
2. Deberías ver:
   ```
   [Niveles] Sistema de progresión inicializado
   [NivelButtonManager] ✓ Suscrito a evento OnNivelCompletado
   [NivelButtonManager] Actualizando estado de botones
   ```

3. Presiona **D** para ver el debug de niveles
   - Tutorial debe estar disponible (✓ DISPONIBLE)
   - Otros niveles deben estar bloqueados (✗ BLOQUEADO)

4. Presiona **T** para marcar Tutorial como completado
   - Los botones deben actualizarse
   - Nivel1 debe pasar a ✓ DISPONIBLE

### En Juego

1. Completa un nivel
2. Verás "Victory" en pantalla
3. Haz clic en "Next Nivel"
4. Vuelves al Lobby
5. **Los botones deben estar actualizados automáticamente**

---

## 🐛 TROUBLESHOOTING

### Error: "No se han asignado botones en el Inspector"
- **Problema**: El array de NivelButtons está vacío
- **Solución**: 
  1. Selecciona NivelButtonManager
  2. En Inspector, cambia el Size de "Nivel Buttons" a 6
  3. Arrastra los 6 botones en orden

### Error: "Niveles.Instance es null"
- **Problema**: No existe NivelesManager o Niveles.cs no está adjunto
- **Solución**:
  1. Crea un GameObject llamado "NivelesManager"
  2. Adjunta Niveles.cs
  3. ✅ Automáticamente hará DontDestroyOnLoad

### Los botones no se actualizan después de completar un nivel
- **Problema**: El evento no se dispara o no se recibe
- **Solución**: Verifica que:
  1. RocaHandler.ShowVictory() se llama cuando ganas
  2. WaveSpawner.FinalizarNivel() se llama al matar todos
  3. Mira los logs: busca "✓ Nivel completado"

### Los botones aparecen grises/bloqueados siempre
- **Problema**: Posiblemente PlayerPrefs no está guardando correctamente
- **Solución**: 
  1. Presiona R en debug para reiniciar progreso
  2. Presiona T para marcar Tutorial completado
  3. Mira los logs de NivelButtonManager

---

## 🔑 PUNTOS CLAVE

1. **Tutorial es siempre el primero y siempre disponible**
2. **Para jugar Nivel1, necesitas completar Tutorial**
3. **Para jugar Nivel2, necesitas completar Nivel1**
4. **Los botones se actualizan automáticamente cuando completas un nivel**
5. **Todo se guarda en PlayerPrefs**

---

## 📞 DEBUGGER RÁPIDO

En cualquier escena con NivelesDebugger, presiona:
- **D** = Toggle Debug Pantalla
- **T** = Marcar Tutorial Completado
- **L** = Marcar Nivel1 Completado
- **R** = Resetear todo el progreso

---

## ¿Dudas?

Revisa los logs de la consola. He mejorado mucho el logging para que sea fácil seguir el flujo.

Busca mensajes como:
- `[Niveles]` - Sistema de niveles
- `[NivelButtonManager]` - Actualización de botones
- `[Button_Scene_Changer]` - Cambio de escenas
