using UnityEngine;

public class DebugGiroscopio : MonoBehaviour
{
    private Quaternion rotacionInicial = Quaternion.identity;
    private Quaternion baseRotation = Quaternion.Euler(90, 0, 0); // Ajuste para modo Landscape
    private bool sensorActivo = false;

    void Start()
    {
        // Activamos el sensor
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            sensorActivo = true;
            
            // Calibramos al inicio
            Recalibrar();
        }
    }

    void Update()
    {
        if (sensorActivo)
        {
            // Leemos la actitud del sensor (Lectura cruda)
            Quaternion rot = Input.gyro.attitude;
            
            // Inversión de ejes necesaria para que Unity entienda el sensor del celular
            Quaternion rotCorregida = new Quaternion(rot.x, rot.y, -rot.z, -rot.w);
            
            // Aplicamos: Offset Inicial * Rotación de Base (90deg) * Lectura corregida
            transform.localRotation = rotacionInicial * baseRotation * rotCorregida;
        }

        // Si tocás la pantalla con el dedo, recalibra el "frente"
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Recalibrar();
        }
    }

    public void Recalibrar()
    {
        Quaternion rot = Input.gyro.attitude;
        Quaternion rotCorregida = baseRotation * new Quaternion(rot.x, rot.y, -rot.z, -rot.w);
        rotacionInicial = Quaternion.Inverse(rotCorregida);
    }

    // --- INTERFAZ DE DEBUG EN PANTALLA ---
    void OnGUI()
    {
        GUI.skin.label.fontSize = 35;
        GUI.color = Color.yellow;

        GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height - 100));
        
        GUILayout.Label("DEBUG GIROSCOPIO");
        GUILayout.Label("-----------------");
        GUILayout.Label("Soporte: " + SystemInfo.supportsGyroscope);
        GUILayout.Label("Activo: " + Input.gyro.enabled);
        
        if (sensorActivo)
        {
            Vector3 angulos = Input.gyro.attitude.eulerAngles;
            GUILayout.Label("Ejes Crudos (Euler):");
            GUILayout.Label("X: " + angulos.x.ToString("F2"));
            GUILayout.Label("Y: " + angulos.y.ToString("F2"));
            GUILayout.Label("Z: " + angulos.z.ToString("F2"));
            
            GUILayout.Space(20);
            GUI.color = Color.cyan;
            GUILayout.Label("TIP: Toca la pantalla para resetear el frente.");
        }
        else
        {
            GUI.color = Color.red;
            GUILayout.Label("ERROR: Sensor no detectado o no soportado.");
        }

        GUILayout.EndArea();
    }
}