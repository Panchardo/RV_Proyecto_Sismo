using UnityEngine;

public class SacudonCamara : MonoBehaviour
{
    public float magnitud = 0.2f; // Qué tan fuerte tiembla la cabeza
    private Vector3 posicionOriginal;

    void Start()
    {
        // Guardamos dónde está la cámara respecto al cuerpo (ej: a 1.7 metros de altura)
        posicionOriginal = transform.localPosition;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            // 1. Generamos un vector aleatorio chiquito (Ruido)
            Vector3 vibracion = Random.insideUnitSphere * magnitud;

            // 2. Se lo sumamos a la posición base
            // Usamos localPosition para que vibre relativo al cuerpo, no al mundo
            transform.localPosition = posicionOriginal + vibracion;
        }
        else
        {
            // 3. Si soltás la T, la cabeza vuelve a su lugar exacto
            transform.localPosition = posicionOriginal;
        }
    }
}