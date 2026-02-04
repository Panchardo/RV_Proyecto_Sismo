using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovimientoVR : MonoBehaviour
{
    [Header("Configuración Básica")]
    public float velocidad = 3.0f; // Controlada por el otro script (ControlCabeza)
    public Transform camaraTransform; 

    [Header("Física de Salto")]
    public float fuerzaGravedad = -9.81f;
    public float alturaSalto = 1.2f; // Altura en metros (saltás 1 metro y pico)
    
    // Variables internas del motor físico
    private CharacterController characterController;
    private Vector3 velocidadVertical; // Acá guardamos la inercia de caída/subida
    private bool estaEnElSuelo;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Input.gyro.enabled = true; 
    }

    void Update()
    {
        // Seguridad por si perdiste la cámara en la mudanza
        if (camaraTransform == null) {
            if (Camera.main != null) camaraTransform = Camera.main.transform;
            else return;
        }

        // 1. CHEQUEO DE PISO (¿Estamos tocando el suelo?)
        estaEnElSuelo = characterController.isGrounded;

        if (estaEnElSuelo && velocidadVertical.y < 0)
        {
            // Si tocamos piso, reseteamos la caída. 
            // Ponemos -2f en vez de 0 para asegurar que el sensor detecte el suelo siempre.
            velocidadVertical.y = -2f;
        }

        // 2. MOVIMIENTO HORIZONTAL (Caminar)
        float moverX = Input.GetAxis("Horizontal");
        float moverZ = Input.GetAxis("Vertical");

        Vector3 frente = camaraTransform.forward;
        Vector3 derecha = camaraTransform.right;

        // Aplanamos vectores (para no volar hacia el cielo si mirás arriba)
        frente.y = 0;
        derecha.y = 0;
        frente.Normalize();
        derecha.Normalize();

        // Calculamos hacia dónde queremos ir caminando
        Vector3 movimientoCaminata = (frente * moverZ) + (derecha * moverX);
        
        // Mover el personaje (Plano Horizontal)
        characterController.Move(movimientoCaminata * velocidad * Time.deltaTime);

        // 3. LOGICA DE SALTO (Plano Vertical)
        // Input.GetButtonDown("Jump") suele ser la ESPACIADORA o el botón "X/A" del Joystick
        if (Input.GetButtonDown("Jump") && estaEnElSuelo)
        {
            // Fórmula física real: V = Raíz(Altura * -2 * Gravedad)
            velocidadVertical.y = Mathf.Sqrt(alturaSalto * -2f * fuerzaGravedad);
        }

        // 4. APLICAR GRAVEDAD (Caída)
        // La gravedad acelera con el tiempo (Time.deltaTime)
        velocidadVertical.y += fuerzaGravedad * Time.deltaTime;

        // Mover el personaje (Eje Y)
        characterController.Move(velocidadVertical * Time.deltaTime);
    }
}