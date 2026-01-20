using UnityEngine;

public class DebuggerJoystick : MonoBehaviour
{
    void Update()
    {
        // Esto te dirá en consola exactamente qué eje se está moviendo
        for (int i = 0; i < 10; i++)
        {
            float val = Input.GetAxis("Axis " + i); // Necesitás tener ejes llamados "Axis 0", "Axis 1", etc en el Input Manager
            if (Mathf.Abs(val) > 0.2f) Debug.Log("Eje moviéndose: " + i + " Valor: " + val);
        }
    }
}