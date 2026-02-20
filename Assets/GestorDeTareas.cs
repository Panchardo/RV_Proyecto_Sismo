using UnityEngine;
using TMPro; // <-- NUEVO: Llamamos a la librería de TextMeshPro

public class GestorDeTareas : MonoBehaviour
{
    [Header("Referencia al Texto UI")]
    public TextMeshProUGUI textoTareas; // <-- NUEVO: Ahora acepta tu texto TMP

    // Variables internas de progreso
    private bool sismoSuperado = false;
    private bool linternaEncontrada = false;
    private bool valvulasCerradas = false;
    private bool evacuacionExitosa = false;

    void Start()
    {
        ActualizarPanel(); // Arranca mostrando todo incompleto
    }

    // --- ESTAS FUNCIONES LAS VAMOS A LLAMAR DESDE TUS OTROS SCRIPTS ---
    
    public void TareaSismoSuperado()
    {
        sismoSuperado = true;
        ActualizarPanel();
    }

    public void TareaLinternaEncontrada()
    {
        linternaEncontrada = true;
        ActualizarPanel();
    }

    public void TareaValvulasCerradas()
    {
        valvulasCerradas = true;
        ActualizarPanel();
    }

    public void TareaEvacuacionExitosa()
    {
        evacuacionExitosa = true;
        ActualizarPanel();
    }

    // --- ACTUALIZACIÓN VISUAL DEL HOLOGRAMA ---
    private void ActualizarPanel()
    {
        if (textoTareas == null) return;

        string contenido = "<b><color=#00FFFF>PROTOCOLO DE EMERGENCIA</color></b>\n\n";

        // TMP usa códigos Hexadecimales para los colores, así que quedan mucho más brillantes
        contenido += sismoSuperado ? "<color=#00FF00>[✓]</color> Sobrevivir al sismo\n" : "<color=#FF0000>[ ]</color> Agacharse y cubrirse\n";
        contenido += linternaEncontrada ? "<color=#00FF00>[✓]</color> Equipar linterna\n" : "<color=#FF0000>[ ]</color> Buscar equipo de emergencia\n";
        contenido += valvulasCerradas ? "<color=#00FF00>[✓]</color> Cortar gas y agua\n" : "<color=#FF0000>[ ]</color> Cerrar suministros\n";
        contenido += evacuacionExitosa ? "<color=#00FF00>[✓]</color> Llegar al punto de encuentro" : "<color=#FF0000>[ ]</color> Evacuar a zona segura";

        textoTareas.text = contenido;
    }
}