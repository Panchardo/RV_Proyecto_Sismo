using UnityEngine;

public class MovimientoVR : MonoBehaviour
{
    public float velocidad = 3.0f; // Velocidad de caminata

    void Update()
    {
        // 1. Leemos el Joystick de la ESP32 (o las flechas del teclado)
        float moverX = Input.GetAxis("Horizontal");
        float moverZ = Input.GetAxis("Vertical");

        // 2. Buscamos hacia dónde está mirando la cámara
        Vector3 direccionFrente = Camera.main.transform.forward;
        Vector3 direccionDerecha = Camera.main.transform.right;

        // 3. Ignoramos la altura (para no volar hacia el cielo si miras arriba)
        direccionFrente.y = 0;
        direccionDerecha.y = 0;
        direccionFrente.Normalize();
        direccionDerecha.Normalize();

        // 4. Calculamos el movimiento
        Vector3 movimiento = (direccionFrente * moverZ) + (direccionDerecha * moverX);

        // 5. Movemos al "Jugador" (no a la cámara)
        transform.position += movimiento * velocidad * Time.deltaTime;
    }
}