using UnityEngine;

public class GiroManual : MonoBehaviour
{
    public float sensibilidad = 2.0f;
    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Update()
    {
        // Solo rotar si mantenés presionada la tecla Alt (para imitar el simulador)
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            rotacionY += Input.GetAxis("Mouse X") * sensibilidad;
            rotacionX -= Input.GetAxis("Mouse Y") * sensibilidad;
            rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0);
        }
    }
}