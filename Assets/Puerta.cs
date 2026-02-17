using UnityEngine;

public class PuertaInteractiva : MonoBehaviour
{
    public float anguloAbierta = 90f;
    public float velocidad = 2f;
    private bool abierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    [Header("Interbloqueo Fase 1 (Cajas)")]
    public DetectorObstaculos detectorPasillo; 

    [Header("Interbloqueo Fase 3 (Evacuación)")]
    public bool esPuertaDeSalida = false; 
    public GameObject linternaDelJugador; 

    [Header("Iluminación Exterior (Solo Salida)")]
    public GameObject solDirectionalLight; // Arrastrá tu "Directional Light" acá

    [Header("Sonidos")]
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;
    public AudioClip sonidoBloqueado; 
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
        // 1. CHEQUEO DE SEGURIDAD FASE 1: Cajas en el pasillo
        if (!abierta && detectorPasillo != null && !detectorPasillo.EstaDespejado())
        {
            Debug.Log("Puerta bloqueada: Hay obstáculos en la vía de evacuación.");
            if (parlante != null && sonidoBloqueado != null) parlante.PlayOneShot(sonidoBloqueado);
            return; 
        }

        // 2. CHEQUEO DE SEGURIDAD FASE 3: Puerta a la calle sin linterna
        if (!abierta && esPuertaDeSalida)
        {
            if (linternaDelJugador == null || !linternaDelJugador.activeInHierarchy)
            {
                Debug.Log("Puerta bloqueada: ¡No podés evacuar a oscuras sin tu linterna!");
                if (parlante != null && sonidoBloqueado != null) parlante.PlayOneShot(sonidoBloqueado);
                return; 
            }
        }

        // 3. LÓGICA NORMAL DE APERTURA
        abierta = !abierta;
        
        // Apagamos la zona verde (Fase 1)
        if (abierta && detectorPasillo != null)
        {
            detectorPasillo.ApagarEfectoVisual();
        }

        // --- NUEVO: RESTAURAR LUZ AL ABRIR LA SALIDA ---
        if (abierta && esPuertaDeSalida)
        {
            Debug.Log("Abriendo salida. Restaurando iluminación global.");

            // Restauramos la luz ambiental y el color base de Unity
            RenderSettings.ambientIntensity = 1f; 
            RenderSettings.reflectionIntensity = 1f;
            RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f); 

            // Prendemos el sol principal
            if (solDirectionalLight != null)
            {
                solDirectionalLight.SetActive(true);
            }

            // Apagamos la linterna de tu mano automáticamente para que no quede prendida de día
            if (linternaDelJugador != null)
            {
                Light luzLinterna = linternaDelJugador.GetComponentInChildren<Light>();
                if (luzLinterna != null) luzLinterna.enabled = false;
            }
        }

        // 4. SONIDO
        if (parlante != null)
        {
            AudioClip clipATocar = abierta ? sonidoAbrir : sonidoCerrar;
            if (clipATocar != null) parlante.PlayOneShot(clipATocar);
        }
    }
}