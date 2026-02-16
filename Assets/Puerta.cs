using UnityEngine;

public class PuertaInteractiva : MonoBehaviour
{
    public float anguloAbierta = 90f;
    public float velocidad = 2f;
    private bool abierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    [Header("Interbloqueo de Seguridad")]
    // Arrastrá acá el cubo invisible que tiene el script DetectorObstaculos
    public DetectorObstaculos detectorPasillo; 

    [Header("Sonidos")]
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;
    public AudioClip sonidoBloqueado; // Un sonido de "traba" para cuando hay cajas
    public AudioSource parlante;

    void Start()
    {
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, anguloAbierta, 0);
    }

    void Update()
    {
        Quaternion objetivo = abierta ? rotacionAbierta : rotacionCerrada;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidad);
    }

    public void AlternarPuerta()
    {
        // 1. CHEQUEO DE SEGURIDAD (Protocolo Mendoza - Acto 1)
        // Si la puerta está cerrada e intentamos abrirla, verificamos el pasillo
        if (!abierta && detectorPasillo != null && !detectorPasillo.EstaDespejado())
        {
            Debug.Log("Puerta bloqueada: Hay obstáculos en la vía de evacuación.");
            
            // Feedback sonoro de error
            if (parlante != null && sonidoBloqueado != null)
            {
                parlante.PlayOneShot(sonidoBloqueado);
            }
            
            // Aquí podrías disparar un mensaje al Canvas: "Despeje el pasillo antes de continuar"
            return; // Cortamos la ejecución: la puerta NO se abre
        }

        // 2. LÓGICA NORMAL
        abierta = !abierta;
        
        if (parlante != null)
        {
            AudioClip clipATocar = abierta ? sonidoAbrir : sonidoCerrar;
            if (clipATocar != null)
            {
                parlante.PlayOneShot(clipATocar);
            }
        }
    }
}