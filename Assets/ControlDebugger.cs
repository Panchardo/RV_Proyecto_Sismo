using UnityEngine;

public class ControladorDebug : MonoBehaviour
{
    void Update()
    {
        // Recorremos todos los posibles botones de un joystick (0 al 19)
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                Debug.Log("BOTÓN DETECTADO: joystick button " + i);
            }
        }
    }
}