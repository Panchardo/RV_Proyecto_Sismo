using UnityEngine;

public class ControlCabeza : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cabeza; 
    public CharacterController cuerpoFisico;
    
    // NUEVO: Referencia al script de caminar para frenarlo
    public MovimientoVR motorDeMovimiento; 

    [Header("Configuración Agachado")]
    public float alturaParado = 1.7f;      
    public float alturaAgachado = 0.9f; 
    public float velocidadAgachado = 5f; // Velocidad de la animación de agacharse

    [Header("Configuración Cápsula")]
    public float alturaCapsulaParado = 2.0f;
    public float alturaCapsulaAgachado = 1.0f;

    // NUEVO: Velocidades de caminata
    [Header("Velocidades de Movimiento")]
    public float velocidadCaminar = 3.0f; // Rápido
    public float velocidadSigilo = 1.0f;  // Lento (agachado)

    [Header("Configuración Sismo")]
    public float magnitudSismo = 0.1f;   
    public KeyCode teclaSismo = KeyCode.T; 

    [Header("Inputs")]
    public KeyCode teclaAgacharse = KeyCode.C;
    public string ejeGatillo = "Agacharse"; 

    // Variables internas
    private bool estaAgachado = false;
    public bool haySismo = false;
    private float alturaActualCalculada;

    [Header("Detección de Techo")]
    // (Ya no usamos distanciaRayo pública porque la calculamos matemáticamente de forma más precisa)
    public LayerMask capasObstaculos; // Para que ignore al Player y solo vea mesas/techos
    public bool hayAlgoArriba = false;

    void Start()
    {
        alturaActualCalculada = alturaParado;
        if (cuerpoFisico == null) cuerpoFisico = GetComponent<CharacterController>();
        
        // NUEVO: Si te olvidaste de asignar el motor, lo buscamos
        if (motorDeMovimiento == null) motorDeMovimiento = GetComponent<MovimientoVR>();
    }

    void Update()
    {
        // --- SENSOR RECALIBRADO ---
        // El origen ahora es la altura de tu cabeza estando agachado
        Vector3 origenSensor = transform.position + Vector3.up * alturaAgachado;
        float radioSensor = 0.15f; // Más fino para no chocar con paredes
        float distanciaChequeo = (alturaParado - alturaAgachado) + 0.1f; // Solo chequea la diferencia de altura

        hayAlgoArriba = Physics.SphereCast(origenSensor, radioSensor, Vector3.up, out RaycastHit hit, distanciaChequeo, capasObstaculos);

        // 1. INPUT
        ProcesarEntrada();

        // 2. CONTROLAR AL OTRO SCRIPT (El cambio de velocidad)
        // Si estamos agachados, le inyectamos la velocidad lenta. Si no, la rápida.
        if (motorDeMovimiento != null)
        {
            motorDeMovimiento.velocidad = estaAgachado ? velocidadSigilo : velocidadCaminar;
        }

        // 3. ALTURA VISUAL (Ojos)
        float objetivoY = estaAgachado ? alturaAgachado : alturaParado;
        alturaActualCalculada = Mathf.Lerp(alturaActualCalculada, objetivoY, Time.deltaTime * velocidadAgachado);

        // 4. ALTURA FÍSICA (Hitbox)
        float alturaCapsulaObjetivo = estaAgachado ? alturaCapsulaAgachado : alturaCapsulaParado;
        cuerpoFisico.height = Mathf.Lerp(cuerpoFisico.height, alturaCapsulaObjetivo, Time.deltaTime * velocidadAgachado);
        
        Vector3 centroNuevo = cuerpoFisico.center;
        centroNuevo.y = cuerpoFisico.height / 2f;
        cuerpoFisico.center = centroNuevo;

        // 5. SISMO
        Vector3 offsetVibracion = Vector3.zero;
        if (Input.GetKey(teclaSismo) || haySismo)
        {
            offsetVibracion = Random.insideUnitSphere * magnitudSismo;
            offsetVibracion.y *= 0.5f; 
        }

        cabeza.localPosition = new Vector3(offsetVibracion.x, alturaActualCalculada + offsetVibracion.y, offsetVibracion.z);
    }

    void ProcesarEntrada()
    {
        // 1. Primero leemos qué quiere hacer el usuario (la señal de control)
        bool quiereEstarAgachado = Input.GetKey(teclaAgacharse);
        try {
            if (Input.GetAxis(ejeGatillo) > 0.3f) quiereEstarAgachado = true;
        } catch {}

        // 2. Aplicamos el enclavamiento físico
        if (estaAgachado && hayAlgoArriba) 
        {
            // Si YA estás agachado y hay un techo, ignoramos el joystick/teclado.
            // Te quedás agachado por seguridad.
            estaAgachado = true; 
        }
        else
        {
            // En cualquier otro caso (estás parado, o estás agachado pero saliste de la mesa),
            // el personaje hace exactamente lo que mande el joystick/teclado.
            estaAgachado = quiereEstarAgachado;
        }
    }

    public void ActivarSismo(bool estado)
    {
        haySismo = estado;
    }

    // --- DEBUG VISUAL ---
    void OnDrawGizmos()
    {
        // Dibujamos exactamente lo mismo que calcula el sensor para poder verlo en la pestaña Scene
        Vector3 origenSensor = transform.position + Vector3.up * alturaAgachado;
        float radioSensor = 0.15f;
        float distanciaChequeo = (alturaParado - alturaAgachado) + 0.1f;

        // Si detecta algo se pone ROJO, si está libre VERDE
        Gizmos.color = hayAlgoArriba ? Color.red : Color.green; 
        
        // Dibuja la línea y la esfera al final
        Gizmos.DrawLine(origenSensor, origenSensor + Vector3.up * distanciaChequeo);
        Gizmos.DrawWireSphere(origenSensor + Vector3.up * distanciaChequeo, radioSensor);
    }
}