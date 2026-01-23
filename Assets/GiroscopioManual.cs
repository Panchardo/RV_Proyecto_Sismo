using UnityEngine;
using System.Collections; // Necesario para la Coroutine

public class GiroscopioKickstart : MonoBehaviour
{
    private Quaternion rotacionInicial;
    private bool sensorActivo = false;

    IEnumerator Start()
    {
        // 1. ESPERA DE SEGURIDAD (Wait-for-Init)
        // Le damos 1 segundo a Android para que termine de cargar la Activity
        yield return new WaitForSeconds(1.0f);

        // 2. CONFIGURACIÓN DEL SAMPLING
        // Forzamos al sensor a leer 60 veces por segundo (0.016 segs)
        // Sin esto, algunos Motorola se quedan esperando.
        Input.gyro.updateInterval = 0.0167f;

        // 3. ENCENDIDO
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            sensorActivo = true;
            
            // Calibramos
            rotacionInicial = new Quaternion(0, 0, 1, 0);
        }
    }

    void Update()
    {
        if (sensorActivo)
        {
            // Leemos
            Quaternion rot = Input.gyro.attitude;

            // FILTRO DE CEROS (Data Validity Check)
            // Un Quaternion (0,0,0,0) es matemáticamente imposible (error).
            // La identidad es (0,0,0,1).
            if (rot.x != 0 || rot.y != 0 || rot.z != 0 || rot.w != 0)
            {
                // Si entra acá, es que HAY datos reales
                transform.localRotation = rotacionInicial * new Quaternion(rot.x, rot.y, -rot.z, -rot.w);
            }
        }
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.skin.label.fontSize = 40;

        GUILayout.Label("Soporte Gyro: " + SystemInfo.supportsGyroscope);
        GUILayout.Label("Status Activo: " + sensorActivo);
        
        // DEBUG DE DATOS
        if (Input.gyro.attitude.x == 0 && Input.gyro.attitude.y == 0 && Input.gyro.attitude.z == 0 && Input.gyro.attitude.w == 0)
        {
            GUI.color = Color.red;
            GUILayout.Label("GYRO MUERTO (0,0,0,0)");
        }
        else
        {
            GUI.color = Color.green;
            GUILayout.Label("GYRO VIVO: " + Input.gyro.attitude);
        }

        // CONTROL CRUZADO: ACELERÓMETRO
        // Si esto se mueve, el Input System anda bien.
        GUI.color = Color.yellow;
        GUILayout.Label("Accel Test: " + Input.acceleration);
    }
}