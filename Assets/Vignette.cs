using UnityEngine;
using UnityEngine.UI;

public class VignetteController : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController playerController;
    public Image mascaraIzquierda;
    public Image mascaraDerecha;

    [Header("Configuracion")]
    public float velocidadMax = 5.0f; // Ajustala a la de tu MovimientoVR.cs
    public float escalaMin = 1.0f;    // Tamaño normal
    public float escalaMax = 1.8f;    // Cuanto más grande, más se cierra el túnel

    void Update()
    {
        // 1. Obtenemos la velocidad actual del jugador
        float velocidadActual = playerController.velocity.magnitude;

        // 2. Normalizamos el valor entre 0 y 1
        float t = Mathf.Clamp01(velocidadActual / velocidadMax);

        // 3. Calculamos la escala (Interpolación lineal)
        float nuevaEscala = Mathf.Lerp(escalaMin, escalaMax, t);

        // 4. Aplicamos a ambos ojos
        Vector3 escalaFinal = new Vector3(nuevaEscala, nuevaEscala, 1f);
        mascaraIzquierda.transform.localScale = escalaFinal;
        mascaraDerecha.transform.localScale = escalaFinal;
        
        // Opcional: También podemos subir la opacidad (Alpha) al movernos
        Color tempColor = mascaraIzquierda.color;
        tempColor.a = Mathf.Lerp(0.6f, 1.0f, t); // 0.6 es un poco transparente, 1 es negro total
        mascaraIzquierda.color = tempColor;
        mascaraDerecha.color = tempColor;
    }
}