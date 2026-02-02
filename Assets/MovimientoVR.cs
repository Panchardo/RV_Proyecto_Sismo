using UnityEngine;

// Esto obliga a que el objeto tenga sí o sí un CharacterController
[RequireComponent(typeof(CharacterController))]
public class MovimientoVR : MonoBehaviour
{
    
    public float velocidad = 3.0f;
    public Transform camaraTransform; 
    
    // Referencia interna al motor físico
    private CharacterController characterController;

    void Start()
    {
        // Buscamos el componente automáticamente
        characterController = GetComponent<CharacterController>();
        
        // Forzar giroscopio (por si acaso)
        Input.gyro.enabled = true; 
    }

    void Update()
    {
        // SEGURIDAD: Referencia a la cámara
        if (camaraTransform == null) {
            if (Camera.main != null) camaraTransform = Camera.main.transform;
            else return;
        }

        // 1. LEER INPUTS
        float moverX = Input.GetAxis("Horizontal");
        float moverZ = Input.GetAxis("Vertical");

        // 2. CALCULAR DIRECCIÓN (Basado en a dónde mirás)
        Vector3 frente = camaraTransform.forward;
        Vector3 derecha = camaraTransform.right;

        // Aplanamos para no volar hacia arriba si mirás el cielo
        frente.y = 0;
        derecha.y = 0;
        frente.Normalize();
        derecha.Normalize();

        // Vector de movimiento deseado
        Vector3 deseoDeMovimiento = (frente * moverZ) + (derecha * moverX);

        // 3. APLICAR GRAVEDAD SIMPLE
        // El CharacterController no tiene gravedad por defecto. 
        // Le agregamos un empujoncito constante para abajo para que no flote si sube un escalón.
        deseoDeMovimiento.y = -9.8f; 

        // 4. MOVER USANDO FÍSICA
        // Move() respeta las paredes. Si chocás, te frena.
        if (deseoDeMovimiento.magnitude > 0.1f || !characterController.isGrounded)
        {
            characterController.Move(deseoDeMovimiento * velocidad * Time.deltaTime);
        }
    }
}