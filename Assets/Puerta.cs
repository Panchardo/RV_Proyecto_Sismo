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

    [Header("Interbloqueo Fase 3 y 4 (Evacuación)")]
    public bool esPuertaDeSalida = false; 
    public GameObject linternaDelJugador; 
    
    // --- AHORA ES UN ARRAY (LISTA) DE VÁLVULAS ---
    public ValvulaInteractiva[] valvulasDeEmergencia; 

    [Header("Iluminación Exterior (Solo Salida)")]
    public GameObject solDirectionalLight; 

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
        // 1. CHEQUEO: Cajas
        if (!abierta && detectorPasillo != null && !detectorPasillo.EstaDespejado())
        {
            Debug.Log("Bloqueada: Obstáculos en la vía.");
            if (parlante != null && sonidoBloqueado != null) parlante.PlayOneShot(sonidoBloqueado);
            return; 
        }

        // 2. CHEQUEO: Linterna y Válvulas (Solo si es puerta de salida)
        if (!abierta && esPuertaDeSalida)
        {
            if (linternaDelJugador == null || !linternaDelJugador.activeInHierarchy)
            {
                Debug.Log("Bloqueada: ¡Necesitás la linterna!");
                if (parlante != null && sonidoBloqueado != null) parlante.PlayOneShot(sonidoBloqueado);
                return; 
            }

            // --- REVISAMOS TODAS LAS VÁLVULAS DE LA LISTA ---
            foreach (ValvulaInteractiva valvula in valvulasDeEmergencia)
            {
                if (valvula != null && !valvula.estaCerrada)
                {
                    Debug.Log("Bloqueada: ¡Falta cerrar suministros (Gas/Agua)!");
                    if (parlante != null && sonidoBloqueado != null) parlante.PlayOneShot(sonidoBloqueado);
                    return; // Si encuentra UNA sola abierta, corta todo y no abre la puerta
                }
            }
        }

        // 3. APERTURA NORMAL
        abierta = !abierta;
        
        if (abierta && detectorPasillo != null) detectorPasillo.ApagarEfectoVisual();

        // 4. RESTAURAR LUZ AL SALIR
        if (abierta && esPuertaDeSalida)
        {
            RenderSettings.ambientIntensity = 1f; 
            RenderSettings.reflectionIntensity = 1f;
            RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f); 

            if (solDirectionalLight != null) solDirectionalLight.SetActive(true);

            if (linternaDelJugador != null)
            {
                Light luzLinterna = linternaDelJugador.GetComponentInChildren<Light>();
                if (luzLinterna != null) luzLinterna.enabled = false;
            }
        }

        if (parlante != null)
        {
            AudioClip clipATocar = abierta ? sonidoAbrir : sonidoCerrar;
            if (clipATocar != null) parlante.PlayOneShot(clipATocar);
        }
    }
}