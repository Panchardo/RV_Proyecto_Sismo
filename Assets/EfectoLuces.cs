using UnityEngine;

public class EfectoLucesSismo : MonoBehaviour
{
    [Header("Conexión al Sismo")]
    public SimuladorSismo scriptPrincipal; // Acá arrastrás tu cerebro del sismo

    [Header("Configuración Eléctrica")]
    public float intensidadMinima = 0.0f; // 0 = apagado total
    public float velocidadFallo = 0.05f;  // Qué tan rápido parpadea (ruido)

    private Light luz;
    private float intensidadOriginal;
    private float temporizador = 0f;

    void Start()
    {
        // Agarramos el componente de luz del objeto
        luz = GetComponent<Light>();
        
        if (luz != null)
        {
            // Guardamos la luz normal de la oficina para restaurarla después
            intensidadOriginal = luz.intensity;
        }
        else
        {
            Debug.LogError("Cuidado: Le pusiste el script de parpadeo a un objeto que no tiene luz.");
        }
    }

    void Update()
    {
        if (luz == null || scriptPrincipal == null) return;

        // Si el sensor del trigger avisa que hay terremoto...
        if (scriptPrincipal.enZonaTerremoto)
        {
            temporizador += Time.deltaTime;
            
            // Generamos la modulación por ancho de pulso (PWM) caótica
            if (temporizador > velocidadFallo)
            {
                // Asigna una intensidad aleatoria entre el mínimo y lo normal
                luz.intensity = Random.Range(intensidadMinima, intensidadOriginal);
                temporizador = 0f;
            }
        }
        else
        {
            // Cuando termina el sismo, la luz vuelve a estar estable al 100%
            luz.intensity = intensidadOriginal;
        }
    }
}