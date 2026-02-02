using UnityEngine;

public class DetectorRefugio : MonoBehaviour
{
    public bool estasA_Salvo = false; // Mirá esta casilla en el Inspector al probar

    void OnTriggerEnter(Collider other)
    {
        // Si lo que tocamos tiene la etiqueta correcta...
        if (other.CompareTag("ZonaSegura"))
        {
            estasA_Salvo = true;
            Debug.Log("✅ ¡ESTAS A SALVO!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si salimos de la etiqueta correcta...
        if (other.CompareTag("ZonaSegura"))
        {
            estasA_Salvo = false;
            Debug.Log("⚠️ SALISTE DEL REFUGIO");
        }
    }
}