using UnityEngine;
using UnityEngine.InputSystem; // Necesario para habilitar el dispositivo en el nuevo sistema

public class GiroscopioManual : MonoBehaviour
{
    private bool giroSoportado;
    private Quaternion rotacionInicial;

    void Start()
    {
        // 1. INTENTO DE DESPERTAR EL SENSOR (Nuevo Input System)
        // Esto es clave: le decimos explícitamente al sistema nuevo que active el giroscopio
        if (UnityEngine.InputSystem.Gyroscope.current != null) {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }

        // 2. CONFIGURACIÓN ESTÁNDAR (Legacy)
        giroSoportado = SystemInfo.supportsGyroscope;

        if (giroSoportado)
        {
            // Usamos "UnityEngine.Input" para que no se confunda con el InputSystem
            UnityEngine.Input.gyro.enabled = true;
            
            // Ajuste de rotación para compensar la diferencia entre Android y Unity (90 grados)
            rotacionInicial = new Quaternion(0, 0, 1, 0); 
        }
    }

    void Update()
    {
        // --- OPCIÓN A: GIROSCOPIO DEL CELULAR ---
        if (giroSoportado && UnityEngine.Input.gyro.enabled)
        {
            // Leemos la actitud usando la librería clásica (que se lleva mejor con Remote)
            Quaternion rot = UnityEngine.Input.gyro.attitude;
            
            // Filtro: Solo aplicamos si la rotación no es cero absoluto (data válida)
            if (rot.x != 0 || rot.y != 0 || rot.z != 0 || rot.w != 0) 
            {
                // Mapeo de coordenadas: Invertimos Z y W para Unity
                transform.localRotation = rotacionInicial * new Quaternion(rot.x, rot.y, -rot.z, -rot.w);
            }
        }

        // --- OPCIÓN B: MOUSE + ALT (RESPALDO PC) ---
        // Esto funciona siempre, tengas o no celular conectado
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            float mouseX = Input.GetAxis("Mouse X") * 2.0f;
            float mouseY = Input.GetAxis("Mouse Y") * 2.0f;
            
            // Rotamos el cuerpo o la cámara manualmente
            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.left, mouseY, Space.Self);
        }

        // --- DEBUG OPTIMIZADO ---
        // Imprime solo 1 vez por segundo (aprox) para no saturar el USB
        if (Time.frameCount % 60 == 0) { 
            Debug.Log("Giro activo: " + giroSoportado + " | Datos: " + UnityEngine.Input.gyro.attitude);
        }
    }
}