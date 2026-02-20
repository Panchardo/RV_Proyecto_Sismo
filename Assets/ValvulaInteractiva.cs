using UnityEngine;
using System.Collections;

public class ValvulaInteractiva : MonoBehaviour
{
    [Header("Componentes de la Válvula")]
    public Transform manette; 
    // Array interno para guardar TODAS las mallas de la válvula (manija y cuerpo)
    private Renderer[] todosLosRenderers; 

    [Header("Configuración de Brillo (Emission)")]
    [ColorUsage(true, true)] 
    public Color colorAlerta = new Color(0f, 1f, 0f, 1f); // Verde por defecto

    [Header("Configuración de Rotación")]
    private float gradosACerrar = 90f;
    public float velocidadRotacion = 3f;
    private Vector3 ejeDeRotacion = new Vector3(0, 0, 1); 

    [Header("Sonido")]
    public AudioSource parlante;
    public AudioClip sonidoCerrar;

    public bool estaCerrada = false;
    private bool emergenciaActivada = false;
    private bool estaAnimando = false;
    private Coroutine corrutinaTitileo;

    void Start()
    {
        // Magia: Busca todos los Renderer dentro de "Valve", "manette" y "vanne"
        todosLosRenderers = GetComponentsInChildren<Renderer>();
        ApagarBrillo(); 
    }

    public void IniciarAlertaEmergencia()
    {
        emergenciaActivada = true;
        if (todosLosRenderers.Length > 0)
        {
            corrutinaTitileo = StartCoroutine(TitilarEmission());
        }
        Debug.Log("Válvula: ¡Alerta activada! Esperando cierre.");
    }

    // ATENCIÓN ACÁ: Si tu script de jugador usa un nombre distinto para interactuar, 
    // cambiale el nombre a esta función (ej: public void Interactuar())
    public void CerrarValvula() 
    {
        // Si no hay emergencia, no hacemos nada y avisamos por consola
        if (!emergenciaActivada) 
        {
            Debug.Log("La válvula está bien, no hay emergencia aún.");
            return;
        }

        if (emergenciaActivada && !estaCerrada && !estaAnimando)
        {
            StartCoroutine(AnimacionCerrar());
        }
    }

    IEnumerator AnimacionCerrar()
    {
        estaAnimando = true;
        
        if (corrutinaTitileo != null) StopCoroutine(corrutinaTitileo);
        ApagarBrillo();

        if (parlante != null && sonidoCerrar != null) parlante.PlayOneShot(sonidoCerrar);

        Quaternion rotacionInicial = manette.localRotation;
        Quaternion rotacionFinal = rotacionInicial * Quaternion.Euler(ejeDeRotacion * gradosACerrar);

        float tiempo = 0;
        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidadRotacion;
            manette.localRotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, tiempo);
            yield return null;
        }

        estaCerrada = true;
        estaAnimando = false;
        Debug.Log("✅ Válvula asegurada.");
    }

    IEnumerator TitilarEmission()
    {
        // Activamos Emission en todos los materiales
        foreach (Renderer ren in todosLosRenderers)
        {
            ren.material.EnableKeyword("_EMISSION");
        }

        while (!estaCerrada)
        {
            float intensidad = (Mathf.Sin(Time.time * 4f) + 1f) / 2f; 
            Color colorActual = colorAlerta * (intensidad * 2f);

            // Le aplicamos el color titilante a TODAS las partes
            foreach (Renderer ren in todosLosRenderers)
            {
                ren.material.SetColor("_EmissionColor", colorActual);
            }
            yield return null;
        }
    }

    void ApagarBrillo()
    {
        foreach (Renderer ren in todosLosRenderers)
        {
            ren.material.SetColor("_EmissionColor", Color.black);
            ren.material.DisableKeyword("_EMISSION");
        }
    }
}