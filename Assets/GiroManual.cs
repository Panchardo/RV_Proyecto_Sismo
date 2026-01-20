using UnityEngine;

public class ControlCamaraJoystick : MonoBehaviour
{
    public float sensibilidad = 100f;
    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        // Bloqueamos el cursor para que no se salga de la ventana mientras testeas
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. Leemos los ejes (esto sirve para Mouse y para Joystick configurado)
        float lookX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float lookY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        // 2. Calculamos la rotación
        rotacionY += lookX;
        rotacionX -= lookY;

        // 3. Limitamos la rotación vertical para no dar la vuelta carnero (clamping)
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

        // 4. Aplicamos la rotación
        transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);

        // TIP: Si querés que el cuerpo del jugador gire con la cámara, 
        // deberías rotar el 'Parent' en el eje Y, pero por ahora esto te da visión 360.
    }
}