using UnityEngine;

public class PuertaInteractiva : MonoBehaviour
{
    public float anguloAbierta = 90f;
    public float velocidad = 2f;
    private bool abierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        // Guardamos la rotación actual como "Cerrada"
        rotacionCerrada = transform.localRotation;
        // Calculamos la rotación "Abierta" sumando grados en Y
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, anguloAbierta, 0);
    }

    void Update()
    {
        // Lerp suaviza el movimiento para que no sea un salto brusco
        Quaternion objetivo = abierta ? rotacionAbierta : rotacionCerrada;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidad);
    }

    // Esta función la llamaremos desde tu script de interacción
    public void AlternarPuerta()
    {
        abierta = !abierta;
    }
}