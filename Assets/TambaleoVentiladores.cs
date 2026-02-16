using UnityEngine;

public class RotarVentilador : MonoBehaviour
{
    [Header("Conexión al Sismo")]
    public SimuladorSismo scriptPrincipal; 

    [Header("Especificaciones del Motor")]
    public float velocidadRotacion = 150f;

    [Header("Efecto Tambaleo (Sismo)")]
    public float anguloTambaleo = 10f; 
    public float velocidadTambaleo = 25f; 

    private float rotacionY = 0f;
    private Quaternion rotacionBase;
    
    // --- NUEVAS VARIABLES DE ESTADO E INERCIA ---
    private bool sismoYaPaso = false; 
    private float velocidadActual;

    void Start()
    {
        rotacionBase = transform.localRotation;
        rotacionY = transform.localEulerAngles.y;
        
        // Arrancamos a velocidad normal
        velocidadActual = velocidadRotacion;
    }

    void Update()
    {
        float oscilacionX = 0f;
        float oscilacionZ = 0f;

        if (scriptPrincipal != null)
        {
            if (scriptPrincipal.enZonaTerremoto)
            {
                // 1. ESTADO: DURANTE EL SISMO
                sismoYaPaso = true; // El sistema "recuerda" que el evento ya ocurrió
                
                // Modulación de energía: el motor gira más lento por la falla eléctrica
                velocidadActual = Mathf.Lerp(velocidadActual, velocidadRotacion * 0.3f, Time.deltaTime);

                // Tambaleo activo de los anclajes
                oscilacionX = Mathf.Sin(Time.time * velocidadTambaleo) * anguloTambaleo;
                oscilacionZ = Mathf.Cos(Time.time * velocidadTambaleo * 1.2f) * anguloTambaleo;
            }
            else if (sismoYaPaso)
            {
                // 2. ESTADO: DESPUÉS DEL SISMO (Corte de energía / Rotura)
                // El motor se quedó sin energía. Lerp simula la fricción frenando la inercia suavemente hasta 0.
                velocidadActual = Mathf.Lerp(velocidadActual, 0f, Time.deltaTime * 0.5f);
                
                // Las oscilaciones vuelven a 0 porque el techo dejó de temblar
            }
            else
            {
                // 3. ESTADO: ANTES DEL SISMO (Normalidad)
                velocidadActual = velocidadRotacion;
            }
        }

        // Aplicamos el giro acumulando la velocidad actual (sea normal, lenta o frenando)
        rotacionY += velocidadActual * Time.deltaTime;

        // Combinamos la base, el tambaleo y la rotación del motor
        transform.localRotation = Quaternion.Euler(rotacionBase.eulerAngles.x + oscilacionX, rotacionY, rotacionBase.eulerAngles.z + oscilacionZ);
    }
}