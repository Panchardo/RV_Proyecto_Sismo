using UnityEngine;
using TMPro;

public class PanelDialogo : MonoBehaviour
{
    [Header("Referencias Visuales")]
    public TextMeshProUGUI textoPantalla;
    
    [Header("Lista de Mensajes (Páginas)")]
    [TextArea(3, 5)] // Esto te da un cuadro grande en el Inspector para escribir cómodo
    public string[] dialogos;

    [Header("Sonido")]
    public AudioSource parlante;
    public AudioClip sonidoBip; // Un ruidito de computadora o tecla al pasar de página
    public bool esTocable = true;

    private int indiceActual = 0;

    void Start()
    {
        // Al empezar, mostramos la primera página de la lista
        if (dialogos.Length > 0 && textoPantalla != null)
        {
            textoPantalla.text = dialogos[0];
        }
    }

    // ESTA FUNCIÓN LA VA A LLAMAR TU JUGADOR AL "HACER CLIC" CON EL RAYO
    public void AvanzarDialogo()
    {
        if (!esTocable) return;
        if (dialogos.Length == 0) return;

        indiceActual++;
        
        // Si llegamos al final de los mensajes, lo dejamos clavado en el último
        if (indiceActual >= dialogos.Length)
        {
            indiceActual = dialogos.Length - 1; 
            return; // Ya no hace ruidito porque no avanza más
        }

        // Actualizamos el texto en la pantalla
        textoPantalla.text = dialogos[indiceActual];
        
        // Hacemos sonar el bip
        if (parlante != null && sonidoBip != null)
        {
            parlante.PlayOneShot(sonidoBip);
        }
    }
    // --- NUEVA FUNCIÓN PARA CAMBIAR EL TEXTO AUTOMÁTICAMENTE ---
    public void EstablecerPaso(int numeroDePaso)
    {
        if (numeroDePaso >= 0 && numeroDePaso < dialogos.Length)
        {
            indiceActual = numeroDePaso;
            textoPantalla.text = dialogos[indiceActual];
            
            if (parlante != null && sonidoBip != null)
            {
                parlante.PlayOneShot(sonidoBip);
            }
        }
    }
}