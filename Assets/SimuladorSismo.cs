using UnityEngine;

public class SimuladorSismo : MonoBehaviour
{
    [Header("Audio del Terremoto")]
    public AudioSource parlanteTerremoto;

    [Header("Fuerza del sismo")]
    public float magnitudSismo = 20f; // Fuerza del empujón (Subile si no se mueven)
    private Rigidbody[] objetosAfectados; 
    public bool enZonaTerremoto = false;
    public MochilaEmergencia mochila;
    [Header("Apagón")]
    // Arrastrá acá las luces principales de la oficina (Directional Light, luces de techo, etc.)
    public GameObject[] lucesOficina;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // 1. Llenamos el array automáticamente al iniciar el juego
        // FindObjectsOfType busca CADA objeto en la escena que tenga Rigidbody
        objetosAfectados = FindObjectsOfType<Rigidbody>();
        
        // Debug para que veas en consola cuántos encontró
        Debug.Log("Se encontraron " + objetosAfectados.Length + " objetos físicos.");
    }

    void Update()
    {
        // Mantené apretada la T para el sismo
        if ((Input.GetKey(KeyCode.T) || enZonaTerremoto) && mochila.getMochilardaLista())
        {
            Temblar();
        }
    }

    void Temblar()
    {
        // 2. Recorremos la lista uno por uno
        foreach (Rigidbody rb in objetosAfectados)
        {
             // Si el objeto es "Kinematic" (como quizás tu mano), no lo empujamos
             if(rb.isKinematic) continue;

             // 3. Vector random (X, Y, Z) entre -1 y 1
             Vector3 direccionRandom = Random.insideUnitSphere;
             
             // 4. Aplanamos la fuerza (Sismo horizontal)
             direccionRandom.y = 0; 
             // Normalizamos para que la dirección mida 1, y multiplicamos por magnitud
             Vector3 fuerzaFinal = direccionRandom.normalized * magnitudSismo;

             // 5. Aplicamos la fuerza. 
             // ForceMode.Force es continuo (como viento o empuje constante)
             // ForceMode.Impulse es golpes (como martillazos). Probá ambos.
             rb.AddForce(fuerzaFinal, ForceMode.Impulse);
        }
    }
    public void IniciarSonido()
    {
        // Chequeamos que exista el parlante y que no esté sonando ya
        if (parlanteTerremoto != null && !parlanteTerremoto.isPlaying)
        {
            parlanteTerremoto.Play();
        }
    }

    public void FrenarSonido()
    {
        if (parlanteTerremoto != null)
        {
            parlanteTerremoto.Stop(); // La función Stop() corta el audio instantáneamente
        }
    }
}