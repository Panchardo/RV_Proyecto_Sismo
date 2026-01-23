using UnityEngine;

public class CamaraGyro : MonoBehaviour
{
    private Quaternion rotacionInicial;

    void Start()
    {
        // Activamos el sensor IMU del celular
        Input.gyro.enabled = true;
        
        // Guardamos la rotación inicial para calibrar
        rotacionInicial = transform.rotation;
    }

    void Update()
    {
        // Leemos la orientación del sensor
        // El "ConvertRotation" es porque Android y Unity tienen ejes distintos
        transform.rotation = ConvertRotation(Input.gyro.attitude);
    }

    // Función de corrección de coordenadas (Matemática de cuaterniones)
    private Quaternion ConvertRotation(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w) * Quaternion.Euler(90, 0, 0);
    }
}