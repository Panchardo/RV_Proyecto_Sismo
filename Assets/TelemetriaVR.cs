using UnityEngine;
using UnityEngine.UI; // Necesario para detectar el componente Button

public class TelemetriaVR : MonoBehaviour
{
    [Header("Configuración del Sensor")]
    public float rango = 20f;
    public LayerMask capasAIgnorar;

    private string reporte = "Buscando...";
    private string ultimoBoton = "Ninguno";

    void Update()
    {
        // 1. ESCÁNER DE JOYSTICK (Mantenemos el Debug)
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick button " + i))
                ultimoBoton = "Botón " + i;
        }

        // 2. LÓGICA DEL RAYO
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rango, ~capasAIgnorar))
        {
            reporte = $"MIRANDO: {hit.collider.name}\nDISTANCIA: {hit.distance:F2}m";

            // --- ESTO ES LO QUE BUSCABAS: LÓGICA DE SELECCIÓN ---
            // Si apretás el 4, el 5 o el touch de la pantalla (Mouse0)
            if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown("joystick button 5") || Input.GetMouseButtonDown(0))
            {
                // Buscamos si el objeto que miramos tiene un componente Botón de Unity
                Button botonUI = hit.collider.GetComponent<Button>();
                
                if (botonUI != null)
                {
                    Debug.Log("¡Accionando botón vía Joystick!");
                    botonUI.onClick.Invoke(); // Esto llama a 'IniciarJuego' o 'CerrarApp'
                }
            }
        }
        else
        {
            reporte = "MIRA: Al aire";
        }
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.black;
        GUI.Box(new Rect(10, 10, 450, 150), "--- MONITOR VR INTERACTIVO ---");
        GUIStyle estilo = new GUIStyle();
        estilo.fontSize = 25;
        estilo.normal.textColor = Color.white;
        GUI.Label(new Rect(25, 40, 400, 100), $"{reporte}\nINPUT: {ultimoBoton}", estilo);
    }
}