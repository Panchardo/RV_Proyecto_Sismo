using UnityEngine;
using UnityEngine.UI;

public class VignetteSismo : MonoBehaviour
{
    public Image[] mascaras; // Arrastrá las dos imágenes acá
    
    [Header("Configuración de Opacidad")]
    [Range(0, 1)] public float opacidadMin = 0.4f; // Casi transparente al estar quieto
    [Range(0, 1)] public float opacidadMax = 1.0f; // Negro total al moverse

    [Header("Sensibilidad")]
    public float suavizado = 5f; // Qué tan rápido reacciona el cambio

    private float alphaObjetivo;
    private float alphaActual;

    void Update()
    {
        // LEEMOS EL JOYSTICK DIRECTAMENTE (Igual que en MovimientoVR.cs)
        float moverX = Input.GetAxis("Horizontal");
        float moverZ = Input.GetAxis("Vertical");
        
        // Calculamos la magnitud del empuje del stick (0 a 1)
        float inputMagnitude = new Vector2(moverX, moverZ).magnitude;
        inputMagnitude = Mathf.Clamp01(inputMagnitude);

        // Si saltás, también podrías querer que se cierre (opcional)
        // if (Input.GetButton("Jump")) inputMagnitude = 1f;

        // Determinamos a qué transparencia queremos llegar
        alphaObjetivo = Mathf.Lerp(opacidadMin, opacidadMax, inputMagnitude);

        // Suavizamos el cambio para que no sea un parpadeo brusco
        alphaActual = Mathf.MoveTowards(alphaActual, alphaObjetivo, suavizado * Time.deltaTime);

        // Aplicamos a las dos cámaras
        foreach (Image img in mascaras)
        {
            Color c = img.color;
            c.a = alphaActual;
            img.color = c;
        }
    }
}