using UnityEngine;

public class MiraInteractiva : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaMaxima = 3f; // Distancia por defecto
    public GameObject miraObjeto; 
    
    [Header("Colores")]
    public Color colorReposo = Color.blue;
    public Color colorActivo = Color.green;

    private Renderer miraRenderer;

    void Start()
    {
        if (miraObjeto != null)
            miraRenderer = miraObjeto.GetComponent<Renderer>();
    }

    // USAMOS LATEUPDATE: Esto arregla que la mira "baile" o se descentre.
    // Se ejecuta DESPUÉS de que la cámara y el joystick terminaron de moverse.
    void LateUpdate()
    {
        if (miraObjeto == null) return;

        RaycastHit hit;
        // Lanzamos rayo desde la posición de la Cabeza hacia adelante
        bool detectado = Physics.Raycast(transform.position, transform.forward, out hit, distanciaMaxima);

        if (detectado)
        {
            // 1. POSICIONAMIENTO INTELIGENTE
            // Ponemos la mira en el punto de impacto...
            // PERO la traemos un poquito hacia nosotros (- transform.forward * 0.05f)
            // para que no quede "enterrada" en la textura de la pared.
            miraObjeto.transform.position = hit.point - (transform.forward * 0.05f);

            // 2. ROTACIÓN (Opcional pero queda Pro)
            // Hacemos que la "chapa" de la mira se acomode al ángulo de la pared
            miraObjeto.transform.rotation = Quaternion.LookRotation(hit.normal);

            // 3. LOGICA DE COLOR
            if (hit.collider.CompareTag("Interactuable"))
            {
                PintarMira(colorActivo);
                if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) // Soporte para toque en pantalla también
                {
                   // Lógica de disparo/acción
                }
            }
            else
            {
                PintarMira(colorReposo);
            }
        }
        else
        {
            // SI NO TOCA NADA:
            // La ponemos flotando a la distancia máxima
            miraObjeto.transform.position = transform.position + (transform.forward * distanciaMaxima);
            
            // La rotamos para que mire al jugador
            miraObjeto.transform.LookAt(transform.position);
            
            PintarMira(colorReposo);
        }
    }

    void PintarMira(Color color)
    {
        if (miraRenderer != null) miraRenderer.material.color = color;
    }
}