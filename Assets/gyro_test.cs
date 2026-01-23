using UnityEngine;

public class TestOficialUnity : MonoBehaviour
{
    

    void Start()
    {
        Input.gyro.enabled = true;
    }

    protected void Update()
    {
        GyroModifyCamera();
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUILayout.Label("Orientación: " + Screen.orientation);
        
        // ACÁ ESTÁ LA VERDAD: Si esto sigue en 0, es hardware.
        GUILayout.Label("Actitud: " + Input.gyro.attitude);
    }

    /********************************************/
    // Lógica textual de la documentación de Unity
    
    void GyroModifyCamera()
    {
        // Aplica la rotación convertida al transform actual
        transform.rotation = GyroToUnity(Input.gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        // "The Gyroscope is right-handed. Unity is left handed."
        // Esta es la conversión oficial:
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}