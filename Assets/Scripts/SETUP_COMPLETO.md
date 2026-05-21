# 🎮 SISTEMA DE NIVELES - GUÍA PASO A PASO

## 🔴 PROBLEMA ACTUAL: TODOS LOS NIVELES ESTÁN DESBLOQUEADOS

### Causa: Los botones NO se están deshabilitando visualmente

---

## ✅ SOLUCIÓN: CONFIGURACIÓN CORRECTA EN INSPECTOR

### ⚠️ IMPORTANTE: Lee esto completo antes de cambiar nada

---

## PASO 1️⃣: Validar que existe Niveles.cs

1. En la escena del **Lobby/Menú**, busca un GameObject llamado `"NivelesManager"`
2. Si no existe, **crea uno nuevo**:
   - Click derecho en Hierarchy → "Create Empty"
   - Nómbralo `"NivelesManager"`
   - Adjúntale el script **Niveles.cs**
   - ✅ Automáticamente hará `DontDestroyOnLoad`

---

## PASO 2️⃣: Configurar NivelButtonManager

### En el Hierarchy:

1. Encuentra el **Canvas** en tu escena
2. Debajo del Canvas, crea un GameObject nuevo:
   - Click derecho en Canvas → "Create Empty"
   - Nómbralo `"NivelesPanelManager"`
   - **Adjúntale el script `NivelButtonManager.cs`**

### En el Inspector (muy importante):

1. Selecciona `"NivelesPanelManager"`
2. Mira el componente **NivelButtonManager**
3. En el campo **"Nivel Buttons"**:
   - ⚠️ Si dice "Size: 0", **cambia a 6**
   - Aparecerán 6 slots: Element 0, Element 1, Element 2...

---

## PASO 3️⃣: Asignar los Botones (ESTO ES CRÍTICO)

En el **Inspector de NivelesPanelManager**, en el componente **NivelButtonManager**, "Nivel Buttons":

**Arrastra los botones en este ORDEN EXACTO:**

```
Element 0 = Button de TUTORIAL
Element 1 = Button de NIVEL1
Element 2 = Button de NIVEL2
Element 3 = Button de NIVEL3
Element 4 = Button de NIVEL4
Element 5 = Button de NIVEL5
```

**¿CÓMO arrastar?**
1. En el Hierarchy, busca cada botón
2. Manteniendo presionado, arrástralo al slot correspondiente en el Inspector

✅ Si lo hiciste bien, en el Play Mode deberías ver logs como:
```
[NivelButtonManager] Bot 0 (Tutorial): interactable = true
[NivelButtonManager] Bot 1 (Nivel1): interactable = false
```

---

## PASO 4️⃣: Configurar cada Botón (si no tiene Button_Scene_Changer)

Cada botón **DEBE tener**:

1. **Componente `Button`** ✓ (debería tener)
2. **Componente `Image`** ✓ (para los colores)
3. **Un hijo con `TextMeshProUGUI`** ✓ (el texto del botón)
4. **El script `Button_Scene_Changer`** ⚠️ (VERIFICA QUE LO TIENE)
   - Click derecho en el botón → "Add Component" → "Button Scene Changer"
   - En el field **"Scene Name"**, escribe el nombre de la escena del nivel
   - Ejemplos: `"Tutorial"`, `"Nivel1"`, `"Nivel2"`, etc.

5. **En el evento `On Click()`**:
   - Debe estar conectado a: **Button_Scene_Changer → onClickEvent()**
   - Si no está, ajusta el Canvas que debe tener **LobbySceneManager**

---

## PASO 5️⃣: Verificar que LobbySceneManager existe

En el **Canvas**, debe haber un componente **LobbySceneManager**:

1. Selecciona el Canvas
2. En el Inspector, busca el script **LobbySceneManager**
3. Si no lo ves, añádelo: "Add Component" → "LobbySceneManager"

---

## 🧪 PRUEBAS RÁPIDAS

### Test 1: Ver el Setup correctamente
1. Abre la consola (Ctrl+Shift+C)
2. En el Hierarchy, busca cualquier GameObject
3. Adjúntale **NivelesValidador.cs**
4. En el Inspector, click en "Validar Setup Completo"
5. Mira la consola para saber qué falta

### Test 2: Debug en Juego
1. Presiona **D** para ver el debug
   - Tutorial debe estar ✓ DESBLOQUEADO
   - Nivel1 debe estar ✗ BLOQUEADO

2. Presiona **T** para marcar Tutorial completado
   - Los botones deben cambiar de color
   - Nivel1 debe pasar a ✓ DESBLOQUEADO

3. Presiona **U** para marcar Nivel1 completado
   - Nivel2 debe desbloquearse

4. Presiona **R** para reiniciar todo

### Test 3: Verificar bloques en botones
1. En Play Mode
2. Mira los botones en la pantalla
3. Los que estén **grises** = bloqueados ✗
4. Los que estén **blancos** = desbloqueados ✓

Si **todos son blancos**, entonces el problema es que **NivelButtonManager no se está ejecutando**

---

## 🐛 SOLUCIÓN RÁPIDA SI SIGUE SIN FUNCIONAR

### Problema: "Los botones siguen siendo blancos"

**Checklist:**
- [ ] ¿Existe el GameObject "NivelesManager" con Niveles.cs?
- [ ] ¿Existe el GameObject "NivelesPanelManager" con NivelButtonManager.cs?
- [ ] ¿Están asignados los 6 botones en "Nivel Buttons" del Inspector?
- [ ] ¿Los botones tienen Button_Scene_Changer adjunto?
- [ ] ¿Los nombres de escena están correctos en Button_Scene_Changer?

**Si todo está bien pero sigue sin funcionar:**

1. En Unity, en la escena del Lobby:
   - Click derecho en Hierarchy → "Create Empty"
   - Adjúntale **NivelesValidador.cs**
   - En el Inspector, click en "Validar Setup Completo"
   - **La consola te dirá exactamente qué falta**

---

## 📞 SCRIPTS DISPONIBLES PARA DEBUG

- **NivelesDebugUI.cs** - Muestra estado en pantalla (ojo: TextMeshPro required)
- **NivelesValidador.cs** - Validador automático en consola
- **NivelesDebugger.cs** - Debugger antiguo (compatible)

Todos responden a:
- **D** = Actualizar debug
- **T** = Marcar Tutorial completado
- **U** = Marcar Nivel1 completado
- **R** = Reiniciar progreso
