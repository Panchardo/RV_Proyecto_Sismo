using UnityEngine;
using System.Collections;

public class GiroscopioKickstart : MonoBehaviour
{
    [Header("Configuración de Calibración")]
    private Quaternion rotacionInicial;
    private bool sensorActivo = false;

    [Header("Configuración Debug (Joystick/PC)")]
    public float sensibilidadJoystick = 100f;
    private float rotXDebug = 0f;
    private float rotYDebug = 0f;

    [Header("Interfaz de Usuario")]
    private bool mostrarMensajeL3 = false;
    private string textoMensaje = "";

    IEnumerator Start()
    {
        // 1. ESPERA DE SEGURIDAD
        yield return new WaitForSeconds(1.0f);

        // 2. CONFIGURACIÓN DEL SENSOR
        Input.gyro.updateInterval = 0.0167f;

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            sensorActivo = true;
            
            // Esperamos un frame para que el sensor devuelva datos reales
            yield return new WaitForEndOfFrame();
            
            // Calibramos la vista inicial
            CalibrarHaciaAdelante();
        }
    }

    void Update()
    {
        // --- DETECCIÓN DE RE-CALIBRACIÓN (L3) ---
        // "Recenter" debe estar configurado en el Input Manager como 'joystick button 8' (o 9/10)
        if (Input.GetButtonDown("Recenter")|| 
        Input.GetKeyDown(KeyCode.JoystickButton8))
        {
            CalibrarHaciaAdelante();
            StopAllCoroutines(); // Detiene mensajes previos si los hay
            StartCoroutine(MostrarMensajeTemporal("¡Vista Recalibrada!", 2.0f));
        }

        // --- LÓGICA DE ROTACIÓN ---
        if (sensorActivo && !Application.isEditor)
        {
            // MODO CELULAR: Usamos el Giroscopio
            Quaternion rot = Input.gyro.attitude;
            
            // Filtro de seguridad para evitar cuaterniones nulos
            if (rot.x != 0 || rot.y != 0 || rot.z != 0 || rot.w != 0)
            {
                // Aplicamos la rotación corregida para Unity (invirtiendo Z y W)
                transform.localRotation = rotacionInicial * new Quaternion(rot.x, rot.y, -rot.z, -rot.w);
            }
        }
        else
        {
            // MODO EDITOR: Usamos tu lógica de Joystick PS3 / Mouse
            SimularRotacionConJoystick();
        }
    }

    void SimularRotacionConJoystick()
    {
        // Leemos los ejes (configurados en el Input Manager como Mouse X/Y)
        float lookX = Input.GetAxis("Mouse X") * sensibilidadJoystick * Time.deltaTime;
        float lookY = Input.GetAxis("Mouse Y") * sensibilidadJoystick * Time.deltaTime;

        rotYDebug += lookX;
        rotXDebug -= lookY;
        
        // Limitamos la vista vertical para no rotar 360 grados sobre el cuello
        rotXDebug = Mathf.Clamp(rotXDebug, -80f, 80f);

        transform.localRotation = Quaternion.Euler(rotXDebug, rotYDebug, 0f);
    }

    public void CalibrarHaciaAdelante()
    {
        // Solo calibramos si estamos en el celular usando el sensor real
        if (!Application.isEditor)
        {
            // Reset transitorio para capturar el valor base
            transform.localRotation = Quaternion.identity;
            
            Quaternion rot = Input.gyro.attitude;
            // Guardamos la inversa de la posición actual como nuestro nuevo "norte"
            rotacionInicial = Quaternion.Inverse(new Quaternion(rot.x, rot.y, -rot.z, -rot.w));
        }
        else
        {
            // En el editor, simplemente reseteamos nuestras variables de rotación manual
            rotXDebug = 0f;
            rotYDebug = 0f;
            Debug.Log("Recalibración de Debug completada.");
        }
    }

    // --- CORRUTINA PARA EL MENSAJE TEMPORAL ---
    IEnumerator MostrarMensajeTemporal(string mensaje, float tiempo)
    {
        textoMensaje = mensaje;
        mostrarMensajeL3 = true;
        yield return new WaitForSeconds(tiempo);
        mostrarMensajeL3 = false;
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 40;

        // Información de Debug General
        GUI.color = Color.white;
        GUILayout.Label("Soporte Gyro: " + SystemInfo.supportsGyroscope);
        
        if (Application.isEditor)
        {
            GUI.color = Color.yellow;
            GUILayout.Label("MODO: Testeo con Joystick/Mouse");
        }
        else
        {
            GUI.color = sensorActivo ? Color.green : Color.red;
            GUILayout.Label("Sensor Status: " + (sensorActivo ? "VIVO" : "APAGADO"));
        }

        // Mostrar mensaje del L3 si la corrutina lo activa
        if (mostrarMensajeL3)
        {
            GUI.color = Color.cyan;
            GUILayout.Label(textoMensaje);
        }
        // Agregá esto dentro de tu función OnGUI
    }
}