using UnityEngine;

public class MovimientoVR : MonoBehaviour
{
    public float velocidad = 3.0f;
    public Transform camaraTransform; // Arrastrá la cámara aquí en el Inspector

    void Update()
    {
        // SEGURIDAD 1: Si hay un tirón de lag (el frame tarda más de 0.1s), 
        // abortamos para que no se quede "pegado" el movimiento anterior.
        if (Time.unscaledDeltaTime > 0.1f) return;

        // SEGURIDAD 2: Referencia a la cámara
        if (camaraTransform == null) {
            if (Camera.main != null) camaraTransform = Camera.main.transform;
            else return;
        }

        // 1. Leemos el Joystick
        float moverX = Input.GetAxis("Horizontal");
        float moverZ = Input.GetAxis("Vertical");

        // 2. Direcciones basadas en la mirada de la cámara
        Vector3 direccionFrente = camaraTransform.forward;
        Vector3 direccionDerecha = camaraTransform.right;

        // 3. Aplanamos el vector para no volar
        direccionFrente.y = 0;
        direccionDerecha.y = 0;
        direccionFrente.Normalize();
        direccionDerecha.Normalize();

        // 4. Calculamos el movimiento final
        Vector3 movimiento = (direccionFrente * moverZ) + (direccionDerecha * moverX);

        // 5. Aplicamos el movimiento
        // Solo movemos si el joystick tiene un valor mínimo (evita el drift)
        if (movimiento.magnitude > 0.05f) {
            transform.position += movimiento * velocidad * Time.deltaTime;
        }
    }
    void Start()
{
    Input.gyro.enabled = true; // Forzamos al celular a prender el sensor
}
}