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
    private bool haySismo = false;
    private float alturaActualCalculada;

    void Start()
    {
        alturaActualCalculada = alturaParado;
        if (cuerpoFisico == null) cuerpoFisico = GetComponent<CharacterController>();
        
        // NUEVO: Si te olvidaste de asignar el motor, lo buscamos
        if (motorDeMovimiento == null) motorDeMovimiento = GetComponent<MovimientoVR>();
    }

    void Update()
    {
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
        estaAgachado = false; 
        if (Input.GetKey(teclaAgacharse)) estaAgachado = true;
        try {
            if (Input.GetAxis(ejeGatillo) > 0.3f) estaAgachado = true;
        } catch {}
    }

    public void ActivarSismo(bool estado)
    {
        haySismo = estado;
    }
}