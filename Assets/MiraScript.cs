using UnityEngine;
using UnityEngine.UI;

public class MiraInteractiva : MonoBehaviour
{
    [Header("Configuración Física")]
    public float distanciaMaxima = 20f; 
    public GameObject miraObjeto; 
    public LayerMask capasAignorar; 

    [Header("Interfaz (HUD)")]
    public Text textoDebug; // El componente Text de tu Canvas

    [Header("Colores")]
    public Color colorReposo = Color.blue;
    public Color colorActivo = Color.green;

    private Renderer miraRenderer;

    void Start()
    {
        // Configuramos la mira visual
        if (miraObjeto != null)
        {
            miraRenderer = miraObjeto.GetComponent<Renderer>();
            // Evitamos que la mira se detecte a sí misma
            if (miraObjeto.GetComponent<Collider>())
                miraObjeto.GetComponent<Collider>().enabled = false;
        }

        // Limpiamos cualquier texto previo al arrancar
        if (textoDebug != null) textoDebug.text = "Buscando colisiones...";
    }

    void LateUpdate()
    {
        if (miraObjeto == null || textoDebug == null) return;

        RaycastHit hit;
        // El "~" invierte la máscara para que ignore SOLO lo que selecciones
        bool detectado = Physics.Raycast(transform.position, transform.forward, out hit, distanciaMaxima, ~capasAignorar);
        
        // Debug visual para la PC
        Debug.DrawRay(transform.position, transform.forward * distanciaMaxima, Color.red);

        if (detectado)
        {
            // 1. ACTUALIZAR HUD CON INFO TÉCNICA
            // Aquí es donde verás si el botón tiene el Collider y el Tag correctos
            textoDebug.text = $"<b>OBJETO:</b> {hit.collider.name}\n" +
                              $"<b>TAG:</b> {hit.collider.tag}\n" +
                              $"<b>LAYER:</b> {LayerMask.LayerToName(hit.collider.gameObject.layer)}";

            // 2. POSICIONAR MIRA
            miraObjeto.transform.position = hit.point - (transform.forward * 0.1f);
            miraObjeto.transform.rotation = Quaternion.LookRotation(hit.normal);

            // 3. LÓGICA DE INTERACCIÓN
            if (hit.collider.CompareTag("Interactuable"))
            {
                PintarMira(colorActivo);
                // Fire1 suele ser el botón principal del joystick o el toque en pantalla
                if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0))
                {
                    Button btn = hit.collider.GetComponent<Button>();
                    if (btn != null) btn.onClick.Invoke();
                }
            }
            else 
            { 
                PintarMira(colorReposo); 
            }
        }
        else
        {
            // Si el rayo no toca nada, lo indicamos para saber que el script está vivo
            textoDebug.text = "<color=red>MIRA: Sin colisión</color>";
            
            // Mira en posición de reposo
            miraObjeto.transform.position = transform.position + (transform.forward * distanciaMaxima);
            miraObjeto.transform.LookAt(transform.position);
            PintarMira(colorReposo);
        }
    }

    void PintarMira(Color color)
    {
        if (miraRenderer != null) miraRenderer.material.color = color;
    }
}