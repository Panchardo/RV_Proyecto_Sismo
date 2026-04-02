using UnityEngine;
using System.Collections;


public class SimuladorSismo : MonoBehaviour
{

    [Header("Configuración Réplica")]
    public float magnitudReplicaFisica = 8f; 
    public float magnitudReplicaCamara = 0.05f; // Más suave que el 0.1f original
    [Header("Referencias a otros scripts")]
    public ControlCabeza scriptCabeza; // Arrastrá el Player acá en el Inspector

    [Header("Audio del Terremoto")]
    public AudioSource parlanteTerremoto;

    [Header("Fuerza del sismo")]
    public float magnitudSismo = 20f; 
    private Rigidbody[] objetosAfectados; 
    public bool enZonaTerremoto = false;
    public MochilaEmergencia mochila;
    
    [Header("Apagón")]
    public GameObject[] lucesOficina;

    [Header("Visuales de Refugio")]
    public GameObject[] resaltadosVerdes; // <-- Arrastrá acá los objetos verdes
    private bool sismoIniciado = false; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        objetosAfectados = FindObjectsOfType<Rigidbody>();
        
        // Empezamos con las zonas verdes apagadas
        ApagarResaltados();
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.T) || enZonaTerremoto) && mochila.getMochilardaLista())
        {
            // --- NUEVO: PRENDER RESALTADOS AL EMPEZAR ---
            if (!sismoIniciado)
            {
                sismoIniciado = true;
                PrenderResaltados();
            }
            Temblar();
        }
    }

    void Temblar()
    {
        foreach (Rigidbody rb in objetosAfectados)
        {
             if(rb.isKinematic) continue;
             Vector3 direccionRandom = Random.insideUnitSphere;
             direccionRandom.y = 0; 
             Vector3 fuerzaFinal = direccionRandom.normalized * magnitudSismo;
             rb.AddForce(fuerzaFinal, ForceMode.Impulse);
        }
    }

    public void IniciarSonido() { if (parlanteTerremoto != null && !parlanteTerremoto.isPlaying) parlanteTerremoto.Play(); }
    public void FrenarSonido() { if (parlanteTerremoto != null) parlanteTerremoto.Stop(); }

    public void PrenderResaltados() { foreach (GameObject obj in resaltadosVerdes) if (obj != null) obj.SetActive(true); }
    public void ApagarResaltados() { foreach (GameObject obj in resaltadosVerdes) if (obj != null) obj.SetActive(false); }

    [Header("Configuración Réplica")]
    public float magnitudReplica = 8.0f; // Más suave que el sismo principal

    public void ProgramarReplica(float tiempoDeEspera)
    {
        StartCoroutine(RutinaReplica(tiempoDeEspera));
    }
    IEnumerator RutinaReplica(float espera)
    {
        Debug.Log($"Esperando {espera} segundos para la réplica...");
        yield return new WaitForSeconds(espera);

        Debug.Log("⚠️ ¡RÉPLICA INICIADA!");
        
        // 1. Guardamos las magnitudes originales para no perderlas
        float magnitudFisicaOriginal = magnitudSismo;
        float magnitudCamaraOriginal = 0f;
        
        if (scriptCabeza != null) 
        {
            magnitudCamaraOriginal = scriptCabeza.magnitudSismo;
            
            // 2. Le aplicamos la magnitud suave a la cámara y LA PRENDEMOS
            scriptCabeza.magnitudSismo = magnitudReplicaCamara;
            scriptCabeza.haySismo = true; 
        }

        // 3. Le aplicamos la magnitud suave a los muebles, prendemos sismo físico y sonido
        magnitudSismo = magnitudReplicaFisica;
        enZonaTerremoto = true;
        IniciarSonido();

        // 4. Duración de la réplica
        yield return new WaitForSeconds(4f); 

        // 5. Apagamos todo
        enZonaTerremoto = false;
        FrenarSonido();
        
        // 6. Restauramos los valores originales para que quede limpio
        magnitudSismo = magnitudFisicaOriginal; 
        if (scriptCabeza != null) 
        {
            scriptCabeza.haySismo = false; // APAGAMOS LA CÁMARA
            scriptCabeza.magnitudSismo = magnitudCamaraOriginal; 
        }
        
        Debug.Log("✅ Réplica terminada.");
    }
}