using UnityEngine;

public class UltimaZona : MonoBehaviour
{
    [Header("Efectos de Victoria")]
    public AudioSource parlante;
    public AudioClip sonidoVictoria; // Un sonido de misión cumplida, aplausos, o radio de rescate
    public Color colorLibre = new Color(0f, 1f, 0f, 0.3f); 
    public MeshRenderer renderizadorZona; 
    public GameDirector gestor;

    private bool simulacionTerminada = false;

    void Start()
    {
        ActualizarColorZona();
    }

    private void OnTriggerEnter(Collider otro)
    {
        // Verificamos que el que entró sea el Jugador (y no una caja o puerta)
        if (!simulacionTerminada && otro.CompareTag("Player"))
        {
            simulacionTerminada = true;
            CompletarSimulacion();
            ApagarEfectoVisual();
            if (gestor != null)
            {
                FindObjectOfType<GestorObjetivos>().MarcarObjetivo("ZonaSegura");
                gestor.TerminarSimulacion();
            }
        }
    }
    public void ActualizarColorZona()
    {
        if (renderizadorZona != null)
        {
            // Cambiamos el color del material dinámicamente
            renderizadorZona.material.color = colorLibre;
        }
    }

    private void CompletarSimulacion()
    {
        Debug.Log("🏆 ¡SIMULACIÓN COMPLETADA! El operario llegó al punto de encuentro sano y salvo.");

        if (parlante != null && sonidoVictoria != null)
        {
            parlante.PlayOneShot(sonidoVictoria);
        }

        // Acá en el futuro podemos hacer que la pantalla se vaya a negro (Fade to Black)
        // o que aparezca un cartel gigante de "Nivel Completado - Tiempo: X minutos".
    }
    private void ApagarEfectoVisual()
    {
        if (renderizadorZona != null)
        {
            renderizadorZona.enabled = false; // Oculta el cubo por completo
        }
    }
}